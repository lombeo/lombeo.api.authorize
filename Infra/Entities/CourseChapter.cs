using Microsoft.EntityFrameworkCore;

namespace Lombeo.Api.Authorize.Infra.Entities
{
    public class CourseChapter
    {
        public int Id { get; set; }
        public int WeekId { get; set; }
        public string Title { get; set; }
        public int Index { get; set; }
    }

    public static class CourseChapterConfiguration
    {
        public static void Config(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CourseChapter>(entity =>
            {
                entity.ToTable("CourseChapters");

                // Khóa chính
                entity.HasKey(e => e.Id);

                // Cấu hình các thuộc tính
                entity.Property(e => e.WeekId)
                    .IsRequired();

                entity.Property(e => e.Index)
                    .IsRequired();

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(255);
            });
        }
    }

}
