using DocumentFormat.OpenXml.Bibliography;
using Microsoft.EntityFrameworkCore;

namespace Lombeo.Api.Authorize.Infra.Entities
{
    public class Reading
    {
        public int Id { get; set; }
        public int CourseChapterId {  get; set; }
        public string Title { get; set; }
        public double Duration { get; set; }
        public string Description { get; set; }
        public int Index { get; set; }
    }

    public static class ReadingConfiguration
    {
        public static void Config(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reading>(entity =>
            {
                entity.ToTable("Readings");

                // Khóa chính
                entity.HasKey(e => e.Id);

                // Cấu hình các thuộc tính
                entity.Property(e => e.CourseChapterId)
                    .IsRequired();

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Duration)
                    .IsRequired();

                entity.Property(e => e.Index)
                    .IsRequired();

                entity.Property(e => e.Description);
            });
        }
    }

}
