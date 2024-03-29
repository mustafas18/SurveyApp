﻿using Domain.Dtos;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IVariableService
    {
        Task<Variable> Create(Variable variable);
        Task<bool> DeleteAsync(int variableId);
        Task<Variable> GetByName(string sheetId,string name);
        Task<List<Variable>> GetBySheetId(string sheetId, int? sheetVersion);
        List<VariableValueLabel> ConvertStringIntoList(string values);
        string ConvertValueLabelToString(List<VariableValueLabel> values);
        Task<List<VariableResultDto>> ReportBySurveyId(string sheetId, int? version);
        Task<DataSet> SheetData(string sheetId, int? version);
        Task<IEnumerable<VariableSurveyResultDto>> SurveyReport(int surveyId);

    }
}
