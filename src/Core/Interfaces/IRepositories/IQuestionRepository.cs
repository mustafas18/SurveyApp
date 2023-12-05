using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.IRepositories
{
    public interface IQuestionRepository
    {
        Task<string> DeleteAsync(int Id);
    }
}
