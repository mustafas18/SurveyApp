﻿using Ardalis.GuardClauses;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class VariableService : IVariableService
    {
        private readonly IEfRepository<Variable> _varRepository;
        private readonly ISheetRepository _sheetRepository;

        public VariableService(IEfRepository<Variable> varRepository,
            ISheetRepository sheetRepository)
        {
            _varRepository = varRepository;
            _sheetRepository = sheetRepository;
        }
        public async Task<Variable> Create(Variable variable)
        {
            Guard.Against.Null(variable);
            variable.SheetVersion = _sheetRepository.GetLatestVersion(variable.SheetId);
            await _varRepository.AddAsync(variable);
            return variable;
        }
    }
}
