using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Entities
{
    public class SheetTest
    {
        private readonly string _sheetId = "abcd-efgh";
        [Fact]
        public void AddsSheetQuestuinIfNotPresent()
        {
            // Arrange
            var sheet = new Sheet();

            // Act
            sheet.AddQuestion(new Question {SheetId = _sheetId, Id=1 });
            var firstQuestion = sheet.Questions.Single();

            // Asset
            Assert.Equal(_sheetId, firstQuestion.SheetId);
        }
        [Fact]
        public void AddQuestions_Should_Add_Questions_To_Sheet()
        {
            // Arrange
            var sheet = new Sheet();

            // Act
            sheet.AddQuestions(new List<Question> {
                new Question { SheetId = _sheetId, Id = 1 },
                new Question { SheetId = _sheetId, Id = 1 }
            });
            var actual = sheet.Questions.Count();

            // Asset
            Assert.Equal(2, actual);
        }
        [Fact]
        public void ClearQuestions_Should_Remove_SheetQuestions()
        {

            // Arrange
            var sheet = new Sheet();
            sheet.AddQuestions(new List<Question> {
                new Question { SheetId = _sheetId, Id = 1 },
                new Question { SheetId = _sheetId, Id = 1 }
            });

            // Act
            sheet.ClearQuestions();

            // Asset
            Assert.Equal(0, sheet.Questions.Count());
        }

    }
}
