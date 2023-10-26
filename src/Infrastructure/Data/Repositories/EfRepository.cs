using Infrastructure.Data;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces.IRepositories;
using Ardalis.Specification.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;
public class EfRepository<TEntity> :RepositoryBase<TEntity>
{
    private readonly AppDbContext _context;

    public EfRepository(AppDbContext dbContext) : base(dbContext)
    {
        _context = dbContext;
    }

}