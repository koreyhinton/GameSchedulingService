namespace GameSchedule;

using Microsoft.EntityFrameworkCore;

public class GameScheduleContext : DbContext
{
    public GameScheduleContext(DbContextOptions<GameScheduleContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
    public DbSet<GameSchedule> GameSchedules { get; set; }
    public DbSet<Game.Game> Games { get; set; }
    public DbSet<Game.GameTemplate> GameTemplates { get; set; }
    public DbSet<Game.Player> Players { get; set; }
    public DbSet<Game.Save> Saves { get; set; }
    public DbSet<Schedule.Schedule> Schedules { get; set; }
    public DbSet<Schedule.Client> Clients { get; set; }
    public DbSet<Schedule.ClientSet> ClientSets { get; set; }
}
