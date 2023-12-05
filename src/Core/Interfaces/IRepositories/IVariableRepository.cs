﻿using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.IRepositories
{
    public interface IVariableRepository
    {
        Task<bool> DeleteAsync(int variableId);
        Task<Variable> GetByName(string sheetId, string name);
        Task<List<Variable>> GetBySheetId(string sheetId, int? sheetVersion);
    }
}
