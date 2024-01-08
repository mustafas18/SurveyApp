using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UserCategory : BaseEntity
    {
        /// <summary>
        /// This class is an anemic model,
        /// </summary>
        public UserCategory()
        {
                
        }
        public UserCategory(string nameFa,string nameEn)
        {
            NameFa = nameFa;
            NameEn = nameEn;
        }
        public string NameFa { get; set; }
        public string NameEn { get;  set; }
        public bool IsDelete { get;  set; }
    }

}
