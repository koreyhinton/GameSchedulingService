using GameSchedule;
using GameSchedule.Game;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;

GameScheduleContext gameScheduleContext;
/*
 * gameScheduleService <-
 */ {
    var services = new ServiceCollection();
    new DbServiceConnector().ConnectSqlite(services);
    var serviceProvider = services.BuildServiceProvider();
    gameScheduleContext = serviceProvider.GetRequiredService<GameScheduleContext>();
}

int? minNumPlayers = null;
int? maxNumPlayers = null;
string gameName = string.Empty;

foreach (var arg in args)
{
    if (new Regex("^[0-9]+-[0-9]+$").Match(arg).Success)
    {
        minNumPlayers = int.Parse(arg.Split('-')[0]);
        maxNumPlayers = int.Parse(arg.Split('-')[1]);
    }
    else
    {
        gameName = arg;
    }
}

if (string.IsNullOrWhiteSpace(gameName) && minNumPlayers == null)
{
    Console.WriteLine("Minimum # of players? (Optional)");
    if (int.TryParse(Console.ReadLine(), out int min))
        minNumPlayers = min;
}

if (string.IsNullOrWhiteSpace(gameName) && maxNumPlayers == null)
{
    Console.WriteLine("Maximum # of players? (Optional)");
    if (int.TryParse(Console.ReadLine(), out int max))
        maxNumPlayers = max;
}

if (string.IsNullOrWhiteSpace(gameName))
{
    Console.WriteLine("Game Name? (Required)");
    gameName = Console.ReadLine() ?? "<null>";
}

Func<DbSet<GameTemplate>, long> getGameTemplateId = (gameTemplates) =>
{
    return gameTemplates
        .Where(o => !string.IsNullOrWhiteSpace(gameName) &&
            o.GameName == gameName && o.PlayerMin == minNumPlayers && o.PlayerMax == maxNumPlayers)
        .Select(o => o.Id)
        .FirstOrDefault();
};

var gameTemplateId = getGameTemplateId(gameScheduleContext.GameTemplates);
string action = "";

if (gameTemplateId == default)
{
    action += "did not exist";
    gameScheduleContext.GameTemplates.Add(new GameSchedule.Game.GameTemplate
    {
        GameName = gameName,
        PlayerMin = minNumPlayers,
        PlayerMax = maxNumPlayers,
        RegistrationDate = DateTime.Now
    });

    gameScheduleContext.SaveChanges();
    gameTemplateId = getGameTemplateId(gameScheduleContext.GameTemplates);
    action += (gameTemplateId == default) ? ", and could not be created" : ", and was created";
} else { action += "already exists"; }

var idString = gameTemplateId == default ? "-" : gameTemplateId.ToString();

Console.WriteLine($"Game Template Id {idString} {action}");
