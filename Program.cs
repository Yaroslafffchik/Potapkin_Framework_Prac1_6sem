using PotapkinPrac1.Middleware;
using PotapkinPrac1.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<TrackService>();

var app = builder.Build();

app.UseMiddleware<TimingMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseDefaultFiles();
app.UseStaticFiles();

var trackService = app.Services.GetRequiredService<TrackService>();

app.MapGet("/api/tracks", (string? artist) =>
{
    var tracks = trackService.GetAll(artist);
    return Results.Ok(tracks);
});

app.MapGet("/api/tracks/{id:int}", (int id) =>
{
    var track = trackService.GetById(id);
    return Results.Ok(track);
});

app.MapPost("/api/tracks", (PotapkinPrac1.Domain.Track track) =>
{
    var createdTrack = trackService.Create(track);
    return Results.Created($"/api/tracks/{createdTrack.Id}", createdTrack);
});

app.Run();
