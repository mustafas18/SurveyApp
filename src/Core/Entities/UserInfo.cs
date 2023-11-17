using Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class UserInfo : BaseEntity
    {
        public string AppUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [NotMapped]
        public string FullName { get { return $"{FirstName} {LastName}"; } }
        public GenderEnum Gender { get; set; }
        public string? Birthday { get; set; }
        public string? PictureBase64 { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public int? ResearchInterestId { get; set; }
        public List<UserDegree>? EducationDegree { get; set; }
        public string? Grade { get; set; }
        public string? Job { get; set; }
        public string? Mobile { get; set; }
        public List<Sheet>? Sheets { get; set; }
        public string? Address { get; set; }
        public string? AtmCard { get; set; }
        public byte[]? CVFileData { get; set; }
        public string? FileContent { get; set; }
        public bool Deleted { get; set; }
    }
}
