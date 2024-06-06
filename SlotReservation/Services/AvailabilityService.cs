
using System.Text;
using System.Text.Json;

public class AvailabilityService : IAvailabilityService, ITransient
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly IReservationRequestValidator validator;

    public AvailabilityService(IHttpClientFactory clientFactory, IReservationRequestValidator validator)
    {
        _clientFactory = clientFactory;
        this.validator = validator;
    }

    public async Task<Availability> GetAvailability(GetAvailabilityRequest date)
    {
      using var client = _clientFactory.CreateClient("AvailabilityService");

      var response = await client.GetAsync($"GetWeeklyAvailability/{date}");

      if (response.IsSuccessStatusCode)
      {
        var availability = await response.Content.ReadFromJsonAsync<Availability>();

        if (availability is null)
        {
          throw new NotFoundException("Availability not found");
        }

        return availability;
      }
      else
      {
        var errorMessage = await response.Content.ReadAsStringAsync();
        if(!string.IsNullOrEmpty(errorMessage))
        {
          throw new BaseHttpException(errorMessage, response.StatusCode);
        }

        throw new BaseHttpException("Availability HTTP Service error", response.StatusCode);
      }
    }

    public async Task ReserveSlot(ReservationRequest slot)
    {
        var json = JsonSerializer.Serialize(slot);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var getAvailabilityRequest = new GetAvailabilityRequest(slot.Start.Year, slot.Start.Month, slot.Start.Day);

        var availability = await GetAvailability(getAvailabilityRequest);

        var (isValid, failureReason) = validator.ValidateReservationRequest(slot, availability);

        if (!isValid)
        {
          throw new BadRequestException(failureReason);
        }

        using var client = _clientFactory.CreateClient("AvailabilityService");

        var response = await client.PostAsync("TakeSlot", content);

        if (response.IsSuccessStatusCode)
        {
          return;
        }
        else
        {
          var errorMessage = await response.Content.ReadAsStringAsync();
          if (!string.IsNullOrEmpty(errorMessage))
          {
            throw new BaseHttpException(errorMessage, response.StatusCode);
          }

          throw new BaseHttpException("Reservation HTTP Service error", response.StatusCode);
        }
    }
}
