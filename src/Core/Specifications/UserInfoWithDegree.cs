using Ardalis.Specification;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Specifications
{
    public class UserInfoWithDegree : Specification<UserInfo>, ISingleResultSpecification
    {
        public UserInfoWithDegree(int userId)
        {
            Query
               .Where(s => s.Id == userId)
               .Include(s => s.EducationDegree);
        }
    }
}
