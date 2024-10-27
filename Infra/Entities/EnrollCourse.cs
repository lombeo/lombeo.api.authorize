using Lombeo.Api.Authorize.Infra.Enums;
using Microsoft.EntityFrameworkCore;

namespace Lombeo.Api.Authorize.Infra.Entities
{
    public class EnrollCourse : CommonEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string InvoiceCode { get; set; }
        public int CourseId { get; set; }
        public string TransactionImgUrl { get; set; }
        public EnrollStatus Status { get; set; } = EnrollStatus.Pending;
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

                entity.HasIndex(e => e.Deleted);
                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("timestamp without time zone");
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp without time zone");

                // Thuộc tính
                entity.Property(e => e.UserId)
                    .IsRequired();

                entity.Property(e => e.CourseId)
                    .IsRequired();

                entity.Property(e => e.InvoiceCode)
                    .IsRequired();

                entity.Property(c => c.TransactionImgUrl)
                   .IsRequired();

                entity.Property(c => c.Status).HasDefaultValue(2)
                   .HasConversion<int>() // Sử dụng kiểu int để lưu trữ enum
                   .IsRequired();

                // Tạo chỉ mục (index) trên cặp (UserId, CourseId) để đảm bảo một người dùng chỉ đăng ký một khóa học duy nhất
                entity.HasIndex(e => new { e.UserId, e.CourseId })
                    .IsUnique();
            });
        }
    }
}
