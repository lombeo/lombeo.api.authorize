using Microsoft.EntityFrameworkCore;

namespace Lombeo.Api.Authorize.Infra.Entities
{
    public class RegisteredCourse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }
    }

    public static class RegisteredCourseConfiguration
    {
        public static void Config(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RegisteredCourse>(entity =>
            {
                entity.ToTable("RegisteredCourses");

                // Khóa chính
                entity.HasKey(e => e.Id);

                // Cấu hình các thuộc tính
                entity.Property(e => e.UserId)
                    .IsRequired();

                entity.Property(e => e.CourseId)
                    .IsRequired();
            });
        }
    }

}
