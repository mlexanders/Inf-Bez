using InfBez.Ui.Models;
using InfBez.Ui.ViewModels;

namespace InfBez.Ui.Interfaces
{
    public interface IAuthService
    {
        Task Registrate(RegistrationModel registrationModel, long chatId);
        Task<User> Login(AuthModel authModel);
        Task<bool> CheckToken(string token);
        Task<User> GetCurrentUser();
    }
}
