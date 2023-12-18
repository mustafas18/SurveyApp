using Domain.Dtos;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.IRepositories
{
    public interface IVariableRepository
    {
        Task<bool> DeleteAsync(int variableId);
        Task<Variable> GetByName(string sheetId, string name);
        Task<IEnumerable<Variable>> GetBySheetId(string sheetId, int? sheetVersion);
        IEnumerable<VariableViewDto> VariableAnswers(string sheetId, int? sheetVersion);
    }
}
