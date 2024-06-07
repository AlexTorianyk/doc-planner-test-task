// ran into some problems with using DateOnly, so decided to create a separate class. Easier for me to implement the automatic switch to Monday as well
public class GetAvailabilityRequest
{
    public DateTime LatestMonday { get; private set; }

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

        // Calculate the number of days between the given date and the previous Monday
        int daysSinceMonday = (int)date.DayOfWeek - (int)DayOfWeek.Monday;

        // If the given date is before Monday, adjust daysSinceMonday to reflect the previous week
        if (daysSinceMonday < 0)
        {
            daysSinceMonday += 7;
        }

        // Subtract the number of days from the given date to get the Monday of the week
        LatestMonday = date.AddDays(-daysSinceMonday);
    }

    public override string ToString()
    {
        return LatestMonday.ToString("yyyyMMdd");
    }
}
