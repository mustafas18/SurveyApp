using AutoMapper;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using WebApi;
using WebApi.Controllers;
using WebApi.ViewModels;

namespace FunctionalTests.Controllers
{
    public class VariablesContorlerTest : IClassFixture<WebApplicationFactory<WebMarker>>
    {
        private readonly HttpClient _client;
        public VariablesContorlerTest(WebApplicationFactory<WebMarker> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetBySheetId()
        {
            var result =await _client.GetAsync("/api/Variable/GetBySheetId");
            Assert.True(result.IsSuccessStatusCode);
        }
        [Fact]
        public async Task GetBySheetId_Returns_List_of_Sheets()
        {
            // Arrange
            var _mockVariableService = new Mock<IVariableService>();
            var _mockMapper = new Mock<IMapper>();
            var controller = new VariableController(_mockVariableService.Object, _mockMapper.Object);

            // Act
            var result = await controller.GetBySheetId("u8y",1);

            // Assert
            var okObjResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<IEnumerable<VariableViewModel>>(okObjResult.Value);
        }
    }
}