using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace Domain.Entities
{
    /// <summary>
    /// Encapsulating fields using private setter, then providing access to those fields via public methods.
    /// Encapsulation keeps the data and code safe within the class itself.
    /// </summary>
    public class UserDegree : BaseEntity
    {
        public UserDegree()
        {
                
        }
        public UserDegree(int id, string titleFa, string titleEn)
        {
            Id = id;
            TitleFa = titleFa;
            TitleEn = titleEn;
        }

        public string TitleFa { get;  set; }
        public string TitleEn { get;  set; }
    }
}
