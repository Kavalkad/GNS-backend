using GNS.Contracts.Requests;
using GNS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GNS.Endpoints.OwnerEndploints
{
    public static partial class OwnersEndpoints
    {
        public static IEndpointRouteBuilder MapWithGamingPlaceEndpoints(this IEndpointRouteBuilder owner)
        {
            var gamingPlaces = owner.MapGroup("gaming-places");
            gamingPlaces.MapPost("add", AddGamingPlaces);
            gamingPlaces.MapGet("get-by-ccid", GetCCGamingPlaces);
            gamingPlaces.MapPut("update", UpdateCCGamingPlaces);
            gamingPlaces.MapDelete("delete", DeleteCCGamingPlaces);

            return owner;
        }
        public static async Task<IResult> AddGamingPlaces(
            [FromBody] AddGamingPlacesRequest request,
            IGamingPlaceService service
            )
        {
            await service.AddGamingPlaces(request);
            return Results.Ok("GamingPlaces added successfully");
        }
        public static async Task<IResult> GetCCGamingPlaces(
            Guid cyberClubId,
            IGamingPlaceService service
            )
        {
            var gamingPlaces = await service.GetCCGamingPlaces(cyberClubId);

            return TypedResults.Ok(gamingPlaces);
        }
        public static async Task<IResult> UpdateCCGamingPlaces(
            [FromBody] UpdateCCGamingPlacesRequest request,
            IGamingPlaceService service
            )
        {
            //await service.UpdateCCGamingPlaces(request);

            return Results.Ok();
        }
        public static async Task<IResult> DeleteCCGamingPlaces(
            [FromBody] DeleteGamingPlacesRequest request,
            IGamingPlaceService service
            )
        {
            await service.DeleteGamingPlaces(request);
            return Results.Ok();
        }
    }
}