using Core.Entities;
using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.RepositoryTest
{
    public class EfRepositoryTest
    {

        [Fact]
        public void AddSheet_Should_Call_DbContext()
        {
            // Arrange
            var dbContext = new Mock<AppDbContext>();
            var dbSetCourseMock = new Mock<DbSet<Sheet>>();
            dbContext.Setup(x => x.Set<Sheet>()).Returns(dbSetCourseMock.Object);
            var sheet = new Sheet { Id = 1, Title = "Sheet1" };
            var sheet2 = new Sheet { Id = 2, Title = "Sheet2" };

            // Act
            var repository = new EfRepository<Sheet>(dbContext.Object);
            repository.AddAsync(sheet);

            //Assert
            dbContext.Verify(x => x.Set<Sheet>());
        }
        [Fact]
        public void AddSheet_Should_Add_Sheet1_To_dbSet()
        {

            // Arrange
            var dbContext = new Mock<AppDbContext>();
            var dbSetUserMock = new Mock<DbSet<Sheet>>();
            dbContext.Setup(x => x.Set<Sheet>()).Returns(dbSetUserMock.Object);
            var sheet = new Sheet { Id = 1, Title = "Sheet1" };
            var course2 = new Sheet { Id = 2, Title = "Sheet2" };

            // Act
            var repository = new EfRepository<Sheet>(dbContext.Object);
            repository.AddAsync(sheet);

            //Assert
            dbSetUserMock.Verify(x => x.Add(It.Is<Sheet>(y => y == sheet)));
        }

    }
}
