using Core.Models;
using Infrastructure.Data.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.RepositoryTest
{
    public class SheetTest
    {
        [Fact]
        public void GetSheetByID_Should_Return_Sheet()
        {
            // Arrange
            var products = new List<Sheet>
            {
                new Sheet {  Id=1, SheetId="de", Name="Sheet1" },
                new Sheet {  Id=2, SheetId="ab", Name="Sheet2" }
            };
           

            // Act

            // Assert

        }
    }
}
