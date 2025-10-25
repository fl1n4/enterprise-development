using RealEstateAgency.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace RealEstateAgency.Infrastructure.Database;

public class RealEstateAgencyDbContext(DbContextOptions<RealEstateAgencyDbContext> options) : DbContext(options)
{
    public DbSet<Client> Clients { get; set; }

    public DbSet<RealEstateObject> RealEstateObjects { get; set; }

    public DbSet<Request> Requests { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(builder =>
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.FullName)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(c => c.PassportNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(c => c.Phone)
                .IsRequired()
                .HasMaxLength(20);

            builder.HasIndex(c => c.PassportNumber).IsUnique();
        });

        modelBuilder.Entity<RealEstateObject>(builder =>
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.CadastralNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(r => r.Address)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(r => r.Floors)
                .IsRequired();

            builder.Property(r => r.TotalArea)
                .IsRequired();

            builder.Property(r => r.Rooms)
                .IsRequired();

            builder.Property(r => r.CeilingHeight)
                .IsRequired();

            builder.Property(r => r.FloorNumber)
                .IsRequired();

            builder.Property(r => r.HasEncumbrance)
                .IsRequired();

            builder.Property(r => r.Type)
                .IsRequired();

            builder.Property(r => r.Purpose)
                .IsRequired();

            builder.HasIndex(r => r.CadastralNumber).IsUnique();
        });

        modelBuilder.Entity<Request>(builder =>
        {
            builder.HasKey(r => r.Id);

            builder.HasOne(r => r.Client)
                .WithMany()
                .HasForeignKey("ClientId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(r => r.Property)
                .WithMany()
                .HasForeignKey("PropertyId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(r => r.Type)
                .IsRequired(false);

            builder.Property(r => r.Amount)
                .IsRequired(false);

            builder.Property(r => r.DateCreated)
                .IsRequired(false);
        });
    }
}
