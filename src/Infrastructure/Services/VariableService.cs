using Ardalis.GuardClauses;
using Domain.Dtos;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Interfaces.IRepositories;
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
        private readonly IVariableRepository _varDataAcess;
        private readonly ISheetRepository _sheetRepository;

        public VariableService(IRepository<Variable> varRepository,
            ISheetRepository sheetRepository,
            IVariableRepository varDataAccess)
        {
            _varRepository = varRepository;
            _varDataAcess = varDataAccess;
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
        public async Task<bool> DeleteAsync(int variableId)
        {
            await _varDataAcess.DeleteAsync(variableId);
            return true;
        }
        public async Task<Variable> GetByName(string sheetId, string name)
        {
            Guard.Against.Null(name);
            var result = await _varDataAcess.GetByName(sheetId, name);
            return result;
        }

        public async Task<List<Variable>> GetBySheetId(string sheetId, int? sheetVersion)
        {
            Guard.Against.Null(sheetId);
            var result = await _varDataAcess.GetBySheetId(sheetId, sheetVersion);
            return result.ToList();
        }
        public List<VariableValueLabel> ConvertStringIntoList(string values)
        {
            if (String.IsNullOrEmpty(values))
            {
                return null;
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
            StringBuilder str = new StringBuilder("{");

            values.ForEach(v => str.Append($"{v.Label}:{v.Value},"));
            str.Remove(str.Length - 1, str.Length);
            str.Append("}");
            return str.ToString();
        }

        public async Task<List<VariableResultDto>> ReportBySurveyId(string sheetId, int? version)
        {
            List<VariableViewDto> varsResult = _varDataAcess.VariableAnswers(sheetId, version).ToList();
            var variables = await _varDataAcess.GetBySheetId(sheetId, version);
            var result = new List<VariableResultDto>();
            foreach (var variable in variables)
            {
                var res = varsResult.Where(s => s.VariableId == variable.Id).ToList();

                var varResult = new VariableResultDto(variable.Id, variable.Name, variable.Messure, variable.Label);
                res.ForEach(s =>
                {
                    varResult.AddAnswer(new VariableAnswer(s.InputValue, s.AnswerLabel, s.AnswerCount));
                });
                result.Add(varResult);
            }

            return result;
        }
    }
}