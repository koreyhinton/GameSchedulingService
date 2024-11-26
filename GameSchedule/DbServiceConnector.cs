
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace GameSchedule;

public class DbServiceConnector
{
    public void ConnectSqlite(IServiceCollection services)
    {
        Directory.CreateDirectory(Path.Combine(Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%"), ".gss"));
        services
            .AddEntityFrameworkSqlite()
            .AddDbContext<GameScheduleContext>(optionsBuilder =>
            {
                //optionsBuilder.UseSqlite("Data Source=.\\game-schedules.db");
                optionsBuilder.UseSqlite(
                    "Data Source=" + Path.Combine(Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%"), ".gss", "game-schedules.db")
                );
                
            });
    }
}
