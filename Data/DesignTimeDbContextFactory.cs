using BarCreate.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BarCreate.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer("Server=localhost;Database=CaseStokDb;Trusted_Connection=True;TrustServerCertificate=True")
                .Options;

            return new AppDbContext(); // Eğer OnConfiguring içinde connection varsa bu satır yeterli
            // return new AppDbContext(options); // AppDbContext'te options alan ctor'un varsa bunu kullan
        }
    }
}