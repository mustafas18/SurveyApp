using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.IRepositories
{
    public interface IEfRepository<TEntity> : IRepositoryBase<TEntity> where TEntity : class, IAggregateRoot
    {
        void Add(TEntity entity);
        Task AddAsync(TEntity entity);
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter = null);

    }
}
