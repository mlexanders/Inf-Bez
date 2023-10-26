using InfBez.Ui.Interfaces;
using InfBez.Ui.Models;
using InfBez.Ui.ViewModels;

namespace InfBez.Ui.Services
{
    public class AuthService : IAuthService
    {
        public async Task<bool> CheckToken(string token)
        {
            return true;
        }

        public async Task<User> GetCurrentUser()
        {
            return new User();
        }

        public async Task<User> Login(AuthModel authModel)
        {
            return new User();
        }

        public async Task Registrate(RegistrationModel registrationModel, long chatId)
        {
            return;
        }
    }
}
