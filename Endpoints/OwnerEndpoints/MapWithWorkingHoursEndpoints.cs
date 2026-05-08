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
                
            workingHours.MapPost("add", AddWorkingHours)
                .AddEndpointFilter<OwnerAccessToCyberClubFilter>();

            var update = workingHours.MapGroup("update")
                .AddEndpointFilter<OwnerAccessToWorkingHoursFilter>();;
                
            update.MapPut("start-hour", UpdateStartHour);
            update.MapPut("end-hour", UpdateEndHour);
            update.MapPut("is-open", UpdateIsOpen);


            workingHours.MapDelete("delete-by-whid", DeleteWHById)
                .AddEndpointFilter<OwnerAccessToWorkingHoursFilter>();

            return owner;
        }
        public static async Task<IResult> AddWorkingHours(
           CreateWorkingHoursRequest request,
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

        public static async Task<IResult> UpdateStartHour(
            UpdateWorkingHoursStartHourRequest request,
            IWorkingHoursService service
            )
        {
            await service.UpdateWorkingHoursStartHourAsync(request);

            return Results.Ok();
        }
        public static async Task<IResult> UpdateEndHour(
            UpdateWorkingHoursEndHourRequest request,
            IWorkingHoursService service
            )
        {
            await service.UpdateWorkingHoursEndHourAsync(request);

            return Results.Ok();
        }
        public static async Task<IResult> UpdateIsOpen(
            UpdateWorkingHoursIsOpenRequest request,
            IWorkingHoursService service
            )
        {
            await service.UpdateWorkingHoursIsOpenAsync(request);

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