namespace InfBez.Ui.Difinitions
{
    public static class AppSettings
    {
        public static string BasePath { get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", "inf-bez");
    }
}
