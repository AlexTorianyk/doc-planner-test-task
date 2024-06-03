
public class AvailabilityService : IAvailabilityService, IScoped
{
    private readonly IHttpClientFactory _clientFactory;

    public AvailabilityService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public async Task<Availability> GetAvailability(string date)
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
        else{
            throw new BadRequestException(response.ReasonPhrase);
        }
    }

    public Task ReserveSlot(ReservationRequest slot)
    {
        throw new NotImplementedException();
    }
}
