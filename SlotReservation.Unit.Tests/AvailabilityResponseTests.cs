namespace SlotReservation.Unit.Tests
{
    public class AvailabilityResponseTests
    {
      public class Constructor : AvailabilityResponseTests{

      
        [Fact]
        public void WhenAvailabilityIsProvided_ShouldSetFacility()
        {
            // Arrange
            var availability = new Availability
            {
                Facility = new Facility { FacilityId = "facility-id", Name = "facility-name", Address = "facility-address" }
            };

            // Act
            var response = new AvailabilityResponse(availability);

            // Assert
            Assert.Equal(availability.Facility, response.Facility);
        }

        [Fact]
        public void WhenAvailabilityIsProvided_ShouldSetSchedule()
        {
            // Arrange
            // Arrange
            var availability = new Availability
            {
                Facility = new Facility { FacilityId = "facility-id", Name = "facility-name", Address = "facility-address" },
                Monday = new WorkDay(),
                Tuesday = new WorkDay()
            };

            // Act
            var response = new AvailabilityResponse(availability);

            // Assert
            Assert.NotNull(response.Schedule);
            Assert.Equal(2, response.Schedule.Count);
        }
      }
    }
}
