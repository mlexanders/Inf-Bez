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


    public class FailedSigInService
    {
        public Dictionary<int, int> Signins { get; set; } = new();

        public async Task Inc(int userId)
        {
            try
            {
                lock (Signins)
                {
                    if (Signins.TryGetValue(userId, out var sigins))
                    {
                        Signins.Remove(userId);
                        Signins.Add(userId, ++sigins);
                        if (sigins > 3) throw new NotImplementedException("Превышено количество попыток ввхода");
                    }
                    else
                    {
                        Signins.Add(userId, 1);
                    }
                }
            }
            catch (NotImplementedException e)
            {
                throw;
            }
        }

        public async Task Delete(int userId)
        {
            try
            {
                lock (Signins)
                {
                    Signins.Remove(userId);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task<int> GetCount(int userId)
        {
            if (Signins.TryGetValue(userId, out var sigins)) return sigins;
            return 0;
        }
    }
}
