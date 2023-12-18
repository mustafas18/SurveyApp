using Ardalis.Specification;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Specifications
{
  public sealed class SheetWithQuestions : Specification<Sheet>, ISingleResultSpecification 
    {
        public SheetWithQuestions(int sheetId)
        {
            Query
                .Where(s => s.Id == sheetId)
                .Include(s => s.Questions);
        }
    }
}
