//using InfBez.Ui;
//using InfBez.Ui.Interfaces;
//using InfBez.Ui.Services;
//using Microsoft.AspNetCore.Components.Authorization;
//using Microsoft.AspNetCore.Components.Web;
//using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
//using Microsoft.EntityFrameworkCore;
//using Radzen;

////var builder = WebAssemblyHostBuilder.CreateDefault(args);
////builder.RootComponents.Add<App>("#app");
////builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddDbContext<AppDbContext>(o => o.UseSqlServer("Server=localhost;Database=infBez;Trusted_Connection=True;TrustServerCertificate=True;Integrated Security=true;Encrypt=false"));
//builder.Services.AddScoped<UsersRepository>();

//builder.Services.AddScoped<CookieService>();
//builder.Services.AddScoped<TokenService>();
//builder.Services.AddScoped<AuthService>();
//builder.Services.AddScoped<AuthenticationStateProvider, TokenService>();
//builder.Services.AddSingleton<ISession, SessionService>();

//builder.Services.AddOptions();
//builder.Services.AddAuthorizationCore();

//builder.Services.AddScoped<DialogService>();
//builder.Services.AddScoped<NotificationService>();
//builder.Services.AddScoped<TooltipService>();
//builder.Services.AddScoped<ContextMenuService>();

//await builder.Build().RunAsync();
