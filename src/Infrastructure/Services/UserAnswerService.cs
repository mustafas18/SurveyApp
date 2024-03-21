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
        private readonly ISurveyRepository _surveyDataAccess;
        private readonly ISurveyService _surveyService;
        private readonly IRepository<UserSurvey> _surveyRepository;
        private readonly IQuestionRepository _questionDapper;
        private readonly ICSharpCompiler _cSharpCompiler;

        //
        public UserAnswerService(IRepository<UserAnswer> userAnswerRepository,
            IRepository<Question> questionRepository,
            ISurveyRepository surveyDataAccess,
            ISurveyService surveyService,
            IRepository<UserSurvey> surveyRepository,
            IQuestionRepository questionDapper,
            ICSharpCompiler cSharpCompiler)
        {
            _userAnswerRepository = userAnswerRepository;
            _questionRepository = questionRepository;
            _surveyDataAccess = surveyDataAccess;
            _surveyService = surveyService;
            _surveyRepository = surveyRepository;
            _questionDapper = questionDapper;
            _cSharpCompiler = cSharpCompiler;
        }
        public async Task Create(List<UserAnswer> answers)
        {
            var surveyGuid = answers.FirstOrDefault().SurveyGuid;
            var version = _surveyService.GetLatestVersion(surveyGuid) + 1;
            answers.ForEach(a => a.SurveyVersion = version);
            await _userAnswerRepository.AddRangeAsync(answers);
            await _cSharpCompiler.CompileCode(surveyGuid);
        }

        public async Task<List<UserAnswer>> GetBySurveyId(int surveyId)
        {
            var result=await _surveyDataAccess.GetUserAnswersAsync(surveyId);
            return  result.ToList();
        }

        public List<UserQuestionResultDto> ReportBySurveyId(string sheetId, int? version)
        {
            List<UserQuestionResultDto> result = new List<UserQuestionResultDto>();

            var questions = _questionRepository.AsNoTracking().Include("UserAnswers")
                            .Where(s => s.SheetId == sheetId && s.SheetVersion == (version ?? 1) && s.Deleted==false)
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
                var userAnswers =_questionDapper.QuestionUserAnswers(question.Id);
                Dictionary<string, int> answerCount = new Dictionary<string, int>();
                int totalAnswers = userAnswers?.GroupBy(s => new { s.SurveyId,s.SurveyVersion,s.UserName }).Count() ?? 0;
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
