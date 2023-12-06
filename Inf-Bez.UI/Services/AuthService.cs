using InfBez.Ui.Interfaces;
using InfBez.Ui.Models;
using InfBez.Ui.Repositories;
using InfBez.Ui.ViewModels;
using System.Diagnostics;

namespace InfBez.Ui.Services
{
    public class AuthService : IAuthService
    {
        private readonly UsersRepository repository;
        private readonly CookieService cookieService;
        private readonly ISession session;
        private readonly FailedSigInService failedSigIn;
        private readonly string uid = "20220531-B48C-9DCA-1BB2-B48C9DCA1BB3";

        public AuthService(UsersRepository repository, CookieService cookieService, ISession session, FailedSigInService failedSigIn)
        {
            this.repository = repository;
            this.cookieService = cookieService;
            this.session = session;
            this.failedSigIn = failedSigIn;
        }

        public async Task<bool> CheckToken(string token)
        {
            return token == await cookieService.GetCookies("token");
        }

        public async Task<User> GetCurrentUser()
        {
            var token = await cookieService.GetCookies("token");
            var id = await session.GetUserId(token);

            var user = await repository.ReadFirst(u => u.Id == id);
            return user;
        }

        public async Task<User> Login(AuthModel authModel)
        {
            if (GetUUID() != uid) throw new NotFiniteNumberException("UID не совпадает");
            var user = await repository.ReadFirst(u => u.Login == authModel.Login) ?? throw new NotImplementedException($"incorrect login or password");

            if (!PasswordHasher.VerifyPassword(authModel.Password, user.Password))
            {
                await failedSigIn.Inc(user.Id);
                throw new NotImplementedException($"incorrect login or password");
            }

            var token = Guid.NewGuid().ToString();
            await cookieService.SetCookies("token", token);
            await session.Add(user.Id, token);
            await failedSigIn.Delete(user.Id);
            return user;
        }

        public async Task Logout()
        {
            var token = await cookieService.GetCookies("token");
            await session.Delete(token);
        }

        public async Task Registrate(RegistrationModel registrationModel)
        {
            var user = await repository.ReadFirst(u => u.Login == registrationModel.Email);
            if (user != null) throw new NotImplementedException($"login {user.Login} is busy");

            await repository.Create(new User()
            {
                Name = registrationModel.Name,
                Adress = registrationModel.Adress,
                Phone = registrationModel.Phone,
                Email = registrationModel.Email,
                Login = registrationModel.Email,
                Password = PasswordHasher.HashPassword(registrationModel.Password),
            });
        }

        private string GetUUID()
        {
            var procStartInfo = new ProcessStartInfo("cmd", "/c " + "wmic csproduct get UUID")
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            var proc = new Process() { StartInfo = procStartInfo };
            proc.Start();

            return proc.StandardOutput.ReadToEnd().Replace("UUID", string.Empty).Trim().ToUpper();
        }
    }
}
