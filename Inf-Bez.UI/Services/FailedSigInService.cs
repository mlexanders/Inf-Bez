namespace InfBez.Ui.Services
{
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
