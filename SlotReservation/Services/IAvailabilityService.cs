public interface IAvailabilityService
{
    Task<Availability> GetAvailability(string date);
    Task ReserveSlot(ReservationRequest slot);
}
