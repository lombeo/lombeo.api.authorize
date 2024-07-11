using Microsoft.EntityFrameworkCore;

namespace Lombeo.Api.Authorize.Infra.Entities
{
    public class UserProfile : CommonEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public bool Gender { get; set; }
        public string Address { get; set; }
        public string PicProfile { get; set; }
        public DateTime Dob {  get; set; }
        public string? Note { get; set; }
        public string? School { get; set; }
        public string? WorkAt { get; set; }
    }

    public static class UserProfileConfiguration
    {
        public static void Config(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserProfile>(entity =>
            {
                entity.ToTable("UserProfiles");
                entity.HasKey(e => e.Id);

                entity.HasIndex(e => e.Deleted);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("timestamp without time zone");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp without time zone");

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(20);

                entity.Property(e => e.Gender)
                    .IsRequired();

                entity.Property(e => e.Address)
                    .HasMaxLength(500);

                entity.Property(e => e.PicProfile)
                    .HasMaxLength(500);

                entity.Property(e => e.Dob)
                    .IsRequired();

                entity.Property(e => e.Note);

                entity.Property(e => e.School)
                    .HasMaxLength(255);

                entity.Property(e => e.WorkAt)
                    .HasMaxLength(255);

                entity.HasOne<User>()
                    .WithOne()
                    .HasForeignKey<UserProfile>(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }

}
