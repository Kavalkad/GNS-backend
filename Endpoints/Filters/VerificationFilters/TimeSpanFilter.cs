using GNS.Contracts.Requests.Interfaces;


namespace GNS.Endpoints.Filters
{
    public class TimeSpanFilter : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(
            EndpointFilterInvocationContext context,
            EndpointFilterDelegate next
            )
        {
            var errors = new Dictionary<string, string[]>();

            if (context.HttpContext.Items.TryGetValue("ValidationErrors", out object? _errors))
            {
                errors = _errors as Dictionary<string, string[]>;
            }

            var request = context.Arguments
                .OfType<ITimeSpanRequest>()
                .FirstOrDefault();

            if (request is null)
            {
                Results.BadRequest("failed to get timespan values");
            }

            var start = request.DateTimeStart;
            var end = request.DateTimeEnd;
            var now = DateTime.Now;

            if (end < now || start < now)
            {
                errors.Add("past time", ["You can't choose past time"]);
            }

            if (end - start != TimeSpan.FromHours(1))
            {
                errors.Add("invalid timespan", ["You can order only 1 hour"]);
            }
            
            if (end.Minute != 0)
            {
                errors.Add("datetimeend", ["datetimeend must match with the hour begining"]);
            }
            
            if (start.Minute != 0)
            {
                errors.Add("datetimestart", ["datetimestart must match with the hour begining"]);
            }

            context.HttpContext.Items["ValidationErrors"] = errors;

            return await next(context);
        }
    }
}