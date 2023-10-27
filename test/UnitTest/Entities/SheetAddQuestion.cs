using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Entities
{
    public class SheetAddQuestion
    {
        private readonly string _sheetId = "abcd-efgh";
        [Fact]
        public void AddsSheetQuestuinIfNotPresent()
        {
            // Arrange
            var sheet = new Sheet();

            // Act
            sheet.AddItem(_sheetId, new Question {SheetId = _sheetId, Id=1 });
            var firstQuestion = sheet.Questions.Single();

            // Asset
            Assert.Equal(_sheetId, firstQuestion.SheetId);
        }

    }
}
