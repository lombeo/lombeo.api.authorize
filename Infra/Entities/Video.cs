using Microsoft.EntityFrameworkCore;

namespace Lombeo.Api.Authorize.Infra.Entities
{
    public class Video
    {
        public int Id { get; set; }
        public int CourseChapterId { get; set; }
        public string Title { get; set; }
        public double Duration { get; set; }
        public string Description { get; set; }
        public string VideoUrl { get; set; }
        public int Index { get; set; }
    }

    public static class VideoConfiguration
    {
        public static void Config(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Video>(entity =>
            {
                entity.ToTable("Videos");

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

                entity.Property(e => e.Description);

                entity.Property(e => e.VideoUrl)
                    .IsRequired();

                entity.Property(e => e.Index)
                    .IsRequired();
            });
        }
    }

}
