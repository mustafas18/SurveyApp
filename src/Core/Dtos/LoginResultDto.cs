using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos
{
    public class LoginResultDto
    {
        public string UserName { get; set; }
        public List<string> UserRoles { get; set; }
        public string AccessToken { get; set; }
        public DateTime IssuedDate { get; set; }
    }
}
