using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UserDegreeMajor : BaseEntity
    {
        public UserDegree Degree { get; set; }
        public string MajorTitle { get; set; }
        public string UserName {  get; set; }
    }
}
