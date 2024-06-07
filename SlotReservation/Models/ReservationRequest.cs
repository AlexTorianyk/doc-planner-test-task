using System.ComponentModel.DataAnnotations;

public class ReservationRequest
{
    [Required(ErrorMessage = "FacilityId is required.")]
    public required string FacilityId { get; set; }

    [Required(ErrorMessage = "Start date is required.")]
    public DateTime Start { get; set; }

    [Required(ErrorMessage = "End date is required.")]
    public DateTime End { get; set; }

    public string? Comments { get; set; }

    [Required(ErrorMessage = "Patient is required.")]
    public required Patient Patient { get; set; }
}
