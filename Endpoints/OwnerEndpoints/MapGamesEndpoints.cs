using GNS.Contracts.Requests;
using GNS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GNS.Endpoints.OwnerEndploints
{
    public static partial class OwnersEndpoints
    {
        public static IEndpointRouteBuilder MapWithGamesEndpoints(this IEndpointRouteBuilder owner)
        {
            var games = owner.MapGroup("games");
            games.MapPost("add", AddGame);

            var update = games.MapGroup("update");
            update.MapPut("title", UpdateGameTitle);
            update.MapPut("on-pc", UpdateGameOnPC);
            update.MapPut("on-playstation", UpdateGameOnPlayStation);
            update.MapPut("on-xbox", UpdateGameOnXbox);

            games.MapDelete("delete", DeleteGame);


            return owner;
        }

        public static async Task<IResult> AddGame(
            AddGameRequest request,
            IGameService gameService
            )
        {
            await gameService.AddAsync(request);
            return Results.Ok();
        }
        public static async Task<IResult> UpdateGameTitle(
            [FromBody] UpdateGameTitleRequest request,
            IGameService service
            )
        {
            await service.UpdateTitleAsync(request);
            return Results.Ok("Game successfully updated");
        }
        public static async Task<IResult> UpdateGameOnPC(
            [FromBody] UpdateGameOnRequest request,
            IGameService service
            )
        {
            await service.UpdateOnPCAsync(request);
            return Results.Ok("Game successfully updated");
        }
        public static async Task<IResult> UpdateGameOnPlayStation(
            [FromBody] UpdateGameOnRequest request,
            IGameService service
            )
        {
            await service.UpdateOnPlayStationAsync(request);
            return Results.Ok("Game successfully updated");
        }
        public static async Task<IResult> UpdateGameOnXbox(
            [FromBody] UpdateGameOnRequest request,
            IGameService service
            )
        {
            await service.UpdateOnXboxAsync(request);
            return Results.Ok("Game successfully updated");
        }

        public static async Task<IResult> DeleteGame(
            Guid gameId,
            IGameService service
            )
        {
            await service.DeleteGameByIdAsync(gameId);
            return Results.Ok("Удалили");
        }
    }
}