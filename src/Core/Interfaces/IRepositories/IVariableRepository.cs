using Domain.Dtos;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.IRepositories
{
    public interface IVariableRepository
    {
        Task<bool> DeleteAsync(int variableId);
        Task<Variable> GetByName(string sheetId, string name);
        Task<IEnumerable<VariableValueLabel>> GetVariableValues(int variableId);
        Task<IEnumerable<VariableWithValuesDto>> GetVariableWithValues(string sheetId, int? sheetVersion);
        Task<IEnumerable<Variable>> GetBySheetId(string sheetId, int? sheetVersion);
        Task<DataTable> GetSurveyData(string sheetId);
        IEnumerable<VariableAnswerDto> VariableAnswers(string sheetId, int? sheetVersion);
        Task<IEnumerable<VariableSurveyResultDto>> SurveyVariableReportAsync(int surveyId);
        Task<IEnumerable<VariableSurveyResultDto>> GetSurveyVariableData(string surveyGuid);
        Task<IEnumerable<VariableSurveyResultDto>> GetSheetVariableData(string sheetId);
        Task<IEnumerable<VariableSurveyResultDto>> UpdateVariables(List<VariableSurveyResultDto> variables, string guidId);
    }
}
