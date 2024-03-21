using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Script : BaseEntity
    {
        public Script()
        {
            
        }
        public Script(string code)
        {
            Code = code;
        }
        public string Code { get; set; }
    }
}
