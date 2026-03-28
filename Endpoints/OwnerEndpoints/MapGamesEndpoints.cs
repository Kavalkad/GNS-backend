using GNS.Contracts.Requests;
using GNS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GNS.Endpoints.OwnerEndploints
{
    public static partial class OwnersEndpoints
    {
        public static IEndpointRouteBuilder MapGamesEndpoints(this IEndpointRouteBuilder owner)
        {
            var games = owner.MapGroup("games");
            games.MapPost("add", AddGame);
            games.MapPost("update", UpdateGame);
            games.MapDelete("delete", DeleteGame);
            games.MapPost("connect-with-gps", ConnectGameWithGPs);

            return owner;
        }
        public async static Task<IResult> ConnectGameWithGPs(
            [FromBody] AddGameGPsRequest request,
            IGameGPService service
            )
        {
            await service.Add(request);
            return Results.Ok();
        }
        public static async Task<IResult> AddGame(
            string title,
            IGameService gameService
            )
        {
            await gameService.Add(title);
            return Results.Ok();
        }

        public static async Task<IResult> UpdateGame(
            [FromBody] UpdateGameRequest request,
            IGameService service
            )
        {
            await service.Update(request);
            return Results.Ok("Game successfully updated");
        }

        public static async Task<IResult> DeleteGame(
            Guid gameId,
            IGameService service
            )
        {
            await service.Delete(gameId);
            return Results.Ok("Удалили");
        }
    }
}