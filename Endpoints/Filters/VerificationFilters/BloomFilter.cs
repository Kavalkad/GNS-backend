using GNS.Contracts.Requests;
using GNS.Data.Repositories.Interfaces;
using GNS.Services.Interfaces;

namespace GNS.Endpoints.Filters
{
    public class BloomFilter(IBloomBytesService bloomBytesService) : IEndpointFilter
    {
        private readonly IBloomBytesService _bloomBytesService = bloomBytesService;

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
                .OfType<RegisterUserRequest>()
                .FirstOrDefault();
            if (request is null)
            {
                return Results.BadRequest("failed to read request body");
            }

            var isEmailExists = await _bloomBytesService.ContainsEmailDataAsync(request.Email);
            var isUserNameExists = await _bloomBytesService.ContainsUserNameDataAsync(request.UserName);


            if (!isEmailExists && !isUserNameExists)
            {
                return await next(context);
            }

            var usersRepository = context.HttpContext.RequestServices
                .GetRequiredService<IUsersRepository>();


            if (await usersRepository.ContainsExpressionAsync(u => u.Email == request.Email))
            {
                errors?.Add("email exists", [$"User with email {request.Email} already exists"]);
            }
            
            if (await usersRepository.ContainsExpressionAsync(u => u.UserName == request.UserName))
            {
                errors?.Add("username exists", [$"User with UserName {request.UserName} already exists"]);
            }

            context.HttpContext.Items["ValidationErrors"] = errors;
            
            return await next(context);
        }
    }
}