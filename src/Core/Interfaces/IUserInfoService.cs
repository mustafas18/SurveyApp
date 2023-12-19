using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUserInfoService
    {
        UserInfo GetUserInfo(string userName);
        List<UserInfo> GetUserInfoList();
        Task AddUserInfo(UserInfo userInfo);
    }
}
