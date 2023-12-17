using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class SheetPage:BaseEntity
    {
        public string SheetId { get; set; }
        public int SheetVersion { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string HtmlText { get; set; }
        public string ThumbnailBase64 { get; set; }
       
    }
}
