using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Presentation;
using Domain.Dtos;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Interfaces.IRepositories;
using Infrastructure.Data.Repositories;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ScriptService : ICSharpCompiler
    {
        private readonly IVariableRepository _variableRepository;
        private readonly IRepository<Sheet> _sheetRepository;
        private readonly IRepository<UserSurvey> _surveyRepository;

        public ScriptService(IVariableRepository variableRepository,
            IRepository<Sheet> sheetRepository,
            IRepository<UserSurvey> surveyRepository)
        {
            _variableRepository = variableRepository;
            _sheetRepository = sheetRepository;
            _surveyRepository = surveyRepository;
        }
        private static List<MetadataReference> DefaultReferences = new List<MetadataReference>()
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.Runtime.CompilerServices.DynamicAttribute).Assembly.Location),
                MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.Console")).Location),
                MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.Runtime")).Location),
                MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.Data")).Location),
                MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.Data.Common")).Location),
                MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.ComponentModel.DataAnnotations")).Location),
                MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.ComponentModel.Primitives")).Location),
                MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.ComponentModel.TypeConverter")).Location),
                MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.Text.Json")).Location),
                MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.Net.Http")).Location),
                MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.Threading.Tasks")).Location),
                MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("Microsoft.CSharp")).Location),
                MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.Private.Uri")).Location),
                MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.Collections")).Location),
                MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("netstandard")).Location)
            };
        private async Task<string> sourceCode(string refId, string script, CompileEnvironment compileEnvironment)
        {
            StringBuilder code = new StringBuilder();
            code.Append(@"using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Runtime;
using System.Data;
using System.Data.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.CSharp;
using System.Text.Json.Serialization;
using System.Collections;
using System.IO;
using System.Text;
using System.Text.Json;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Dynamic;
namespace Code {
            public class VariableDto {
                public int Id {get;set;}
                public string Name {get;set;}
                public int? DataType {get;set;}
                public int Value {get;set;}

                public VariableDto(int id,string name,int dataType,int value)
                {
                    Id = id;
                    Name = name;
                    DataType = dataType;
                    Value = value;
                }
            }
            public partial class Program
            {

");
            var variables = new List<VariableSurveyResultDto>();
            if (compileEnvironment == CompileEnvironment.Execute)
            {
                variables = _variableRepository.GetSurveyVariableData(refId).Result.ToList();
            }
            else
            {
                variables = _variableRepository.GetSheetVariableData(refId).Result.ToList();
            }
            foreach (var variable in variables)
            {
                var fieldLine = variable.Type switch
                {
                    VariableTypeEnum.Numeric => $"public static int {variable.Name} = {variable.Sum};",
                    _ => throw new NotImplementedException()
                };
                code.AppendLine(fieldLine);
            }
            code.Append(@"public List<VariableDto> Main(){");
            code.Append(script);
            code.AppendLine("List<VariableDto> variableDtos = new List<VariableDto>(){");
            foreach (var p in variables)
            {
                if (!p.ReadOnly)
                {
                    code.AppendLine($"new VariableDto({p.Id},\"{p.Name}\",{(int)p.Type},{p.Name}),");
                }

            }
            code.AppendLine("};");
            code.AppendLine(@"return variableDtos;}
                         }//end class
                        }//endNameSpace");
            return code.ToString();
        }

        public async Task<List<CompilerDiagnoisticDto>> CompileCode(string refId, string script, CompileEnvironment compileEnvironment)
        {
            List<MetadataReference> nativeReferences = new List<MetadataReference>();
            var nativeReferencesBinaries = new List<byte[]>();

            CSharpCompilation compilation = CSharpCompilation.Create(
           Path.GetRandomFileName(),
           syntaxTrees: new[] { CSharpSyntaxTree.ParseText(await sourceCode(refId, script, compileEnvironment)) },
           references: DefaultReferences.Concat(nativeReferences),
           options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            var errorList = new List<CompilerDiagnoisticDto>();

            Thread thread = new Thread(() =>
            {
                using (var ms = new MemoryStream())
                {
                    try
                    {
                        EmitResult result = compilation.Emit(ms);
                        if (!result.Success)
                        {
                            foreach (var err in result.Diagnostics)
                            {
                                errorList.Add(new CompilerDiagnoisticDto(err.Severity, err.GetMessage().ToString()));
                            }
                            //throw new Exception(error.ToString());
                        }
                        else
                        {
                            ms.Seek(0, SeekOrigin.Begin);

                            var assemblyContext = new AssemblyLoadContext(null, true);
                            var assembly = assemblyContext.LoadFromStream(ms);

                            nativeReferencesBinaries.ForEach(p =>
                            {
                                using var stream = new MemoryStream(p);

                                assemblyContext.LoadFromStream(stream);
                            });

                            Type type = assembly.GetType("Code.Program");
                            var methodOutput = type.InvokeMember("Main",
                                    BindingFlags.Default | BindingFlags.InvokeMethod,
                                    null,
                                    Activator.CreateInstance(type),
                                    null);
                            if (compileEnvironment == CompileEnvironment.Execute)
                            {
                                UpdateVariables(methodOutput, refId);
                            }
                            assemblyContext.Unload();
                            errorList.Add(new CompilerDiagnoisticDto(DiagnosticSeverity.Info, "Ok"));
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            );

            thread.Start();

            if (!thread.Join(TimeSpan.FromSeconds(6)))
            {
                thread.Interrupt();
            }
            return errorList;
        }
        public async Task CompileCode(string guidId)
        {
            var sheetId = _surveyRepository.FirstOrDefault(s => s.Guid == guidId).SheetId;
            var latestVersion = _sheetRepository.Where(s2 => s2.SheetId == sheetId).Max(s2 => s2.Version);
            var script = _sheetRepository.FirstOrDefault(s => s.SheetId == sheetId && s.Version == latestVersion).Script?.Code;
            if (script != null && script != "")
            {
                await CompileCode(guidId, script, CompileEnvironment.Execute);
            }

        }
        private async Task UpdateVariables(object variables, string guidId)
        {
            if (variables != null)
            {
                List<VariableSurveyResultDto> variableList = System.Text.Json.JsonSerializer.Deserialize<List<VariableSurveyResultDto>>(System.Text.Json.JsonSerializer.Serialize(variables));
                await _variableRepository.UpdateVariables(variableList, guidId);
            }
        }

        public async Task<List<CompilerDiagnoisticDto>> TestCode(string sheetId, string script)
        {
            return await CompileCode(sheetId, script, CompileEnvironment.Test);
        }
    }

}
