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

        [Fact]
        public void AddSheet_Should_Call_DbContext()
        {
            // Arrange
            var dbContext = new Mock<AppDbContext>();
            var dbSetCourseMock = new Mock<DbSet<Course>>();
            dbContext.Setup(x => x.Set<Course>()).Returns(dbSetCourseMock.Object);
            var course = new Course { Id = 1, TitleEn = "Course1" };
            var course2 = new Course { Id = 2, TitleEn = "Course2" };

            // Act
            var repository = new Repository<Course>(dbContext.Object);
            repository.Add(course);

            //Assert
            dbContext.Verify(x => x.Set<Course>());
        }
        [Fact]
        public void AddSheet_Should_Add_Sheet1_To_dbSet()
        {

            // Arrange
            var dbContext = new Mock<AppDbContext>();
            var dbSetUserMock = new Mock<DbSet<Course>>();
            dbContext.Setup(x => x.Set<Course>()).Returns(dbSetUserMock.Object);
            var course = new Course { Id = 1, TitleEn = "Course1" };
            var course2 = new Course { Id = 2, TitleEn = "Course2" };

            // Act
            var repository = new Repository<Course>(dbContext.Object);
            repository.Add(course);

            //Assert
            dbSetUserMock.Verify(x => x.Add(It.Is<Course>(y => y == course)));
        }

    }
}
