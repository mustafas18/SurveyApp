using Domain.Dtos;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class UserInfoService : IUserInfoService
    {
        private readonly IRepository<UserInfo> _userInfoRepository;
        private readonly IUserRepository _userRepository;

        public UserInfoService(IRepository<UserInfo> userInfoRepository,
            IUserRepository userRepository
            )
        {
            _userInfoRepository = userInfoRepository;
            _userRepository = userRepository;
        }
        public List<UserInfo> GetUserInfoList()
        {
            var user = _userInfoRepository.AsNoTracking().ToList();
            return user;
        }
        public UserInfo GetUserInfo(int userId)
        {
            var user = _userInfoRepository.Include("Category").FirstOrDefault(u => u.Id == userId);
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
         await _userRepository.UpdateInfo(userInfo);
        }

        public async Task<bool> UploadCV(FileUploadDto fileUploadDto)
        {
            var userInfo = _userInfoRepository.FirstOrDefault(s => s.Id == fileUploadDto.UserInfoId);
            userInfo.CVFileData = fileUploadDto.DataBytes;
            userInfo.FileContent = fileUploadDto.FileContent;
            await _userInfoRepository.UpdateAsync(userInfo);
            return true;
        }

        public FileUploadDto DownloadCV(int userInfoId)
        {
            var userInfo = _userInfoRepository.FirstOrDefault(s => s.Id == userInfoId);
            var result = new FileUploadDto
            {
                DataBytes = userInfo.CVFileData,
                FileContent = userInfo.FileContent,
                UserInfoId = userInfoId
            };
            return result;
        }
    }
}
