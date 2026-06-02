using GNS.Data;
using GNS.Endpoints;
using GNS.Extensions;
using GNS.Services;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;





var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
services.Configure<RefreshTokenOptions>(configuration.GetSection(nameof(RefreshTokenOptions)));

services.AddAntiforgery();



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
builder.Host.UseDefaultServiceProvider(options =>
{
    options.ValidateScopes = true;
    options.ValidateOnBuild = true;
});


services.AddApiAuthentication(configuration);


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

app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();
app.UseMiddlewares();

app.MapUsersEndpoints();
app.MapOwnerEndpoints();
app.MapEmployeeEndpoints();

app.Run();

