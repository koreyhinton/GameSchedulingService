using GameSchedule.Schedule;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameSchedule;

public class GameSchedule
{
    public long Id { get; set; }
    public DateTime CreatedDate {  get; set; }

    public long GameId {  get; set; }
    [InverseProperty(nameof(Game.GameSchedule))]
    public Game.Game? Game {  get; set; } 

    public long ScheduleId { get; set; }
    [InverseProperty(nameof(Schedule.GameSchedule))]
    public Schedule.Schedule? Schedule { get; set; }
}
