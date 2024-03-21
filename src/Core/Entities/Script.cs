using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Script : BaseEntity
    {
        public string SheetId { get; set; }
        public string Code { get; set; }
    }
}
