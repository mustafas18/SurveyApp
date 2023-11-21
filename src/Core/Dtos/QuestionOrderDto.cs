using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos
{
    public class QuestionOrderDto
    {
        public string SheetId;
        public List<QuestionOrderQDto> Questions;
    }
    public class QuestionOrderQDto
    {
        public int Id { get; set; }
        public int order { get; set; }
    }
}
