using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.IRepositories
{
    public interface IUserCategoryRepository
    {
        Task<IEnumerable<UserCategory>> GetListAsync(int? page);
        Task<IEnumerable<UserInfo>> GetCategoryUsers(int categoryId);
    }
}
