using Microsoft.EntityFrameworkCore;

namespace Lombeo.Api.Authorize.Infra.Entities
{
    public class Review : CommonEntity
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int ReviewerId { get; set; }
        public string Description { get; set; }
        public double Rating { get; set; }

    }

    public static class ReviewConfiguration
    {
        public static void Config(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Review>(entity =>
            {
                // Tên bảng
                entity.ToTable("Reviews");

                // Khóa chính
                entity.HasKey(e => e.Id);

                entity.HasIndex(e => e.Deleted);
                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("timestamp without time zone");
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp without time zone");

                // Thuộc tính
                entity.Property(e => e.CourseId)
                    .IsRequired();

                entity.Property(e => e.ReviewerId)
                    .IsRequired();

                entity.Property(e => e.Description)
                    .HasMaxLength(2000);

                entity.Property(e => e.Rating)
                    .IsRequired()
                    .HasColumnType("decimal(3,2)")  // Đảm bảo lưu rating với độ chính xác 2 chữ số thập phân
                    .HasDefaultValue(0.0);

                // Chỉ mục để ngăn một người dùng đánh giá một khóa học nhiều lần
                entity.HasIndex(e => new { e.CourseId, e.ReviewerId })
                    .IsUnique();
            });
        }
    }
}
