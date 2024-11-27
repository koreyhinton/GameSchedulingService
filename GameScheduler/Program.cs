using GameSchedule;
using GameSchedule.Game;
using GameSchedule.Schedule;
using GameScheduler;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
new DbServiceConnector().ConnectSqlite(builder.Services);
builder.WebHost.ConfigureKestrel(o => { o.AllowSynchronousIO = true; });
var app = builder.Build();

app.MapGet("/", () => "Hello World!");



//app.MapPost("/schedule", (HttpRequest request, [FromServices] GameScheduleContext context) =>
app.MapPost("/schedule", async Task<Results<Ok<GameSchedule.GameSchedule>, NotFound>> (GameScheduleRequest request, [FromServices] GameScheduleContext context) =>
{
    /*JsonElement gameScheduleRequestBody = JsonSerializer.Deserialize<dynamic>(request.Body);

    gameScheduleRequestBody.TryGetProperty("GameTemplateId", out JsonElement value);

    long gameTemplateId = value.GetInt64();

    gameScheduleRequestBody.TryGetProperty("ClientName", out JsonElement value2);
    string clientName = value2.GetString();
    */

    var gameSchedule = await context.GameSchedules
        .Include(gs => gs.Schedule)
            .ThenInclude(s => s!.SchedulingClient)
                .ThenInclude(s => s!.ClientSet)
                    .ThenInclude(cs => cs!.Clients)
        .FirstOrDefaultAsync(gameSched => gameSched!.Game!.Name == request.GameName && !gameSched.Game.Over);

    if (gameSchedule == null)
    {
        const int maxNumPlayers = 25;

        var gameTemplate = await context.GameTemplates
            .AsNoTracking()
            .FirstOrDefaultAsync(gt => gt.GameName == request.GameName);

        if (gameTemplate == null)
            return TypedResults.NotFound();

        var players = new List<Player>();
        for (var i = 0; i < (gameTemplate?.PlayerMax == null ? 0 : Math.Min(maxNumPlayers, gameTemplate.PlayerMax.Value)); i++)
        {
            players.Add(new Player
            {
                Num = (short)(i+1),
                SaveData = new Save()
            });
        }

        gameSchedule = new GameSchedule.GameSchedule
        {
            CreatedDate = DateTime.Now,
            Game = new Game
            {
                Name = request.GameName,
                Over = false,
                Players = players
            },
            Schedule = new GameSchedule.Schedule.Schedule
            {
                SchedulingClient = new GameSchedule.Schedule.Client
                {
                    DisplayName = request.ClientName,
                    TeamName = request.TeamName
                }
            },
        };
        context.Players.AddRange(players);
        context.Games.Add(gameSchedule.Game);
        context.Schedules.Add(gameSchedule.Schedule);
        context.Clients.Add(gameSchedule.Schedule.SchedulingClient);
        context.ClientSets.Add(new GameSchedule.Schedule.ClientSet
        {
            Clients = new List<Client> { gameSchedule.Schedule.SchedulingClient }
        });
        context.GameSchedules.Add(gameSchedule);

        await context.SaveChangesAsync();
        return TypedResults.Ok(await context.GameSchedules.AsNoTracking().FirstOrDefaultAsync(gs => gs.Id != default && gs.Id == gameSchedule.Id));
    }
    else
    {
        if (!(gameSchedule.Schedule!.SchedulingClient!.ClientSet!.Clients.Any(c => c.DisplayName == request.ClientName)))
        {
            gameSchedule.Schedule!.SchedulingClient!.ClientSet!.Clients.Add(
                new Client
                {
                    DisplayName = request.ClientName,
                    TeamName = request.TeamName
                }
            );
            await context.SaveChangesAsync();
            return TypedResults.Ok(await context.GameSchedules.AsNoTracking().FirstOrDefaultAsync(gs => gs.Id != default && gs.Id == gameSchedule.Id));
        }
    }

    return TypedResults.Ok(await context.GameSchedules.AsNoTracking().FirstOrDefaultAsync(gs => gs.Id != default && gs.Id == gameSchedule.Id));
});

app.Run();
