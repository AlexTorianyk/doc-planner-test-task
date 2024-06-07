using Xunit;
using System;
using System.Collections.Generic;

namespace SlotReservation.Unit.Tests
{
    public class WorkDayResponseTests
    {
        [Fact]
        public void WhenWorkPeriodIsProvided_ShouldCalculateAvailableSlots()
        {
            // Arrange
            var workDayResponse = new WorkDayResponse
            {
                Day = DayOfWeek.Monday,
                WorkPeriod = new WorkPeriod
                {
                    StartHour = 9,
                    EndHour = 17,
                    LunchStartHour = 12,
                    LunchEndHour = 13
                }
            };

            // Act
            workDayResponse.CalculateAvailableSlots(60);

            // Assert
            Assert.NotNull(workDayResponse.AvailableSlots);
            Assert.Equal(7, workDayResponse.AvailableSlots.Count); // assuming 60-minute slots and 1-hour lunch break
        }

        [Fact]
        public void WhenBusySlotsAreProvided_ShouldExcludeFromAvailableSlots()
        {
            // Arrange
            var workDayResponse = new WorkDayResponse
            {
                Day = DayOfWeek.Monday,
                WorkPeriod = new WorkPeriod
                {
                    StartHour = 9,
                    EndHour = 17,
                    LunchStartHour = 12,
                    LunchEndHour = 13
                },
                BusySlots = new List<TimeSlot>
            {
                new TimeSlot { Start = new DateTime().AddHours(10), End = new DateTime().AddHours(11) }
            }
            };

            // Act
            workDayResponse.CalculateAvailableSlots(60);

            // Assert
            Assert.NotNull(workDayResponse.AvailableSlots);
            Assert.DoesNotContain(workDayResponse.AvailableSlots, slot => slot.Start.ToTimeSpan().Hours == 10);
        }
    }
}
