using GNS.Endpoints.Filters;
using GNS.Services.Implementations;
using GNS.Services.Interfaces;

namespace GNS.Extensions
{
    public static partial class AddServices
    {
        public static IServiceCollection AddScopedFilters(this IServiceCollection services)
        {
            services.AddScoped<ManagerAccessToCyberClubFilter>();
            services.AddScoped<ManagerAccessToEmployeeFilter>();

            services.AddScoped<EmployeeAccessToGamingPlaceFilter>();
            services.AddScoped<EmployeeAccessToOrderFilter>();

            services.AddScoped<OwnerAccessToCyberClubFilter>();
            services.AddScoped<OwnerAccessToEmployeeFilter>();
            services.AddScoped<OwnerAccessToWorkingHoursFilter>();
            services.AddScoped<OwnerAccessToGamingPlaceFilter>();

            services.AddScoped<QueryNamesFilter>();
            services.AddScoped<QueryUserNameFilter>();
            services.AddScoped<QueryCityFilter>();
            services.AddScoped<QueryGameTitleFilter>();
            services.AddScoped<QueryEmailFilter>();

            services.AddScoped<OrderStatusFilter>();
            services.AddScoped<TerminalValidationFilter>();
            services.AddScoped<AddressFilter>();
            services.AddScoped<BonusFilter>();
            services.AddScoped<CityFilter>();
            services.AddScoped<EmailFilter>();
            services.AddScoped<NameFilter>();
            services.AddScoped<NumberFilter>();
            services.AddScoped<PasswordFilter>();
            services.AddScoped<PenaltyFilter>();
            services.AddScoped<PricePerHourFilter>();
            services.AddScoped<SecretWordFilter>();
            services.AddScoped<SuperSecretWordFilter>();
            services.AddScoped<TaxIdentificationNumberFilter>();
            services.AddScoped<TimeSpanFilter>();
            services.AddScoped<UserNameFilter>();
            services.AddScoped<BloomFilter>();


            return services;
        }
    }
}