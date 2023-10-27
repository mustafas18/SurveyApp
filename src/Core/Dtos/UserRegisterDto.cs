using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Core.Dtos
{
    public class UserRegisterDto
    {
      
            public string FullName { get; set; }
            [Required]
            public string UserName { get; set; }

            [Required]
            public string? Mobile { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
            [DataType(DataType.Password)]
            public string Password { get; set; }

        
    }
}
