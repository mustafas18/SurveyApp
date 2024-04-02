using Domain.Dtos;
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
        List<UserInfo> GetUserInfoList();
        UserInfo GetUserInfo(string userName);
        UserInfo GetUserInfo(int userId);
        Task AddUserInfo(UserInfo userInfo);
        Task UpdateUserInfo(UserInfo userInfo);
        Task<bool> UploadCV(FileUploadDto fileUploadDto);
        FileUploadDto DownloadCV(int userInfoId);
    }
}
