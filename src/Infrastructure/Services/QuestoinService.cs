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
    public class QuestoinService : IQuestionService
    {
        private readonly IEfRepository<Question> _questionRepository;
        private readonly ISheetRepository _sheetRepository;

        public QuestoinService(IEfRepository<Question> questionRepository,
            ISheetRepository sheetRepository)
        {
            _questionRepository = questionRepository;
            _sheetRepository = sheetRepository;
        }
        public async Task<Question> CreateAsync(string sheetId, Question question)
        {
            question.SheetVersion = _sheetRepository.GetLatestVersion(sheetId);
            await _questionRepository.AddAsync(question);
            return question;
        }
    }
}
