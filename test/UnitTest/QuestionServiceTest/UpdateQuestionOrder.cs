using Core.Entities;
using Core.Interfaces.IRepositories;
using Core.Interfaces;
using Infrastructure.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Infrastructure.Services;
using Core.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using MediatR;

namespace UnitTest.QuestionServiceTest
{
    public class UpdateQuestionOrder
    {
        [Fact]
        public void UpdateQuestionOrder_Should_Return_Ordered_Questions()
        {
            // Arrange
            List<QuestionOrderQDto> questionOrderQDto = new List<QuestionOrderQDto> {
                new QuestionOrderQDto{ Id=2, order=1},
                new QuestionOrderQDto{ Id=3, order=2},
                new QuestionOrderQDto{ Id=1, order=3}
            };
            var questionOrderDto = new QuestionOrderDto { SheetId = "h4f", Questions = questionOrderQDto };
            Sheet sheet=new Sheet { Id = 1, SheetId = "h4f", Version = 1, Title = "Test Sheet" };
            sheet.AddQuestions(new List<Question> {
                new Question { Id = 1, Order = 1, Text="Q1" },
                new Question { Id = 2, Order = 2, Text = "Q2" },
                new Question { Id = 3, Order = 3, Text = "Q3" }
            });

            var mockQuestionRepository = new Mock<IRepository<Question>>();
            var mockRedisCache=new Mock<IRedisCacheService>();
            var mockSheetRepository = new Mock<IRepository<Sheet>>();
            var mockSheetDapperRepository = new Mock<ISheetRepository>();
            var mockQuestionRepo=new Mock<IQuestionRepository>();
            var mockMediator = new Mock<IMediator>();
            mockSheetDapperRepository.Setup(s => s.GetLatestVersion(It.IsAny<string>())).Returns(1);
            mockSheetRepository.SetReturnsDefault(sheet);

            var questionService = new QuestionService(mockQuestionRepository.Object,
                mockSheetRepository.Object, mockQuestionRepo.Object, mockMediator.Object, mockSheetDapperRepository.Object);

            // Act
            var question = questionService.UpdateQuestionOrder(questionOrderDto);

            // Assert
            Assert.True(question != null);
            Assert.Equal("Q2", question.FirstOrDefault().Text);
            
        }
    }
}
