using Microsoft.EntityFrameworkCore;

namespace Lombeo.Api.Authorize.Infra.Entities
{
    public class EnrollCourse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }
    }

    public static class EnrollCourseConfiguration
    {
        public static void Config(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EnrollCourse>(entity =>
            {
                // Tên bảng
                entity.ToTable("EnrollCourses");

                // Khóa chính
                entity.HasKey(e => e.Id);

                // Thuộc tính
                entity.Property(e => e.UserId)
                    .IsRequired();

                entity.Property(e => e.CourseId)
                    .IsRequired();

                // Tạo chỉ mục (index) trên cặp (UserId, CourseId) để đảm bảo một người dùng chỉ đăng ký một khóa học duy nhất
                entity.HasIndex(e => new { e.UserId, e.CourseId })
                    .IsUnique();
            });
        }
    }
}
