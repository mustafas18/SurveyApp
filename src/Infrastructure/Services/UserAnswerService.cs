﻿using Domain.Dtos;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class UserAnswerService : IUserAnswerService
    {
        private readonly IRepository<UserAnswer> _userAnswerRepository;
        private readonly IRepository<Question> _questionRepository;

        public UserAnswerService(IRepository<UserAnswer> userAnswerRepository,
            IRepository<Question> questionRepository)
        {
            _userAnswerRepository = userAnswerRepository;
            _questionRepository = questionRepository;
        }
        public async Task Create(List<UserAnswer> answers)
        {
            await _userAnswerRepository.AddRangeAsync(answers);
        }

        public async Task<List<UserAnswer>> GetBySurveyId(int surveyId)
        {
            return await _userAnswerRepository
                            .AsNoTracking()
                            .Where(s => s.SurveyId == surveyId)
                            .ToListAsync();
        }

        public List<UserQuestionResultDto> ReportBySurveyId(string sheetId, int? version)
        {
            List<UserQuestionResultDto> result = new List<UserQuestionResultDto>();

            var questions = _questionRepository.AsNoTracking().Include("UserAnswers")
                            .Where(s => s.SheetId == sheetId && s.SheetVersion == (version ?? 1))
                            .Select(q => new Question
                            {
                                Id = q.Id,
                                Text = q.Text,
                                UserAnswers = q.UserAnswers
                            })
                            .ToList();

            foreach (var question in questions)
            {
                var answers = question.UserAnswers;
                Dictionary<string, int> answerCount = new Dictionary<string, int>();
                int totalAnswers = answers?.GroupBy(s=> new { s.SurveyId }).Count() ?? 0;
                // Count the frequency of each answer
                foreach (var answer in answers)
                {
                    if (answer != null)
                    {
                        if (answerCount.ContainsKey(answer.InputValue))
                        {
                            answerCount[answer.InputValue]++;
                        }
                        else
                        {
                            answerCount[answer.InputValue] = 1;
                        }
                    }
                }
                List<UserAnswerResultDto> answerDtos = new List<UserAnswerResultDto>();
                foreach (var answer in answerCount)
                {
                    string? answerLabel = answers.FirstOrDefault(s => s.InputValue == answer.Key)?.InputLabel;
                    answerDtos.Add(new UserAnswerResultDto(answer.Key, answer.Value, answerLabel));
                }
                result.Add(new UserQuestionResultDto(question.Id, question.Text, totalAnswers, answerDtos.OrderByDescending(s=>s.Count).ToList()));
            }

            return result;
        }
    }
}