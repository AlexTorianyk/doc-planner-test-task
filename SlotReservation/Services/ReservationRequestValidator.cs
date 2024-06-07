public class ReservationRequestValidator : ITransient, IReservationRequestValidator
{
    public (bool isValid, string failureReason) ValidateReservationRequest(ReservationRequest reservationRequest, Availability availability)
    {
        if (reservationRequest.FacilityId != availability.Facility.FacilityId)
        {
            return (false, ReservationRequestValidationFailureReason.InvalidFacilityId);
        }

        if (isEndTimeBeforeStartTime(reservationRequest))
        {
            return (false, ReservationRequestValidationFailureReason.EndTimeBeforeStartTime);
        }

        if (IsSlotDurationValid(reservationRequest, availability))
        {
            return (false, ReservationRequestValidationFailureReason.InvalidSlotDuration);
        }

        var requestedWorkDay = GetRequestedWorkDay(reservationRequest, availability);

        if (requestedWorkDay is null || requestedWorkDay.WorkPeriod is null)
        {
            return (false, ReservationRequestValidationFailureReason.WorkDayNotAvailable);
        }

        if (IsDuringLunchTime(reservationRequest, requestedWorkDay))
        {
            return (false, ReservationRequestValidationFailureReason.DuringLunchTime);
        }

        if (IsOutOfWorkingHours(reservationRequest, requestedWorkDay))
        {
            return (false, ReservationRequestValidationFailureReason.OutOfWorkingHours);
        }

        if (IsSlotAlreadyReserved(reservationRequest, requestedWorkDay))
        {
            return (false, ReservationRequestValidationFailureReason.SlotAlreadyReserved);
        }

        return (true, string.Empty);
    }

    private bool IsSlotAlreadyReserved(ReservationRequest reservationRequest, WorkDay workDay)
    {
        if (workDay.BusySlots is null || !workDay.BusySlots.Any())
        {
            return false;
        }
        return workDay.BusySlots.Any(slot =>
          slot.Start < reservationRequest.End &&
          slot.End > reservationRequest.Start);
    }

    private bool IsOutOfWorkingHours(ReservationRequest reservationRequest, WorkDay requestedWorkDay)
    {
        return reservationRequest.Start.Hour < requestedWorkDay.WorkPeriod.StartHour || reservationRequest.End.Hour > requestedWorkDay.WorkPeriod.EndHour;
    }

    private static bool IsSlotDurationValid(ReservationRequest reservationRequest, Availability availability)
    {
        return reservationRequest.End - reservationRequest.Start != TimeSpan.FromMinutes(availability.SlotDurationMinutes);
    }

    private static bool isEndTimeBeforeStartTime(ReservationRequest reservationRequest)
    {
        return reservationRequest.End < reservationRequest.Start;
    }

    private WorkDay? GetRequestedWorkDay(ReservationRequest reservationRequest, Availability availability)
    {
        switch (reservationRequest.Start.DayOfWeek)
        {
            case DayOfWeek.Monday:
                return availability.Monday;
            case DayOfWeek.Tuesday:
                return availability.Tuesday;
            case DayOfWeek.Wednesday:
                return availability.Wednesday;
            case DayOfWeek.Thursday:
                return availability.Thursday;
            case DayOfWeek.Friday:
                return availability.Friday;
            case DayOfWeek.Saturday:
                return availability.Saturday;
            case DayOfWeek.Sunday:
                return availability.Sunday;
            default:
                return null;
        }
    }

    // checks if the reservation ends before lunch starts or starts after lunch ends
    private bool IsDuringLunchTime(ReservationRequest reservationRequest, WorkDay requestedWorkDay)
    {
        TimeSpan lunchStart = new TimeSpan(requestedWorkDay.WorkPeriod.LunchStartHour, 0, 0);
        TimeSpan lunchEnd = new TimeSpan(requestedWorkDay.WorkPeriod.LunchEndHour, 0, 0);

        return reservationRequest.End.TimeOfDay > lunchStart &&
               reservationRequest.Start.TimeOfDay < lunchEnd;
    }
}
