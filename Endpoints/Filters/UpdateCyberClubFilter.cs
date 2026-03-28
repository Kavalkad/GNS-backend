/*
using GNS.Contracts.Requests;
using GNS.Extensions;

namespace GNS.Endpoints.Filters
{
    public class VerifyCyberClubParametersFilter : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(
            EndpointFilterInvocationContext context,
            EndpointFilterDelegate next)
        {
            var errors = new Dictionary<string, string[]>();

            if (context.HttpContext.Items.TryGetValue("ValidationErrors", out object? _errors))
            {
                errors = _errors as Dictionary<string, string[]>;
            }

            var request = context.Arguments
                .OfType<UpdateCyberClubRequest>()
                .FirstOrDefault()
                    ?? throw new Exception("Can't read request data");

            if (!Guid.TryParse(request.CyberClubId, out Guid cyberClubId))
            {
                errors?.Add("cyberclubId", ["Invalid id format"]);
            }

            if (!string.IsNullOrEmpty(request.NewName) && !request.NewName.IsName())
            {
                errors?.Add("newname", ["newname has incorrect format. newname must contain only letters."]);
            }

            if (!string.IsNullOrEmpty(request.NewCity) && !request.NewCity.All(char.IsLetter))
            {
                errors?.Add("newcity", ["newcity has incorrect format. city must contain only letters."]);
            }

            if (!string.IsNullOrEmpty(request.NewAddress) && !request.NewAddress.IsAddress())
            {
                errors?.Add("newaddress", ["newaddress has incorrect format. newname must contain only letters, digits, white space or \".\" ."]);
            }

            context.HttpContext.Items["ValidationErrors"] = errors;

            return await next(context);
        }
    }
}
*/