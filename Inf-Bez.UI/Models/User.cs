using Actions.Common.Base;

namespace InfBez.Ui.Models
{
    public class User : Entity<int>
    {
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
