using System.ComponentModel.DataAnnotations.Schema;

namespace GameSchedule.Schedule;

[Table(nameof(Client))]
public class Client
{
    public long Id { get; set; }

    public string DisplayName { get; set; } = string.Empty;

    public long ClientSetId { get; set; }
    [ForeignKey(nameof(ClientSetId))]
    [InverseProperty(nameof(ClientSet.Clients))]
    public ClientSet? ClientSet { get; set; }

    public long HostedScheduleId { get; set; }
    [InverseProperty(nameof(Schedule.SchedulingClient))]
    public Schedule? HostedSchedule { get; set; }
}
