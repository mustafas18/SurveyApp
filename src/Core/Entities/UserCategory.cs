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
        public UserCategory()
        {
                
        }
        public UserCategory(string nameFa,string nameEn)
        {
            NameFa = nameFa;
            NameEn = nameEn;
        }
        public string NameFa { get;private set; }
        public string NameEn { get; private set; }
        public bool IsDelete { get; private set; }
        public void Delete()
        {
            IsDelete = true;
        }
    }

}
