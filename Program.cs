using GNS.BackgroundServices;
using GNS.Data;
using GNS.Data.Repositories.Implementations;
using GNS.Data.Repositories.Interfaces;
using GNS.Endpoints;
using GNS.Endpoints.Filters;
using GNS.Extensions;
using GNS.Services;
using GNS.Services.Implementations;
using GNS.Services.Interfaces;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.Extensions.Hosting;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi;


var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
services.AddScopedFilters();
services.AddScopedReposiotries();
services.AddScopedServices();
services.AddHostedServices();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "GNS API",
        Version = "v1",
        Description = "API с поддержкой JWT авторизации"
    });

    // Добавление определения безопасности
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Введите JWT токен в формате: Bearer {your_token}"
    });
}
);
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

app.UseMiddlewares();

app.UseAuthentication();
app.UseAuthorization();

app.MapUsersEndpoints();
app.MapOwnerEndpoints();
app.MapEmployeeEndpoints();

app.Run();

