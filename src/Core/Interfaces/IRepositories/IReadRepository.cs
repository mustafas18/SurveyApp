using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.IRepositories
{
    public interface IReadRepository<T> : IReadRepositoryBase<T> where T : class
    {
    }
}
