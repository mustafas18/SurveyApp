using Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.ViewModels
{
    public class UserCategoryViewModel
    {
        public UserCategoryViewModel(string nameFa, string nameEn)
        {
            NameFa = nameFa;
            NameEn = nameEn;
        }
        public string NameFa { get; set; }
        public string NameEn { get; set; }
        public ICollection<UserInfo>? Participants { get; set; }
        public int? ParticipantCount { get; set; }
    }
}
