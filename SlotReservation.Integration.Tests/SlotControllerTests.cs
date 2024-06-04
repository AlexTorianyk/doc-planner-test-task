using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace SlotReservation.Integration.Tests
{
    public class SlotControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        // I love e2e tests for such projects because you can test the whole system at once. Additionally, you could mock out certain services, use a test url, etc. But the closer you can get to production, the better.
        private readonly WebApplicationFactory<Program> _factory;

        public SlotControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        public class GetAvailability : SlotControllerTests
        {
            public GetAvailability(WebApplicationFactory<Program> factory) : base(factory)
            {
            }

            [Fact]
            public async Task WhenGetAvailabilityWithCorrectDateParam_ShouldReturnOk()
            {
                // Arrange
                var client = _factory.CreateClient();
                int year = 2024;
                int month = 06;
                int day = 03;

                // Act
                var response = await client.GetAsync($"/slot/availability?year={year}&month={month}&day={day}");

                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }

            [Fact]
            public async Task WhenGetAvailabilityWithIncorrectDateParam_ShouldReturnBadRequest()
            {
                // Arrange
                var client = _factory.CreateClient();
                int year = 2024;
                int month = 06;
                int day = 04;

                // Act
                var response = await client.GetAsync($"/slot/availability?year={year}&month={month}&day={day}");

                // Assert
                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            }
        }
    }
}
