using Domain.Dtos;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.IRepositories
{
    public interface IQuestionRepository
    {
        Task<(string SheetId,int Version)> DeleteAsync(int Id);
        IEnumerable<QuestionWithAnswerDto> QuestionWithAnswers(string sheetId, int? sheetVersion);
        IEnumerable<UserAnswer> QuestionUserAnswers(int questionId);
    }
}
