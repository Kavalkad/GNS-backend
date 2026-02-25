

using GNS.Services.Implementations;
using GNS.Services.Interfaces;

namespace GNS.Extensions
{
    public static partial class AddServices
    {
        public static IServiceCollection AddScopedServices(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddScoped<IJwtProvider, JwtProvider>();
            services.AddScoped<IHasher, Hasher>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<ICyberClubService, CyberClubService>();
            services.AddScoped<IGamingPlaceService, GamingPlaceService>();
            services.AddScoped<IGameService, GameService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IWorkingHoursService, WorkingHoursService>();
            services.AddScoped<IOwnerService, OwnerService>();
            services.AddScoped<ITimeSlotsService, TimeSlotsService>();
            services.AddScoped<IGameGamingPlaceService, GameGamingPlaceService>();
            services.AddScoped<IBloomFilterService, BloomFilterService>();
            
            return services;
        }
    }
}