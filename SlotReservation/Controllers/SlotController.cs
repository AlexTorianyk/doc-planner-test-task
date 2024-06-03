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

  [HttpGet("availability/{date}")]
  public async Task<IActionResult> GetAvailability(string date)
  {
    var availability = await _availabilityService.GetAvailability(date);
    
    return Ok(availability);    
  }
}
