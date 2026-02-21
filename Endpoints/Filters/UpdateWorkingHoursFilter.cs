
using GNS.Contracts;
using GNS.Contracts.Requests;
using GNS.Enums;

namespace GNS.Endpoints.Filters
{
    public class UpdateWorkingHoursFilter : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(
            EndpointFilterInvocationContext context,
            EndpointFilterDelegate next)
        {
            var request = context.Arguments
                .OfType<UpdateWorkingHoursRequest>()
                .FirstOrDefault();
            if (request is null)
            {
                Results.BadRequest();
            }

            var errors = new Dictionary<string, string[]>();

            if (context.HttpContext.Items.TryGetValue("ValidationErrors", out object? _errors))
            {
                errors = _errors as Dictionary<string, string[]>;
            }

            if (!Enum.TryParse(request!.NewDayOfWeek, out CustomDayOfWeek dayOfWeek)
                && !string.IsNullOrEmpty(request.NewDayOfWeek))
            {
                errors!.Add("newdayofweek", [$"Invalid NewDayOfWeek value: {request.NewDayOfWeek}"]);
            }

            if (!TimeOnly.TryParse(request!.NewStartHour, out TimeOnly startHour)
                && !string.IsNullOrEmpty(request.NewStartHour))
            {
                errors!.Add("newstarthour", [$"Invalid NewStartHour value: {request.NewStartHour}"]);
            }
            if (!TimeOnly.TryParse(request!.NewEndHour, out TimeOnly endHour)
                && !string.IsNullOrEmpty(request.NewEndHour))
            {
                errors!.Add("newendhour", [$"Invalid NewEndHour value: {request.NewEndHour}"]);
            }

            if (!bool.TryParse(request!.NewIsOpen, out bool newIsOpen)
                && !string.IsNullOrEmpty(request.NewIsOpen))
            {
                errors!.Add("newdayofweek", [$"Invalid NewIsOpen value: {request.NewIsOpen}"]);
            }
            context.HttpContext.Items["ValidationErrors"] = errors;


            return await next(context);
        }
    }
}