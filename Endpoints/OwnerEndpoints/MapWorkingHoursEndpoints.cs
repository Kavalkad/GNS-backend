using GNS.Contracts.Requests;
using GNS.Endpoints.Filters;
using GNS.Services.Interfaces;

namespace GNS.Endpoints.OwnerEndploints
{
    public static partial class OwnersEndpoints
    {
        public static IEndpointRouteBuilder MapWithWorkingHoursEndpoints(this IEndpointRouteBuilder owner)
        {
            var workingHours = owner.MapGroup("working-hours");
            workingHours.MapPost("add", AddWorkingHours);
            workingHours.MapGet("get-clubs-wh", GetCCWorkingHours);
            workingHours.MapPut("update", UpdateWorkingHours)
                .AddEndpointFilter<UpdateWorkingHoursFilter>()
                .AddEndpointFilter<FinalValidationFilter>();
           // workingHours.MapDelete("delete-by-ccid", DeleteWHByCCId);
            workingHours.MapDelete("delete-by-whid", DeleteWHById);

            return owner;
        }
         public static async Task<IResult> AddWorkingHours(
            AddWorkingHoursRequest request,
            IWorkingHoursService service
            )
        {
            await service.AddWorkingHoursAsync(request);

            return Results.Ok($"WorkingHours for day {request.DayOfWeek} successfully addd");
        }
        public static async Task<IResult> GetCCWorkingHours(
            Guid cyberClubId,
            IWorkingHoursService service
            )
        {
            var workingHours = await service.GetByCyberClubIdAsync(cyberClubId);

            return TypedResults.Ok(workingHours);
        }

        public static async Task<IResult> UpdateWorkingHours(
            UpdateWorkingHoursRequest request,
            IWorkingHoursService service
            )
        {
            await service.UpdateWorkingHoursAsync(request);

            return Results.Ok();
        }

        // Delete WorkingHours
        public static async Task<IResult> DeleteWHById(
            Guid whId,
            IWorkingHoursService service
            )
        {
            await service.DeleteByWorkingHoursIdAsync(whId);
            return Results.Ok();
        }
        /*
        public static async Task<IResult> DeleteWHByCCId(
            Guid ccId,
            IWorkingHoursService service
            )
        {
            await service.DeleteByCId(ccId);
            return Results.Ok();
        }
        */
    } 
}