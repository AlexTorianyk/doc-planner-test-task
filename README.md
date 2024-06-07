# DocPlanner test task for .NET developer

This readme will include setup instructions, suggested request payloads, and brief comments on some of my decisions.

## Setup

### Install .NET 8

To install .NET 8, follow these steps:

1. Visit the official .NET website at [dotnet.microsoft.com](https://dotnet.microsoft.com/).
2. Navigate to the Downloads section.
3. Select the .NET version 8 download option.
4. Choose the appropriate installer for your operating system (Windows, macOS, or Linux).
5. Download and run the installer.
6. Follow the installation wizard instructions to complete the installation process.

Once the installation is complete, you can verify the installation by opening a command prompt or terminal and running the following command:

```
dotnet --version
```

If the command returns the version number of .NET 8, then the installation was successful.

### Clone the Repository

Run `git clone https://github.com/AlexTorianyk/doc-planner-test-task.git`

### Run the Application

Navigate to the project directory and run the following command:

```bash
dotnet run --project .\SlotReservation\SlotReservation.csproj
```

The application will start and listen on port 5165 by default. Go to `http://localhost:5165/swagger/index.html` to access the Swagger UI and test the API endpoints.

## Suggested Request Payloads

### Get availability

This accepts any date and returns the availability for that week.

### Reserve slot

You'll have to adjust the facility id and the start/end date if those slots are busy 😊

```json
{
  "FacilityId": "<get this from the get request since it's reset each day>",
  "Start": "2024-06-15T11:00:00",
  "End": "2024-06-15T11:10:00",
  "Comments": "My arm hurts a lot",
  "Patient": {
    "Name": "Mario",
    "SecondName": "Neta",
    "Email": "mario.neta@example.com",
    "Phone": "555 44 33 22"
  }
}
```
## Comments

I've tried to sprinkle comments here in there in code, this section is more so to explain larger decisions.

### Get request

I've decided to make any date valid and just return the availability for that week. Michal also suggested to make the response more client-friendly, so I modified it to return the actually available slots. The mapping is done in the Controller, could have a dedicated Mapper class for it but I didn't want to overcomplicate things.

### Exceptions

I've created HTTP exceptions to make returning responses easier. In an onion-architectured application I wouldn't want domain exceptions to have the knowledge of HTTP return codes and would have introduced a class to map domain exceptions to http exceptions, but this app is so small that this was easier.

### Testing strategy

I'm a big fan of e2e tests which are as close to production as possible. I've written a few of them to test the whole flow of the application. I've also written a few unit tests to test the business logic. I didn't test the `AvailabilityService` because it was mostly covered by the e2e tests. In a real world application I would conform to the team's testing strategy and reached a 100% coverage.
