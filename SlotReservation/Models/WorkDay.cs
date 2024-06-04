public class WorkDay
{
    public WorkPeriod? WorkPeriod { get; set; }
    public List<BusySlot>? BusySlots { get; set; }
}

public class BusySlot
{
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
}
