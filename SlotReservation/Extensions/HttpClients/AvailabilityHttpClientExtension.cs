// registering a named HttpClient for the AvailabilityService
public static class AvailabilityHttpClientExtension
{
    public static void AddAvailabilityHttpClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient("AvailabilityService", client =>
          {
              var baseAddress = configuration["AvailabilityService:BaseAddress"];
              if (string.IsNullOrWhiteSpace(baseAddress))
              {
                  throw new InvalidOperationException("AvailabilityService:BaseAddress is required");
              }
              client.BaseAddress = new Uri(baseAddress);
              client.DefaultRequestHeaders.Add("Accept", "application/json");
              client.DefaultRequestHeaders.Add("Authorization", configuration["AvailabilityService:AuthKey"]);
          });
    }
}
