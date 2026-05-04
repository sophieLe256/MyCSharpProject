using WebApplication1.Data;
using WebApplication1.Dtos;
using WebApplication1.Entities;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Endpoints;

public static class GamesEndPoints
{
    const string GetGameEndpointName = "GetGame";

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("games").WithParameterValidation();

        // GET /games
        group.MapGet("/", async (GameStoreContext db) =>
            await db.Games
                    .Include(g => g.Genre)
                    .Select(g => new GameDto(g.Id, g.Name, g.Genre!.Name, g.Price, g.ReleaseDate))
                    .AsNoTracking()
                    .ToListAsync());

        // GET /games/{id}
        group.MapGet("/{id}", async (int id, GameStoreContext db) =>
        {
            Game? game = await db.Games.Include(g => g.Genre).AsNoTracking().FirstOrDefaultAsync(g => g.Id == id);
            return game is null
                ? Results.NotFound()
                : Results.Ok(new GameDto(game.Id, game.Name, game.Genre!.Name, game.Price, game.ReleaseDate));
        }).WithName(GetGameEndpointName);

        // POST /games
        group.MapPost("/", async (CreateGameDto newGame, GameStoreContext db) =>
        {
            Genre? genre = await db.Genres.FirstOrDefaultAsync(g => g.Name == newGame.Genre);
            if (genre is null)
            {
                return Results.BadRequest($"Genre '{newGame.Genre}' not found.");
            }

            Game game = new()
            {
                Name = newGame.Name,
                GenreId = genre.Id,
                Price = newGame.Price,
                ReleaseDate = newGame.ReleaseDate
            };

            db.Games.Add(game);
            await db.SaveChangesAsync();

            GameDto dto = new(game.Id, game.Name, genre.Name, game.Price, game.ReleaseDate);
            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, dto);
        });

        // PUT /games/{id}
        group.MapPut("/{id}", async (int id, UpdateGameDto updatedGame, GameStoreContext db) =>
        {
            Game? game = await db.Games.FindAsync(id);
            if (game is null)
            {
                return Results.NotFound();
            }

            Genre? genre = await db.Genres.FirstOrDefaultAsync(g => g.Name == updatedGame.Genre);
            if (genre is null)
            {
                return Results.BadRequest($"Genre '{updatedGame.Genre}' not found.");
            }

            game.Name = updatedGame.Name;
            game.GenreId = genre.Id;
            game.Price = updatedGame.Price;
            game.ReleaseDate = updatedGame.ReleaseDate;

            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        // DELETE /games/{id}
        group.MapDelete("/{id}", async (int id, GameStoreContext db) =>
        {
            Game? game = await db.Games.FindAsync(id);
            if (game is null)
            {
                return Results.NotFound();
            }
            db.Games.Remove(game);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        return group;
    }
}