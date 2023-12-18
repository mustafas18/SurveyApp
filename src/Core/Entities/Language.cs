using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    /// <summary>
    /// Encapsulating fields for setting and providing access to those fields via public methods.
    /// </summary>
    public class Language : BaseEntity
    {
        public Language(int id,string title, TextDirectionEnum textDirection)
        {
            base.Id = id;
            Title = title;
            TextDirection = textDirection;
        }
        public string Title { get; private set; }
        public TextDirectionEnum TextDirection { get; private set; }
    }
}
