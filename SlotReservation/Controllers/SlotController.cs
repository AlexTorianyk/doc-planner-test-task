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
    
    return Ok(availability);    
  }
}
