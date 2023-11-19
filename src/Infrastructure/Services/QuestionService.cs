using Core.Dtos;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IRepository<Question> _questionRepository;
        private readonly ISheetRepository _sheetRepository;

        public QuestionService(IRepository<Question> questionRepository,
            ISheetRepository sheetRepository)
        {
            _questionRepository = questionRepository;
            _sheetRepository = sheetRepository;
        }
        public async Task<string> CreateAsync(string sheetId, Question question)
        {
            question.SheetId = sheetId;
            question.UserId = "";
            question.SheetVersion = _sheetRepository.GetLatestVersion(sheetId);
            await _questionRepository.AddAsync(question);
            return "OK";
        }

        public async Task<IEnumerable<QuestionDto>> GetBySheetId(string sheetId, int? sheetVersion)
        {
            if (sheetVersion != null)
            {
                return _questionRepository
                      .Where(s => s.SheetId == sheetId && s.SheetVersion == sheetVersion && s.Deleted==false)
                      .Select(q => new QuestionDto
                      {
                          Text = q.Text,
                          Required = q.Required,
                          Type = q.Type,
                          VariableId = q.VariableId,
                          Order = q.Order
                      })
                      .AsEnumerable();
            }
            else
            {
                return _questionRepository
                      .Where(s => s.SheetId == sheetId && s.Deleted == false)
                      .Select(q => new QuestionDto
                      {
                          Text = q.Text,
                          Required = q.Required,
                          Type = q.Type,
                          VariableId = q.VariableId,
                          Order = q.Order
                      })
                      .AsEnumerable();
            }

        }
    }
}
