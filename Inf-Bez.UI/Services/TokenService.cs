using InfBez.Ui.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace InfBez.Ui.Services
{
    public class TokenService : AuthenticationStateProvider
    {
        private readonly CookieService cookieService;
        private readonly AuthService authService;

        public TokenService(AuthService authService, CookieService cookieService)
        {
            this.authService = authService;
            this.cookieService = cookieService;
        }

        public async Task SetLogoutState()
        {
            await cookieService.SetCookies("token", "");
            await authService.Logout();
            NotifyAuthenticationStateChanged(Task.FromResult(GetStateAnonymous()));
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await cookieService.GetCookies("token");
            if (string.IsNullOrWhiteSpace(token) || !await authService.CheckToken(token)) return GetStateAnonymous();

            var user = await authService.GetCurrentUser();

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Authentication, token),
                new Claim(ClaimTypes.Role, user.Role is Role.Admin ? "Admin" : "User")
            };

            var claimsIdentity = new ClaimsIdentity(claims, "Token");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            return new AuthenticationState(claimsPrincipal);
        }

        private static AuthenticationState GetStateAnonymous()
        {
            var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
            return new(anonymous);
        }
    }
}
