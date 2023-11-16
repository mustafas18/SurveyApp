using Core.Entities;

namespace WebApi.ViewModels
{
    public class UserFullNameViewModel
    {
        public UserFullNameViewModel(string id,string fullName)
        {
            Id = id;
            FullName = fullName;
        }
        public UserFullNameViewModel(UserInfo userInfo)
        {
            Id = userInfo.AppUserId;
            FullName = userInfo.FullName;
        }
        public string Id { get; set; }
        public string FullName { get; set; }
    }
}
