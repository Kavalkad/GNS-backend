using GNS.Data.Repositories.Implementations;
using GNS.Data.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace GNS.Extensions
{
    public static partial class AddServices
    {
        public static IServiceCollection AddScopedReposiotries(this IServiceCollection services)
        {
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IEmployeesRepository, EmployeesRepository>();
            services.AddScoped<ICyberClubsRepository, CyberClubsRepository>();
            services.AddScoped<IGamingPlacesRepository, GamingPlacesRepository>();
            services.AddScoped<IGamesRepository, GamesRepository>();
            services.AddScoped<IOrdersRepository, OrdersRepository>();
            services.AddScoped<IWorkingHoursRepository, WorkingHoursRepository>();
            services.AddScoped<IOwnersRepository, OwnersRepository>();
            services.AddScoped<IGameGPsRepository, GameGPsRepository>();

            return services;
        }
    }
}