// ran into some problems with using DateOnly, so decided to create a separate class, it better encapsulates the Monday only rule too
public class GetAvailabilityRequest
{
    public DateTime Date { get; private set; }

    public GetAvailabilityRequest(int year, int month, int day)
    {
        if (year < 1 || year > 9999)
        {
            throw new BadRequestException("Year must be between 1 and 9999.");
        }

        if (month < 1 || month > 12)
        {
            throw new BadRequestException("Month must be between 1 and 12.");
        }

        if (day < 1 || day > 31)
        {
            throw new BadRequestException("Day must be between 1 and 31.");
        }

        DateTime date;
        try
        {
            date = new DateTime(year, month, day);
        }
        catch (ArgumentOutOfRangeException)
        {
            throw new BadRequestException("The provided date is invalid.");
        }

        if (date.DayOfWeek != DayOfWeek.Monday)
        {
            throw new BadRequestException("The date must be a Monday.");
        }

        Date = date;
    }

    public override string ToString()
    {
        return Date.ToString("yyyyMMdd");
    }
}
