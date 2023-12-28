using Domain.Dtos;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Interfaces.IRepositories;
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
        private readonly ISurveyService _surveyService;
        private readonly IRepository<UserSurvey> _surveyRepository;
        private readonly IQuestionRepository _questionDapper;
        //
        public UserAnswerService(IRepository<UserAnswer> userAnswerRepository,
            IRepository<Question> questionRepository,
            ISurveyService surveyService,
            IRepository<UserSurvey> surveyRepository,
            IQuestionRepository questionDapper)
        {
            _userAnswerRepository = userAnswerRepository;
            _questionRepository = questionRepository;
            _surveyService = surveyService;
            _surveyRepository = surveyRepository;
            _questionDapper = questionDapper;
        }
        public async Task Create(List<UserAnswer> answers)
        {
            var version = _surveyService.GetLatestVersion(answers.FirstOrDefault().SurveyGuid) + 1;
            answers.ForEach(a => a.SurveyVersion = version);
            await _userAnswerRepository.AddRangeAsync(answers);
        }

        public async Task<List<UserAnswer>> GetBySurveyId(int surveyId)
        {
            var surveyGuid= _surveyService.GetSurveyGuid(surveyId);
            return await _userAnswerRepository
                            .AsNoTracking()
                            .Where(s => s.SurveyId == surveyId && s.SurveyVersion == _surveyService.GetLatestVersion(surveyGuid))
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
                                UserAnswers = q.UserAnswers,
                                Answers = q.Answers,
                            })
                            .ToList();
            var answers = _questionDapper.QuestionWithAnswers(sheetId, version);
            foreach (var question in questions)
            {
                if (question.Type == QuestionTypeEnum.TextInput || question.Type == QuestionTypeEnum.TextArea)
                {
                    continue;
                }
                var questionAnswers = answers.Where(s => s.QuestionId == question.Id);
                var userAnswers = question.UserAnswers?.Where(s => s.QuestionType != QuestionTypeEnum.TextInput && s.QuestionType != QuestionTypeEnum.TextArea && s.SurveyVersion == _surveyService.GetLatestVersion(s.SurveyId));
                Dictionary<string, int> answerCount = new Dictionary<string, int>();
                int totalAnswers = userAnswers?.GroupBy(s => new { s.SurveyId }).Count() ?? 0;
                // Count the frequency of each answer
                foreach (var answer in userAnswers)
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
                foreach (var ans in questionAnswers)
                {
                    if (userAnswers.Where(s => s.AnswerId == ans.AnswerId).Count() == 0)
                    {
                        answerDtos.Add(new UserAnswerResultDto("_null_", 0, ans.AnswerText));
                    }
                }
                foreach (var answer in answerCount)
                {
                    string? answerLabel = userAnswers.FirstOrDefault(s => s.InputValue == answer.Key)?.InputLabel;
                    answerDtos.Add(new UserAnswerResultDto(answer.Key, answer.Value, answerLabel));
                }
                result.Add(new UserQuestionResultDto(question.Id, question.Text, totalAnswers, answerDtos.OrderByDescending(s => s.Count).ToList()));
            }

            return result;
        }
    }
}
