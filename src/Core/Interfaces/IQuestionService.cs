using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IQuestionService
    {
        Task<Question> CreateAsync(string sheetId, Question question);
    }
}
