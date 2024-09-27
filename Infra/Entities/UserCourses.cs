using Microsoft.EntityFrameworkCore;

namespace Lombeo.Api.Authorize.Infra.Entities
{
    public class UserCourses
    {
        public int UserId { get; set; }
        public int LearningCourseId { get; set; }
        public DateTime RegisteredAt { get; set; }
    }

    public static class UserCoursesConfiguration
    {
        public static void Config(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserCourses>(entity =>
            {
                entity.ToTable("UserCourses");

                // Thiết lập khóa chính
                entity.HasKey(uc => new { uc.UserId, uc.LearningCourseId });

                // Cột lưu thời gian đăng ký
                entity.Property(uc => uc.RegisteredAt)
                    .IsRequired();
            });
        }
    }
}
