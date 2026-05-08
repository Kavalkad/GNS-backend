using GNS.Contracts.Requests;
using GNS.Contracts.Requests.Interfaces;
using GNS.Endpoints.Filters;
using GNS.Exceptions;
using GNS.Extensions;
using GNS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GNS.Endpoints.OwnerEndploints
{
    public static partial class OwnersEndpoints
    {
        public static IEndpointRouteBuilder MapWithCyberClubEndpoints(this IEndpointRouteBuilder owner)
        {
            var cyberClubs = owner.MapGroup("cyberclub");
                

            cyberClubs.MapPost("add", AddCyberClub);
            cyberClubs.MapGet("get-my-clubs", GetOwnersCyberClubs);

            var updates = cyberClubs.MapGroup("update")
                .AddEndpointFilter<OwnerAccessToCyberClubFilter>();

            updates.MapPut("name", UpdateCyberClubName)
                .AddEndpointFilter<NameFilter>()
                .AddEndpointFilter<TerminalValidationFilter>();

            updates.MapPut("city", UpdateCyberClubCity)
                .AddEndpointFilter<CityFilter>()
                .AddEndpointFilter<TerminalValidationFilter>();

            updates.MapPut("address", UpdateCyberClubAddress)
                .AddEndpointFilter<AddressFilter>()
                .AddEndpointFilter<TerminalValidationFilter>();


            cyberClubs.MapDelete("delete-by-id", DeleteClubById)
                .AddEndpointFilter<OwnerAccessToCyberClubFilter>();
            

            return owner;
        }
        public static async Task<IResult> AddCyberClub(
            [FromBody] CreateCyberClubRequest request,
            ICyberClubService cyberClubService
            )
        {
            await cyberClubService.AddAsync(request);
            return Results.Ok("CyberClub was successfully created");
        }

        public static async Task<IResult> GetOwnersCyberClubs(
            ICyberClubService service,
            HttpContext context
            )
        {
            var idClaim = context.User.Claims.FirstOrDefault(c => c.Type == "Id")
                ?? throw new Exception("Ты шо дядя не взял с собой idClaim? А голову ты дома не забыл?");

            if (!Guid.TryParse(idClaim.Value, out Guid ownerId))
            {
                throw new IncorrectGuidException(idClaim.Value);
            }
            var cyberClubs = await service.GetOwnerCyberClubsAsync(ownerId);

            return TypedResults.Ok(cyberClubs);
        }

        public static async Task<IResult> UpdateCyberClubName(
            [FromBody] UpdateCyberClubNameRequest request,
            ICyberClubService cyberClubService
            )
        {
            await cyberClubService.UpdateCyberClubNameAsync(request);
            return Results.Ok();
        }
        public static async Task<IResult> UpdateCyberClubCity(
            [FromBody] UpdateCyberClubCityRequest request,
            ICyberClubService cyberClubService
            )
        {
            await cyberClubService.UpdateCyberClubCityAsync(request);
            return Results.Ok();
        }
        public static async Task<IResult> UpdateCyberClubAddress(
            [FromBody] UpdateCyberClubAddressRequest request,
            ICyberClubService cyberClubService
            )
        {
            await cyberClubService.UpdateCyberClubAddressAsync(request);
            return Results.Ok();
        }


        public static async Task<IResult> DeleteClubById(
            Guid cyberClubId,
            ICyberClubService cyberClubservice
            )
        {
            await cyberClubservice.DeleteClubByIdAsync(cyberClubId);

            return Results.Ok($"CyberClub with id: {cyberClubId} was successfully deleted");
        }



        /*  public static async Task<IResult> DeleteClubByName(
                    string name,
                    ICyberClubService cyberClubService
                    )
                {
                    //await cyberClubService.DeleteByName(name);

                    return Results.Ok($"CyberClub with name: {name} had deleted");
                }
                */
    }
}