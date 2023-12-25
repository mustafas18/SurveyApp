using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class UserInfoService: IUserInfoService
    {
        private readonly IRepository<UserInfo> _userInfoRepository;

        public UserInfoService(IRepository<UserInfo> userInfoRepository )
        {
            _userInfoRepository = userInfoRepository;
        }
        public List<UserInfo> GetUserInfoList()
        {
            var user = _userInfoRepository.AsNoTracking().ToList();
            return user;
        }
        public UserInfo GetUserInfo(int userId)
        {
            var user = _userInfoRepository.AsNoTracking().FirstOrDefault(u => u.Id == userId);
            return user;
        }
        public UserInfo GetUserInfo(string userName)
        {
            var user = _userInfoRepository.AsNoTracking().FirstOrDefault(u => u.UserName == userName);
            return user;
        }
        public async Task AddUserInfo(UserInfo userInfo)
        {
            await _userInfoRepository.AddAsync(userInfo);
        }
        public async Task UpdateUserInfo(UserInfo userInfo)
        {
            await _userInfoRepository.UpdateAsync(userInfo);
        }
    }
}
