using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class SlotController : ControllerBase
{
    private readonly IAvailabilityService _availabilityService;

    public SlotController(IAvailabilityService availabilityService)
    {
        _availabilityService = availabilityService;
    }

    [HttpGet("availability")]
    public async Task<IActionResult> GetAvailability(int year, int month, int day)
    {
        var request = new GetAvailabilityRequest(year, month, day);

        var availability = await _availabilityService.GetAvailability(request);

        var availabilityResponse = new AvailabilityResponse(availability);

        return Ok(availabilityResponse);
    }

    [HttpPost("reserve")]
    public async Task<IActionResult> ReserveSlot([FromBody] ReservationRequest slot)
    {
        await _availabilityService.ReserveSlot(slot);

        return Ok();
    }
}
