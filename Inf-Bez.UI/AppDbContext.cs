using InfBez.Ui.Models;
using Microsoft.EntityFrameworkCore;

namespace InfBez.Ui
{
    public class AppDbContext : DbContext
    {
        DbSet<User> Users { get; set; }

        public AppDbContext(DbContextOptions options) : base(options) { }
    }
}
