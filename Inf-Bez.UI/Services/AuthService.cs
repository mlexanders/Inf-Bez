using InfBez.Ui.Interfaces;
using InfBez.Ui.Models;
using InfBez.Ui.ViewModels;

namespace InfBez.Ui.Services
{
    public class AuthService : IAuthService
    {
        private readonly UsersRepository repository;
        private readonly CookieService cookieService;
        private readonly ISession session;

        public AuthService(UsersRepository repository, CookieService cookieService, ISession session)
        {
            this.repository = repository;
            this.cookieService = cookieService;
            this.session = session;
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
            var user = await repository.ReadFirst(u => u.Login == authModel.Login) ?? throw new NotImplementedException($"incorrect login or password");
            if (!PasswordHasher.VerifyPassword(authModel.Password, user.Password)) throw new NotImplementedException($"incorrect login or password");

            var token = Guid.NewGuid().ToString();
            await cookieService.SetCookies("token", token);
            await session.Add(user.Id, token);

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
                Login = registrationModel.Email,
                Password = PasswordHasher.HashPassword(registrationModel.Password),
            });
        }
    }
}
