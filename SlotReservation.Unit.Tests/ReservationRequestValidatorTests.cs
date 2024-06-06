namespace SlotReservation.Unit.Tests
{
    public class ReservationRequestValidatorTests
    {
      public class ValidateReservationRequest : ReservationRequestValidatorTests
      { 
        private const string defaultFacilityId = "default-facility-id";
        private ReservationRequest CreateReservationRequest(string facilityId = defaultFacilityId, DateTime? start = null, DateTime? end = null)
        {
          start ??= new DateTime(2024, 06, 05, 15, 00, 00);;
          end ??= start.Value.AddHours(1);
        
          return new ReservationRequest
          {
            FacilityId = facilityId,
            Start = start.Value,
            End = end.Value,
            Patient = new Patient()
          };
        }

        private Facility CreateFacility(string facilityId = defaultFacilityId, string name = "Default Facility Name", string address = "Default Facility Address")
        {
          return new Facility
          {
            FacilityId = facilityId,
            Name = name,
            Address = address
          };
        }

        [Fact]
        public void WhenInvalidFacilityIdIsPassed_ShouldReturnError()
        {
          // Arrange
          var reservationRequest = CreateReservationRequest("invalid-facility-id");
          var availability = new Availability { Facility = CreateFacility() };
          var validator = new ReservationRequestValidator();

          // Act
          var (isValid, failureReason) = validator.ValidateReservationRequest(reservationRequest, availability);

          // Assert
          Assert.False(isValid);
          Assert.Equal(ReservationRequestValidationFailureReason.InvalidFacilityId, failureReason);
        }

      [Fact]
      public void WhenEndTimeBeforeStartTime_ShouldReturnError()
      {
        // Arrange
        var reservationRequest = CreateReservationRequest(start: new DateTime(2024, 06, 05, 15, 00, 00), end: new DateTime(2024, 06, 05, 14, 00, 00));
        var availability = new Availability { Facility = CreateFacility() };
        var validator = new ReservationRequestValidator();

        // Act
        var (isValid, failureReason) = validator.ValidateReservationRequest(reservationRequest, availability);

        // Assert
        Assert.False(isValid);
        Assert.Equal(ReservationRequestValidationFailureReason.EndTimeBeforeStartTime, failureReason);
      }

      [Fact]
      public void WhenInvalidSlotDuration_ShouldReturnError()
      {
        // Arrange
        var reservationStart = new DateTime(2024, 06, 05, 15, 00, 00);

        var reservationRequest = CreateReservationRequest(start: reservationStart, end: reservationStart.AddMinutes(30));

        var availability = new Availability { SlotDurationMinutes = 10, Facility = CreateFacility()};
        var validator = new ReservationRequestValidator();

        // Act
        var (isValid, failureReason) = validator.ValidateReservationRequest(reservationRequest, availability);

        // Assert
        Assert.False(isValid);
        Assert.Equal(ReservationRequestValidationFailureReason.InvalidSlotDuration, failureReason);
      }

      [Fact]
      public void WhenDayDoesNotHaveAnyAvailableSlots_ShouldReturnError()
      {
        // Arrange
        var reservationRequest = CreateReservationRequest();
        // Not setting any days of the week, so the work day will be null
        var availability = new Availability
        {
          SlotDurationMinutes = 60, Facility = CreateFacility(),
        };
        var validator = new ReservationRequestValidator();

        // Act
        var (isValid, failureReason) = validator.ValidateReservationRequest(reservationRequest, availability);

        // Assert
        Assert.False(isValid);
        Assert.Equal(ReservationRequestValidationFailureReason.WorkDayNotAvailable, failureReason);
      }

      [Fact]
      public void WhenDuringLunchTime_ShouldReturnError()
      {
        // Arrange
        var reservationStart = new DateTime(2024, 06, 05, 11, 30, 00);
        var reservationRequest = CreateReservationRequest(start: reservationStart, end: reservationStart.AddMinutes(10));
        var availability = new Availability
        {
          SlotDurationMinutes = 10, Facility = CreateFacility(),
          Wednesday = new WorkDay
          {
            WorkPeriod = new WorkPeriod { StartHour = 9, EndHour = 17, LunchStartHour = 11, LunchEndHour = 12},
          }
        };
        var validator = new ReservationRequestValidator();

        // Act
        var (isValid, failureReason) = validator.ValidateReservationRequest(reservationRequest, availability);

        // Assert
        Assert.False(isValid);
        Assert.Equal(ReservationRequestValidationFailureReason.DuringLunchTime, failureReason);
      }

      [Fact]
      public void WhenOutOfWorkingHours_ShouldReturnError()
      {
        // Arrange
        var reservationRequest = CreateReservationRequest(start: new DateTime(2024, 06, 05, 18, 00, 00));
        var availability = new Availability
        {
          SlotDurationMinutes = 60, Facility = CreateFacility(),
          Wednesday = new WorkDay
          {
            WorkPeriod = new WorkPeriod { StartHour = 9, EndHour = 17, LunchStartHour = 11, LunchEndHour = 12},
          }
        };
        var validator = new ReservationRequestValidator();

        // Act
        var (isValid, failureReason) = validator.ValidateReservationRequest(reservationRequest, availability);

        // Assert
        Assert.False(isValid);
        Assert.Equal(ReservationRequestValidationFailureReason.OutOfWorkingHours, failureReason);
      }

      [Fact]
      public void WhenSlotAlreadyReserved_ShouldReturnError()
      {
        // Arrange
        var reservationStart = new DateTime(2024, 06, 05, 15, 00, 00);
        var reservationRequest = CreateReservationRequest(start: reservationStart);
        var availability = new Availability
        {
          SlotDurationMinutes = 60,
          Facility = CreateFacility(),
          Wednesday = new WorkDay
          {
            WorkPeriod = new WorkPeriod { StartHour = 9, EndHour = 17, LunchStartHour = 11, LunchEndHour = 12 },
            BusySlots = new List<BusySlot> { new BusySlot { Start = reservationStart, End = reservationStart.AddHours(1) } }
          }
        };
        var validator = new ReservationRequestValidator();

        // Act
        var (isValid, failureReason) = validator.ValidateReservationRequest(reservationRequest, availability);

        // Assert
        Assert.False(isValid);
        Assert.Equal(ReservationRequestValidationFailureReason.SlotAlreadyReserved, failureReason);
      }
    }
  }
}
