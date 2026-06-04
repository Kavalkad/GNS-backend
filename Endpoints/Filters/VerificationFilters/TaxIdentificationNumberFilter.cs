using GNS.Contracts.Requests.Interfaces;


namespace GNS.Endpoints.Filters
{
    public class TaxIdentificationNumberFilter : IEndpointFilter
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
                .OfType<ITaxIdentificationNumberRequest>()
                .FirstOrDefault();

            if (request is null)
            {
                Results.BadRequest("failed to read TaxIdentifiactionNumber from request");
            }

            var taxIdentifiactionNumber = request!.TaxIdentificationNumber;

            if (taxIdentifiactionNumber.Length != 9)
            {
                errors!.Add("TaxIdentificationNumber Lenth", ["TaxIdentifiactionNumber must contain only 9 digits"]);
            }

            if (taxIdentifiactionNumber.Any(c => !char.IsDigit(c)))
            {
                errors!.Add("TaxIdentificationNumber", ["TaxIdentificationNumber must contain only digits"]);
            }
            
            context.HttpContext.Items["ValidationErrors"] = errors;
            
            return await next(context);
        }
    }
}