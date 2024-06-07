public class AvailabilityResponse{
    public Facility Facility { get; set; }
    public List<WorkDayResponse> Schedule { get; set; }

    public AvailabilityResponse(Availability availability)
    {
        Facility = availability.Facility;
        Schedule = new List<WorkDayResponse>();

        // I don't particularly like this, but I wanted to add information about the day of the week
        var days = new List<(DayOfWeek, WorkDay?)>
        {
            (DayOfWeek.Monday, availability.Monday),
            (DayOfWeek.Tuesday, availability.Tuesday),
            (DayOfWeek.Wednesday, availability.Wednesday),
            (DayOfWeek.Thursday, availability.Thursday),
            (DayOfWeek.Friday, availability.Friday),
            (DayOfWeek.Saturday, availability.Saturday),
            (DayOfWeek.Sunday, availability.Sunday)
        };

        foreach (var (dayOfWeek, day) in days)
        {
            if (day != null)
            {
                var workDay = new WorkDayResponse
                {
                    Day = dayOfWeek,
                    WorkPeriod = day.WorkPeriod,
                    BusySlots = day.BusySlots
                };

                workDay.CalculateAvailableSlots(availability.SlotDurationMinutes);

                Schedule.Add(workDay);
            }
        }
    }
}
