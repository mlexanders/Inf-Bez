namespace InfBez.Ui.Interfaces
{
    public interface ISession
    {
        Task<int> GetUserId(string token);
        Task Add(int userId, string token);
        Task Delete(string token);
    }
}
