public interface IReservationRequestValidator
{
    (bool isValid, string failureReason) ValidateReservationRequest(ReservationRequest reservationRequest, Availability availability);
}
