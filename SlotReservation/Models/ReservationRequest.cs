public class ReservationRequest
{
    public required string FacilityId { get; set; }
    public required DateTime Start { get; set; }
    public required DateTime End { get; set; }
    public string? Comments { get; set; }
    public required Patient Patient { get; set; }
}
