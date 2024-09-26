using Microsoft.EntityFrameworkCore;

namespace Lombeo.Api.Authorize.Infra.Entities
{
    public class CourseWeek
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Index { get; set; }
    }

    public static class CourseWeekConfiguration
    {
        public static void Config(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CourseWeek>(entity =>
            {
                entity.ToTable("CourseWeeks");

                // Khóa chính
                entity.HasKey(e => e.Id);

                // Cấu hình các thuộc tính
                entity.Property(e => e.CourseId)
                    .IsRequired();

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Index)
                    .IsRequired();

                entity.Property(e => e.Description)
                    .HasMaxLength(500); // Độ dài mô tả tối đa là 500 ký tự
            });
        }
    }

}
