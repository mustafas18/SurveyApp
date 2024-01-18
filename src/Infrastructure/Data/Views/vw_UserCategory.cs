using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Infrastructure.Data.Views
{
    public class vw_UserCategory
    {
        public int Id { get;  }
        public string NameFa { get; }
        public string NameEn { get; }
        public int UserCount { get; }

    }
}
