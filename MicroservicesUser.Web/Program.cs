using Microservices.BusinessLogic.Implementations;
using Microservices.BusinessLogic.Interfaces;
using Microservices.Common.AutoMapperProfiles;
using MicroservicesUser.BusinessLogic.Implementations;
using MicroservicesUser.BusinessLogic.Interfaces;
using MicroservicesUser.BusinessLogic.ServerStorage.Interfaces;
using MicroservicesUser.BusinessLogic.SignalRHubs;
using MicroservicesUser.Common.AutoMapperProfiles;
using MicroservicesUser.DataAccess.Data;
using MicroservicesUser.DataAccess.Repository.Implementations;
using MicroservicesUser.DataAccess.Repository.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Setting up DbContext
string connection = builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
builder.Services.AddDbContext<MicroservicesUserDbContext>(options =>
options.UseNpgsql(connection, npgsqlOptions => npgsqlOptions.MigrationsAssembly("MicroservicesUser.Migrations")));

//Adding SignalR
builder.Services.AddSignalR();

//Adding Token store
builder.Services.AddSingleton<ITokenStore, InMemoryTokenStore>();

//Adding background services
builder.Services.AddHostedService<TokenExpiryBackgroundService>();

//Setting up automapper profiles
builder.Services.AddAutoMapper(typeof(UserProfile).Assembly);
builder.Services.AddAutoMapper(typeof(DashboardProfile).Assembly);

//Setting up Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IEmailVerificationRepository, EmailVerificationRepository>();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IProxyVpnDetectionRepository, ProxyVpnDetectionRepository>();

//Setting up business services
builder.Services.AddScoped<IJwtServices, JwtServices>();
builder.Services.AddScoped<IEmailServices, EmailServices>();
builder.Services.AddScoped<IAuthenticationServices, AuthenticationServices>();
builder.Services.AddScoped<IDashboardServices, DashboardServices>();
builder.Services.AddScoped<IEmailVerificationServices, EmailVerificationServices>();
builder.Services.AddScoped<IGenericAPIClientServices, GenericAPIClientServices>();
builder.Services.AddScoped<IEncryptDecryptServices, EncryptDecryptServices>();




//Injecting HttpClientService
builder.Services.AddHttpClient<GenericAPIClientServices>();

//Setting up JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
   .AddJwtBearer(options =>
   {
       options.RequireHttpsMetadata = false;
       options.SaveToken = true;
       options.TokenValidationParameters = new TokenValidationParameters
       {
           ValidateIssuer = false,
           ValidateAudience = true,
           ValidateLifetime = true,
           ValidateIssuerSigningKey = true,
           ValidIssuer = builder.Configuration["Jwt:Issuer"],
           ValidAudience = builder.Configuration["Jwt:Audience"],
           IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? string.Empty)),
       };
       options.Events = new JwtBearerEvents
       {
           OnMessageReceived = context =>
           {
               string token = context.Request.Cookies["AuthToken"] ?? string.Empty;
               if (!string.IsNullOrEmpty(token))
               {
                   context.Token = token;
               }
               return Task.CompletedTask;
           },
           OnChallenge = context =>
           {
               context.Response.Cookies.Delete("AuthToken");
               context.HandleResponse();
               string path = context.Request.Path.ToString();
               if (path.Contains("Admin", StringComparison.OrdinalIgnoreCase))
               {
                   context.Response.Redirect("/Authentication/AdminLogin");
               }
               else
               {
                   context.Response.Redirect("/Authentication/Login");
               }
               return Task.CompletedTask;
           }
       };
   });

WebApplication app = builder.Build();
app.MapHub<LogoutHub>("/logouthub");
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Authentication}/{action=Login}/{id?}");

app.Run();
