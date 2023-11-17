using Ardalis.GuardClauses;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.IRepositories;
using Infrastructure.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class VariableService : IVariableService
    {
        private readonly IRepository<Variable> _varRepository;
        private readonly IVariableRepository _varDapperRepository;
        private readonly ISheetRepository _sheetRepository;

        public VariableService(IRepository<Variable> varRepository,
            ISheetRepository sheetRepository,
            IVariableRepository varDapperRepository)
        {
            _varRepository = varRepository;
            _varDapperRepository = varDapperRepository;
            _sheetRepository = sheetRepository;
        }

  
        public async Task<Variable> Create(Variable variable)
        {
            Guard.Against.Null(variable);
            variable.Values = ConvertStringIntoList(variable.ValuesAsString);
            variable.SheetVersion = _sheetRepository.GetLatestVersion(variable.SheetId);
            await _varRepository.AddAsync(variable);
            return variable;
        }
        public async Task<Variable> GetByName(string sheetId,string name)
        {
            Guard.Against.Null(name);
            var result = await _varDapperRepository.GetByName(sheetId,name);
            return result;
        }

        public async Task<List<Variable>> GetBySheetId(string sheetId, int? sheetVersion)
        {
            Guard.Against.Null(sheetId);
            var result = await _varDapperRepository.GetBySheetId(sheetId, sheetVersion);
            if (result != null)
            {
                result.ForEach(s => s.ValuesAsString = ConvertValueLabelToString(s.Values));
            }
            return result;
        }
        public List<VariableValueLabel> ConvertStringIntoList(string values)
        {
            if (String.IsNullOrEmpty(values))
            {
                return  null;
            }

            values = values.Trim('{', '}');

            string[] keyValuePairs = values.Split(',');

            List<VariableValueLabel> valueList = new List<VariableValueLabel>();

            foreach (string pair in keyValuePairs)
            {
                // Split each pair by colon ':'
                string[] keyValue = pair.Split(':');

                // Trim any whitespaces from the key and value
                int key = int.Parse(keyValue[0].Trim());
                string value = keyValue[1].Trim('\'');

                valueList.Add(new VariableValueLabel(key, value));
            }
            return valueList;
        }
        public string ConvertValueLabelToString(List<VariableValueLabel> values)
        {
            if (values == null)
            {
                return "";
            }
            StringBuilder str=new StringBuilder("{");

            values.ForEach(v=>str.Append($"{v.Label}:{v.Value},"));
            str.Remove(str.Length-1,str.Length);
            str.Append("}");
            return str.ToString();
        }

    }
}
