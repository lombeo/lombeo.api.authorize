using Lombeo.Api.Authorize.DTO.DoctorDTO;
using Lombeo.Api.Authorize.Infra.Helps;
using Microsoft.EntityFrameworkCore;

namespace Lombeo.Api.Authorize.Infra.Entities
{
    public class Doctor : CommonEntity
    {
        public int Id { get; set; }
        public string ProfilePic { get; set; }
        public string Name { get; set; }
        public string BriefInfo { get; set; }
        public string Room { get; set; }
        public string Location { get; set; }
        public List<int> Shift { get; set; }
        public double Price { get; set; }
        public List<DoctorInfo> Info { get; set; }
    }

    public static class DoctorConfiguration
    {
        public static void Config(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.ToTable("Doctors");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.ProfilePic).HasMaxLength(255);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.BriefInfo).HasMaxLength(500);
                entity.Property(e => e.Room).HasMaxLength(50);
                entity.Property(e => e.Location).HasMaxLength(100);

                entity.Property(e => e.Shift)
                    .HasConversion(new JsonValueConverter<List<int>>())
                    .HasColumnType("jsonb");

                entity.Property(e => e.Price)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.Info)
                    .HasConversion(new JsonValueConverter<List<DoctorInfo>>())
                    .HasColumnType("jsonb");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp without time zone");
                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("timestamp without time zone");
                entity.Property(e => e.Deleted)
                    .HasDefaultValue(false);

                entity.HasIndex(e => e.Name).HasDatabaseName("IX_Doctors_Name");
                entity.HasIndex(e => e.Deleted);
            });
        }
    }
}
