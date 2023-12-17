using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UserDegree : BaseEntity
    {
        public UserDegree(int id,string titleFa, string titleEn)
        {
            base.Id= id;
            TitleFa = titleFa;
            TitleEn = titleEn;
        }

        public string TitleFa { get; set; }
        public string TitleEn { get; set; }
    }
}
