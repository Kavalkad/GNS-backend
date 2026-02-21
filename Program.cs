using GNS.BackgroundServices;
using GNS.Data;
using GNS.Data.Repositories.Implementations;
using GNS.Data.Repositories.Interfaces;
using GNS.Endpoints;
using GNS.Endpoints.Filters;
using GNS.Extensions;
using GNS.Middleware;
using GNS.Services;
using GNS.Services.Implementations;
using GNS.Services.Interfaces;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.Extensions.Hosting;

using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
services.AddScopedFilters();
services.AddScopedReposiotries();
services.AddScopedServices();
services.AddHostedServices();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowAnyOrigin();
    });
});
services.AddDbContext<AppDbContext>(
    options =>
    {
        options.UseSqlite(configuration.GetConnectionString(nameof(AppDbContext)));
    }
);


services.AddApiAuthentication(configuration);
services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always
});
app.UseAuthorization();
//app.UseMiddleware<CustomExceptionMiddleware>();


app.MapUsersEndpoints();
app.MapOwnerEndpoints();
app.MapEmployeeEndpoints();

app.Run();
