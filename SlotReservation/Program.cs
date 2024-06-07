var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDependencies();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddProblemDetails();
builder.Services.AddControllers();
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddHealthChecks();

builder.Services.AddAvailabilityHttpClient(builder.Configuration);

var app = builder.Build();

app.UseExceptionHandler("/error");
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();

// Needed for WebApplicationFactory e2e tests
public partial class Program { }
