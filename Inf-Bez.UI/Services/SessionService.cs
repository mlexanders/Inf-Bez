using InfBez.Ui.Interfaces;

namespace InfBez.Ui.Services
{
    public class SessionService : ISession
    {
        public Dictionary<string, int> Sessions { get; set; } = new();

        public async Task Add(int userId, string token)
        {
            try
            {
                lock (Sessions)
                {
                    Sessions.Add(token, userId);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task Delete(string token)
        {
            try
            {
                lock (Sessions)
                {
                    Sessions.Remove(token);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task<int> GetUserId(string token)
        {
            if (Sessions.TryGetValue(token, out var id)) return id;
            return 0;
        }
    }
}
