using System.Text.Json.Serialization;

public class WorkDayResponse
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public DayOfWeek Day { get; set; }
    public WorkPeriod? WorkPeriod { get; set; }
    public List<TimeSlot>? BusySlots { get; set; }
    public List<TimeSlotResponse>? AvailableSlots { get; set; }

    public void CalculateAvailableSlots(int slotDurationMinutes)
    {
        if (WorkPeriod != null)
        {
            AvailableSlots = new List<TimeSlotResponse>();

            var workStart = new TimeOnly().AddHours(WorkPeriod.StartHour);
            var workEnd = new TimeOnly().AddHours(WorkPeriod.EndHour);
            var lunchStart = new TimeOnly().AddHours(WorkPeriod.LunchStartHour);
            var lunchEnd = new TimeOnly().AddHours(WorkPeriod.LunchEndHour);

            for (var slotStart = workStart; slotStart < lunchStart; slotStart = slotStart.AddMinutes(slotDurationMinutes))
            {
                AvailableSlots.Add(new TimeSlotResponse { Start = slotStart, End = slotStart.AddMinutes(slotDurationMinutes) });
            }

            for (var slotStart = lunchEnd; slotStart < workEnd; slotStart = slotStart.AddMinutes(slotDurationMinutes))
            {
                AvailableSlots.Add(new TimeSlotResponse { Start = slotStart, End = slotStart.AddMinutes(slotDurationMinutes) });
            }

            if (BusySlots != null)
            {
                AvailableSlots.RemoveAll(slot => BusySlots.Any(busySlot => slot.Start.ToTimeSpan() < busySlot.End.TimeOfDay && slot.End.ToTimeSpan() > busySlot.Start.TimeOfDay));
            }
        }
    }
}
