using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Language:BaseEntity
    {
        public Language(int id,string title, TextDirectionEnum textDirection)
        {
            base.Id = id;
            Title = title;
            TextDirection = textDirection;
        }
        public string Title { get; set; }
        public TextDirectionEnum TextDirection { get; set; }
    }
}
