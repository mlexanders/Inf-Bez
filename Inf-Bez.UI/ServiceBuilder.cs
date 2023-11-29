using InfBez.Ui.Interfaces;
using InfBez.Ui.Repositories;
using InfBez.Ui.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Radzen;

namespace InfBez.Ui
{
    public static class ServiceBuilder
    {
        public static void RegistrateCommonServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(o => o.UseSqlServer("Server=localhost;Database=infBez;Trusted_Connection=True;TrustServerCertificate=True;Integrated Security=true;Encrypt=false"));
            services.AddTransient<UsersRepository>();
            services.AddTransient<AttachedFilesRepository>();
            services.AddScoped<FileChecker>();

            services.AddScoped<CookieService>();
            services.AddScoped<TokenService>();
            services.AddScoped<AuthService>();
            services.AddScoped<AuthenticationStateProvider, TokenService>();
            services.AddSingleton<ISession, SessionService>();
            services.AddSingleton<FailedSigInService>();

            services.AddOptions();
            services.AddAuthorizationCore();

            services.AddScoped<DialogService>();
            services.AddScoped<NotificationService>();
            services.AddScoped<TooltipService>();
            services.AddScoped<ContextMenuService>();
        }
    }
}
