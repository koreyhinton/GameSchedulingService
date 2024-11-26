
using System.ComponentModel.DataAnnotations.Schema;

namespace GameSchedule.Schedule;

[Table(nameof(Schedule))]
public class Schedule
{
    public long Id { get; set; }

    public long GameScheduleId { get; set; }
    [ForeignKey(nameof(GameScheduleId))]
    [InverseProperty(nameof(GameSchedule.Schedule))]
    public GameSchedule? GameSchedule { get; set; }

    public long SchedulingClientId { get; set; } // the host
    [ForeignKey(nameof(SchedulingClientId))]
    [InverseProperty(nameof(Client.HostedSchedule))]
    public Client? SchedulingClient { get; set; }
}
