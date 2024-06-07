// This is so the tests aren't bound to the message, but the status code. This way, you can change the message without breaking the tests.
public static class ReservationRequestValidationFailureReason
{
    public const string EndTimeBeforeStartTime = "End time is before start time";
    public const string OutOfWorkingHours = "Reservation is outside of working hours";
    public const string DuringLunchTime = "Reservation is during lunch time";
    public const string InvalidSlotDuration = "Invalid slot duration";
    public const string SlotAlreadyReserved = "Slot is already taken";
    public const string InvalidFacilityId = "Invalid facility id";
    public const string WorkDayNotAvailable = "Work day is not available";
}
