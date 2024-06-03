public class Availability
{
    public required Facility Facility { get; set; }
    public int SlotDurationMinutes { get; set; }
    // Would be better to have a list of WorkDay objects in the response
    public WorkDay? Monday { get; set; }
    public WorkDay? Tuesday { get; set; }
    public WorkDay? Wednesday { get; set; }
    public WorkDay? Thursday { get; set; }
    public WorkDay? Friday { get; set; }
    public WorkDay? Saturday { get; set; }
    public WorkDay? Sunday { get; set; }
}
