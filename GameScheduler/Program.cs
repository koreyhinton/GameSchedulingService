using GameSchedule;
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
app.MapPost("/schedule", async Task<Results<Ok<GameSchedule.GameSchedule>, NotFound>> (HttpRequest request, [FromServices] GameScheduleContext context) =>
{
    JsonElement gameScheduleRequestBody = JsonSerializer.Deserialize<dynamic>(request.Body);

    gameScheduleRequestBody.TryGetProperty("GameTemplateId", out JsonElement value);

    long gameTemplateId = value.GetInt64();

    gameScheduleRequestBody.TryGetProperty("ClientName", out JsonElement value2);
    string clientName = value2.GetString();

    var template = await context.GameTemplates
        .AsNoTracking()
        .FirstOrDefaultAsync(gt => gt.Id == gameTemplateId);

    if (template == null)
        return TypedResults.NotFound();

    // todo: build the game and save

    return TypedResults.Ok(new GameSchedule.GameSchedule() { Game = new GameSchedule.Game.Game { Name = "Test Game" }  });//$"test {gameTemplateId} {clientName}");
});

app.Run();
