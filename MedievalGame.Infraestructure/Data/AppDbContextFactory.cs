using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using MedievalGame.Infraestructure.Data;
using Microsoft.Extensions.Configuration;

public class AppDbContextFactory() : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        optionsBuilder.UseSqlServer("Server=localhost,1433;Database=master;User Id=sa;Password=medieval123PasswordSegura;TrustServerCertificate=True;");

        return new AppDbContext(optionsBuilder.Options);
    }
}