using InfBez.Ui;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;


Console.WriteLine();
public class ContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)

        => new(new DbContextOptionsBuilder<AppDbContext>().UseSqlServer("Server=localhost;Database=infBez;Trusted_Connection=True;TrustServerCertificate=True;Integrated Security=true;Encrypt=false", b => b.MigrationsAssembly("infBez.Context")).Options);
}