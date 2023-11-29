using Actions.Common.Base;

namespace InfBez.Ui.Models
{
    public class User : Entity<int>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Adress { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
    }

    public enum Role
    {
        User, Admin
    }
}
