using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class UserInfo:BaseEntity
    {

            public string AppUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public string Birthday { get; set; }

        public string Country { get; set; }
        public string City { get; set; }
        public int ResearchInterestId { get; set; }
        public int EducationDegreeId { get; set; }
        public List<string> Grade { get; set; }
        public string Job { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string AtmCard { get; set; }
        public byte[] CVFileData { get; set; }
        public string FileContent { get; set; }
    }
}
