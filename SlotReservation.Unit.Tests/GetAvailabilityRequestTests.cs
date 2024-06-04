using Xunit;
using System;

namespace SlotReservation.Unit.Tests
{
    public class GetAvailabilityRequestTests
    {
        public class Constructor : GetAvailabilityRequestTests
        {
            [Fact]
            public void WhenValidDateIsPassed_ShouldSetDateProperty()
            {
                // Arrange
                int year = 2024;
                int month = 06;
                int day = 03; // This is a Monday

                // Act
                var request = new GetAvailabilityRequest(year, month, day);

                // Assert
                Assert.Equal(new DateTime(year, month, day), request.Date);
            }

            [Fact]
            public void WhenNonMondayIsPassed_ShouldThrowBadRequestException()
            {
                // Arrange
                int year = 2024;
                int month = 06;
                int day = 04; // This is not a Monday

                // Act & Assert
                Assert.Throws<BadRequestException>(() => new GetAvailabilityRequest(year, month, day));
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
                int day = 03; // This is a Monday
                var request = new GetAvailabilityRequest(year, month, day);

                // Act
                var result = request.ToString();

                // Assert
                Assert.Equal("20240603", result);
            }
        }
    }
}
