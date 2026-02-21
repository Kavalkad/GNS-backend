

using GNS.BackgroundServices;
using GNS.Endpoints.Filters;
using GNS.Services.Implementations;
using GNS.Services.Interfaces;

namespace GNS.Extensions
{
    public static partial class AddServices
    {
        public static IServiceCollection AddHostedServices(this IServiceCollection services)
        {
            services.AddHostedService<MontlyResetBonusesService>();
            services.AddHostedService<MontlyResetPenaltiesService>();

            return services;
        }
    }
}