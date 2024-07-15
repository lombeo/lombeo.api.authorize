using Microsoft.EntityFrameworkCore;

namespace Lombeo.Api.Authorize.Infra.Entities
{
    public class Messenger : CommonEntity
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public static class MessageConfiguration
    {
        public static void Config(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Messenger>(entity =>
            {
                entity.ToTable("Messages");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.SenderId)
                    .IsRequired();

                entity.Property(e => e.ReceiverId)
                    .IsRequired();

                entity.Property(e => e.Content)
                    .IsRequired();

                entity.Property(e => e.Timestamp)
                    .HasColumnType("timestamp without time zone")
                    .IsRequired();

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp without time zone")
                    .IsRequired();

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("timestamp without time zone")
                    .IsRequired();

                entity.Property(e => e.Deleted)
                    .HasDefaultValue(false)
                    .IsRequired();

                entity.HasIndex(e => e.Deleted);
            });
        }
    }
}
