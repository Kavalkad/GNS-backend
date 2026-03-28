using GNS.Contracts.Requests;
using GNS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GNS.Endpoints.OwnerEndploints
{
    public static partial class OwnersEndpoints
    {
        public static IEndpointRouteBuilder MapCyberClubEndpoints(this IEndpointRouteBuilder owner)
        {
            var cyberClubs = owner.MapGroup("cyberclub");
            cyberClubs.MapPost("add", AddCyberClub);
            cyberClubs.MapGet("get-my-clubs", GetOwnersCyberClubs);
            cyberClubs.MapPut("update", UpdateClub);
            cyberClubs.MapDelete("delete-by-id", DeleteClubById);
            cyberClubs.MapDelete("delete-by-name", DeleteClubByName);

            return owner;
        }
        public static async Task<IResult> AddCyberClub(
            [FromBody] AddCyberClubRequest request,
            ICyberClubService cyberClubService
            )
        {
            await cyberClubService.AddCyberClub(request);
            return Results.Ok("CyberClub was successfully created");
        }

        public static async Task<IResult> GetOwnersCyberClubs(ICyberClubService service)
        {
            var cyberClubs = await service.GetMyCyberClubs();

            return TypedResults.Ok(cyberClubs);
        }

        public static async Task<IResult> UpdateClub(
                [FromBody] UpdateCyberClubRequest request,
                ICyberClubService cyberClubService
            )
        {
            // cyberClubService.Update(request);
            return Results.Ok();
        }


        public static async Task<IResult> DeleteClubById(
            Guid cyberClubId,
            ICyberClubService cyberClubsRepository
            )
        {
            //await cyberClubsRepository.DeleteById(cyberClubId);

            return Results.Ok($"CyberClub with id: {cyberClubId} was successfully deleted");
        }
        public static async Task<IResult> DeleteClubByName(
            string name,
            ICyberClubService cyberClubService
            )
        {
            //await cyberClubService.DeleteByName(name);

            return Results.Ok($"CyberClub with name: {name} had deleted");
        }
    }
}