using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.IRepositories
{
    public interface IDapperRepository<T> where T : class
    {

        Task<IEnumerable<T>> GetAll();
        Task<bool> DeleteAsync(int Id);
    }
}
