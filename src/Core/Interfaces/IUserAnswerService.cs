﻿using Domain.Dtos;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUserAnswerService
    {
        Task Create(List<UserAnswer> answers);
        Task<List<UserAnswer>> GetBySurveyId(int surveyId);
        List<UserQuestionResultDto> ReportBySurveyId(string sheetId, int? version);
    }
}
