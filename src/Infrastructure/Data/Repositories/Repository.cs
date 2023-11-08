using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Infrastructure.Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly AppDbContext _context;
        public Repository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
            _context.SaveChanges();
        }

        public async Task AddAsync(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public List<TEntity> GetAll()
        {
            return _context.Set<TEntity>().ToList();
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            //try
            //{
                _context.Set<TEntity>().Update(entity);
                await _context.SaveChangesAsync();
            return entity;
            //}
            //catch (DbUpdateConcurrencyException ex)
            //{
            //    if (entity is Course)
            //    {
            //        _context.Database.ExecuteSqlAsync("");
            //        await _context.SaveChangesAsync();
            //    }
            //    }

            return entity;
        }

        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> filter = null,
                            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                            string includeProperties = "",
                            int first = 0, int offset = 0)
        {
            if (filter != null)
            {
                return _context.Set<TEntity>().Where(filter);
            }
            return null;


        }
        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> filter = null)
        {
            if (filter != null)
            {
                return _context.Set<TEntity>().FirstOrDefault(filter);
            }
            return _context.Set<TEntity>().FirstOrDefault();
        }
        public IQueryable<TEntity> OrderBy(Expression<Func<TEntity, bool>> filter = null)
        {
                return _context.Set<TEntity>().OrderBy(filter);
        }
        public TEntity LastOrDefault()
        {
            return _context.Set<TEntity>().LastOrDefault();
        }

        public IQueryable<TEntity> Include([NotParameterized] string entityProperties)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();
            foreach (var includeProperty in entityProperties.Split(",", StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public IQueryable<TEntity> AsNoTracking()
        {
            return _context.Set<TEntity>().AsNoTracking();
        }
    }
}
