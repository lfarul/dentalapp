using Xunit;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using DentalApp;

namespace XUnitTestProject
{
    public class BasicTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        public BasicTest(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/")]
        [InlineData("/Home")]
        [InlineData("/Home/About")]
        [InlineData("/Account/Register")]
        [InlineData("/Account/Login")]
        [InlineData("/Employee/List")]
        [InlineData("/Employee/NewList")]
        [InlineData("/Appointment/CreateAppointment")]
        [InlineData("/Appointment/GetAllAppointments")]
        public async Task GetHttpRequest(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Index_WhenCalled_ReturnsApplicationForm()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/WebApi/GetUsers");

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Contains("Joanna", responseString);
            Assert.Contains("Lineker", responseString);
        }
    }
}
