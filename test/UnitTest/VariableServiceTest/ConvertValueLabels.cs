using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.IRepositories;
using Infrastructure;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.VariableServiceTest
{
    public class ConvertValueLabels
    {
        [Fact]
        private void ConvertStringToVariables()
        {
            // Arrange
            string inputString = "{0:male,1:female}";
            var varRepository= new Mock<IRepository<Variable>>();
            var sheetRepository = new Mock<ISheetRepository>();
            var varDapperRepository = new Mock<IVariableRepository>();

            var variableService = new Infrastructure.Services.VariableService(varRepository.Object, 
                                                            sheetRepository.Object,
                                                            varDapperRepository.Object);

            // Act
            List<VariableValueLabel> variableValueList = variableService.ConvertStringIntoList(inputString);

            // Asset
            Assert.True(variableValueList.Count == 2);
            Assert.Equal(variableValueList.FirstOrDefault().Label,"male");
        }
    }
}
