using Domain.Dtos;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IQuestionService
    {
        Task<string> CreateAsync(string sheetId, Question question);
        Task<List<Question>> UpdateQuestionOrder(QuestionOrderDto question);
        Task<bool> DeleteQuestionAsync(int questionId);
        int CountSheetQuestion(string sheetId, int? sheetVersion);
        Task<IEnumerable<QuestionDto>> GetBySheetId(string sheetId, int? sheetVersion);
    }
}
