using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos
{
    public class FileUploadDto
    {
        public int UserInfoId { get; set; }
        public string FileContent { get; set; }
        public IFormFile? Data { get; set; }
        public byte[]? DataBytes { get; set; }
        public string FileName { get; set; }
    }
}
