using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Text;
using System.Text.Json;

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
                int year = -1; // This is an invalid year
                int month = 06;
                int day = 04;

                // Act
                var response = await client.GetAsync($"/slot/availability?year={year}&month={month}&day={day}");

                // Assert
                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            }
        }

    public class ReserveSlotTests : SlotControllerTests
    {
        public ReserveSlotTests(WebApplicationFactory<Program> factory) : base(factory)
        {
        }

        // We'd need a cancellation endpoint to run this test multiple times, otherwise the slot will be busy
        // [Fact]
        // public async Task WhenReserveSlotWithValidRequest_ShouldReturnOk()
        // {
        //     // Arrange
        //     var client = _factory.CreateClient();
        //     var reservationRequest = new ReservationRequest
        //     {
        //         FacilityId = "38df4bee-26d9-4feb-a49c-cc49e078982c", // Set the FacilityId property
        //         Patient = new Patient(),
        //         Start = new DateTime(2024, 06, 03, 10, 0, 0),
        //         End = new DateTime(2024, 06, 03, 10, 10, 0)
        //     };
        //     var json = JsonSerializer.Serialize(reservationRequest);
        //     var content = new StringContent(json, Encoding.UTF8, "application/json");

        //     // Act
        //     var response = await client.PostAsync("/slot/reserve", content);

        //     // Assert
        //     Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        // }

        [Fact]
        public async Task WhenReserveSlotWithInvalidRequest_ShouldReturnBadRequest()
        {
            // Arrange
            var client = _factory.CreateClient();
            var reservationRequest = new ReservationRequest
            {
                FacilityId = "123", // Set the FacilityId property
                Start = new DateTime(2024, 06, 03, 10, 0, 0),
                End = new DateTime(2024, 06, 03, 9, 0, 0), // End time is before start time, which is invalid,
                Patient = new Patient
                {
                    Name = "John Doe",
                    Email = "johndoe@example.com",
                    Phone = "1234567890"
                }
            };
            var json = JsonSerializer.Serialize(reservationRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/slot/reserve", content);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        }
    }
}
