using GNS.Endpoints.Filters;
using GNS.Services.Implementations;
using GNS.Services.Interfaces;

namespace GNS.Extensions
{
    public static partial class AddServices
    {
        public static IServiceCollection AddScopedFilters(this IServiceCollection services)
        {
            services.AddScoped<BonusFilter>();
            services.AddScoped<PenaltyFilter>();
            services.AddScoped<PersonFilter>();
            services.AddScoped<RegisterEmployeeFilter>();
            services.AddScoped<UpdateEmployeeFilter>();
            services.AddScoped<NamesFilter>();
            services.AddScoped<EmailFilter>();
            services.AddScoped<UserNameFilter>();
            services.AddScoped<OrderStatusFilter>();
            services.AddScoped<FinalValidationFilter>();
            services.AddScoped<UpdateWorkingHoursFilter>();
            services.AddScoped<BloomFilter>();

            return services;
        }
    }
}