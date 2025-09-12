using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BarCreate.Models;

namespace BarCreate.Data
{
    public class AppDbContext : DbContext
    {
       
        private const string Conn =
            "Server=localhost;Database=CaseStokDb;Trusted_Connection=True;TrustServerCertificate=True";

        public DbSet<StokKartBilgi> StokKartBilgileri => Set<StokKartBilgi>();
        public DbSet<Barkod> Barkodlar => Set<Barkod>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer(Conn);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StokKartBilgi>(e =>
            {
                e.ToTable("StokKartBilgi");
                e.HasKey(x => x.StokNo);
                e.Property(x => x.StokNo).HasColumnType("char(18)").IsRequired();
                e.Property(x => x.KasaIciMiktar).HasColumnType("decimal(18,3)");
                e.Property(x => x.EksiltmeMiktar).HasColumnType("decimal(18,3)");
            });

            modelBuilder.Entity<Barkod>(e =>
            {
                e.ToTable("Barkod");
                e.HasKey(x => x.BarkodNo);
                e.Property(x => x.BarkodNo).HasColumnType("char(10)").IsRequired();

                e.Property(x => x.StokNo).HasColumnType("char(18)").IsRequired();
                e.Property(x => x.KasaIciMiktar).HasColumnType("decimal(18,3)");
                e.Property(x => x.EksiltmeMiktar).HasColumnType("decimal(18,3)");

                e.HasIndex(x => x.StokNo);

                e.HasOne(x => x.Stok)
                 .WithMany(s => s.Barkodlar)
                 .HasForeignKey(x => x.StokNo)
                 .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
