using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Language:BaseEntity
    {
        public string Title { get; set; }
        public TextDirectionEnum TextDirection { get; set; }
    }
}
