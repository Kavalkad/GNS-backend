using GNS.Contracts.Requests;
using GNS.Data.Repositories.Interfaces;
using GNS.Services.Interfaces;

namespace GNS.Endpoints.Filters
{
    public class BloomFilter : IEndpointFilter
    {
        private readonly IBloomBytesService _bloomBytesService;
        public BloomFilter(IBloomBytesService bloomBytesService)
        {
            _bloomBytesService = bloomBytesService;
        }
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
                return Results.BadRequest("Invalid request body");
            }

            var isEmailExists = await _bloomBytesService.FindEmailData(request.Email);
            var isUserNameExists = await _bloomBytesService.FindUserNameData(request.UserName);


            if (!isEmailExists && !isUserNameExists)
            {
                return await next(context);
            }

            var usersRepository = context.HttpContext.RequestServices
                .GetRequiredService<IUsersRepository>();

            if (await usersRepository.ContainsEmail(request.Email))
            {
                errors?.Add("email exists", [$"User with email {request.Email} already exists"]);
            }
            if (await usersRepository.ContainsUserName(request.UserName))
            {
                errors?.Add("username exists", [$"User with UserName {request.Email} already exists"]);
            }

            context.HttpContext.Items["ValidationErrors"] = errors;
            
            return await next(context);
        }
    }
}