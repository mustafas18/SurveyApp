using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Serialization;
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
        [JsonIgnore]
        public bool IsDelete { get;  set; }
        [NotMapped]
        public int Participants { get; set; }
    }

}
