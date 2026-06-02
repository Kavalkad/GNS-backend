using GNS.Contracts.Requests;
using GNS.Contracts.Requests.Implementations;
using GNS.Endpoints.Filters;
using GNS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GNS.Endpoints.OwnerEndploints
{
    public static partial class OwnersEndpoints
    {
        public static IEndpointRouteBuilder MapWithGamingPlaceEndpoints(this IEndpointRouteBuilder owner)
        {
            var gamingPlaces = owner.MapGroup("gaming-places");
            gamingPlaces.MapPost("add", AddGamingPlaces)
                .AddEndpointFilter<OwnerAccessToCyberClubFilter>()
                .AddEndpointFilter<PricePerHourFilter>()
                .AddEndpointFilter<TerminalValidationFilter>();

            var update = gamingPlaces.MapGroup("update")
                .AddEndpointFilter<OwnerAccessToGamingPlaceFilter>();

            update.MapPut("number", UpdateGamingPlaceNumber)
                .AddEndpointFilter<NumberFilter>()
                .AddEndpointFilter<TerminalValidationFilter>();
            update.MapPut("price-per-hour", UpdateGamingPlacePricePerHour)
                .AddEndpointFilter<PricePerHourFilter>()
                .AddEndpointFilter<TerminalValidationFilter>();

            gamingPlaces.MapDelete("delete", DeleteGamingPlace)
                .AddEndpointFilter<OwnerAccessToGamingPlaceFilter>();

            return owner;
        }
        public static async Task<IResult> AddGamingPlaces(
            [FromBody] CreateGamingPlacesRequest request,
            IGamingPlaceService service
            )
        {
            await service.AddGamingPlacesAsync(request);
            return Results.Ok("GamingPlaces added successfully");
        }

        public static async Task<IResult> UpdateGamingPlaceNumber(
            IGamingPlaceService service,
            [FromBody] UpdateGamingPlaceNumberRequest request
        )
        {
            await service.UpdateGamingPlaceNumberAsync(request);

            return Results.Ok();
        }
        public static async Task<IResult> UpdateGamingPlacePricePerHour(
            IGamingPlaceService service,
            [FromBody] UpdateGamingPlacePricePerHourRequest request
        )
        {
            await service.UpdateGamingPlacePricePerHourAsync(request);

            return Results.Ok();
        }
        
        public static async Task<IResult> DeleteGamingPlace(
            Guid gamingPlaceId,
            IGamingPlaceService service
            )
        {
            await service.DeleteGamingPlaceAsync(gamingPlaceId);
            return Results.Ok();
        }
    }
}