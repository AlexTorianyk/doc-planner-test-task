public interface IAvailabilityService
{
    Task<Availability> GetAvailability(GetAvailabilityRequest date);
    Task ReserveSlot(ReservationRequest slot);
}
