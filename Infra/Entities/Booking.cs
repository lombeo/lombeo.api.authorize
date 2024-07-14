using Microsoft.EntityFrameworkCore;

namespace Lombeo.Api.Authorize.Infra.Entities
{
    public class Booking : CommonEntity
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool Gender { get; set; }
        public string Address { get; set; }
        public DateTime Dob { get; set; }
        public string? Note { get; set; }
        public int DoctorId { get; set; }
        public int ShiftTo { get; set; }
        public int Status { get; set; }
    }

    public static class BookingConfiguration
    {
        public static void Config(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.ToTable("Bookings");
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

                entity.Property(e => e.Dob)
                   .IsRequired();

                entity.Property(e => e.Note);
                entity.Property(e => e.DoctorId);
                entity.Property(e => e.ShiftTo);
                entity.Property(e => e.Status);
                entity.HasIndex(e => e.Email);
            });
        }
    }
}
