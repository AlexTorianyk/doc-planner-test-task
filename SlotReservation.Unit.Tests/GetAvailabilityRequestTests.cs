namespace SlotReservation.Unit.Tests
{
    public class GetAvailabilityRequestTests
    {
        public class Constructor : GetAvailabilityRequestTests
        {
            [Fact]
            public void WhenMondayIsPassed_ShouldReturnThatSameMonday()
            {
                // Arrange
                int year = 2024;
                int month = 06;
                int day = 03; // This is a Monday

                // Act
                var request = new GetAvailabilityRequest(year, month, day);

                // Assert
                Assert.Equal(new DateTime(year, month, 03), request.LatestMonday); // The Monday of that week is June 3, 2024
            }


            [Fact]
            public void WhenNonMondayIsPassed_ShouldSetDatePropertyToMondayOfThatWeek()
            {
                // Arrange
                int year = 2024;
                int month = 06;
                int day = 05; // This is a Wednesday

                // Act
                var request = new GetAvailabilityRequest(year, month, day);

                // Assert
                Assert.Equal(new DateTime(year, month, 03), request.LatestMonday); // The Monday of that week is June 3, 2024
            }

            [Fact]
            public void WhenInvalidYearIsPassed_ShouldThrowBadRequestException()
            {
                // Arrange
                int year = -1; // This is an invalid year
                int month = 06;
                int day = 03; 

                // Act & Assert
                Assert.Throws<BadRequestException>(() => new GetAvailabilityRequest(year, month, day));
            }

            [Fact]
            public void WhenInvalidMonthIsPassed_ShouldThrowBadRequestException()
            {
                // Arrange
                int year = 2024;
                int month = 13; // This is an invalid month
                int day = 03; 

                // Act & Assert
                Assert.Throws<BadRequestException>(() => new GetAvailabilityRequest(year, month, day));
            }

            [Fact]
            public void WhenInvalidDayIsPassed_ShouldThrowBadRequestException()
            {
                // Arrange
                int year = 2024;
                int month = 06;
                int day = 32; // This is not a valid day

                // Act & Assert
                Assert.Throws<BadRequestException>(() => new GetAvailabilityRequest(year, month, day));
            }
        }

        public class ToString : GetAvailabilityRequestTests
        {
            [Fact]
            public void WhenToStringIsCalled_ShouldReturnCorrectFormat()
            {
                // Arrange
                int year = 2024;
                int month = 06;
                int day = 05; // This is a Wednesday
                var request = new GetAvailabilityRequest(year, month, day);

                // Act
                var result = request.ToString();

                // Assert
                Assert.Equal("20240603", result); // The Monday of that week is June 3, 2024
            }
        }
    }
}
