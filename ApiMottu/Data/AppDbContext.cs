using Microsoft.EntityFrameworkCore;
using ApiMottu.Models;

namespace ApiMottu.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Moto> Motos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Moto>(entity =>
            {
                entity.ToTable("MOTOS"); 

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                      .HasColumnName("ID")
                      .ValueGeneratedOnAdd(); 

                entity.Property(e => e.Placa)
                      .HasColumnName("PLACA")
                      .HasMaxLength(10)
                      .IsRequired();

                entity.Property(e => e.Modelo)
                      .HasColumnName("MODELO")
                      .HasMaxLength(50)
                      .IsRequired();

                entity.Property(e => e.Status)
                      .HasColumnName("STATUS")
                      .HasMaxLength(20)
                      .IsRequired();

                entity.Property(e => e.Localizacao)
                      .HasColumnName("LOCALIZACAO")
                      .HasMaxLength(50)
                      .IsRequired();
            });
        }
    }
}
