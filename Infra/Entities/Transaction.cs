using Lombeo.Api.Authorize.Infra.Enums;
using Microsoft.EntityFrameworkCore;

namespace Lombeo.Api.Authorize.Infra.Entities
{
    public class Transaction : CommonEntity
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public decimal Price { get; set; }
        public int UserId { get; set; }
        public TransactionStatus Status { get; set; }
    }

    public static class TransactionConfiguration
    {
        public static void Config(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>(entity =>
            {
                // Đặt tên bảng
                entity.ToTable("Transactions");

                // Thiết lập khóa chính
                entity.HasKey(t => t.Id);

                // Thiết lập các cột
                entity.Property(t => t.CourseId)
                    .IsRequired();

                entity.Property(t => t.CourseName)
                    .IsRequired()
                    .HasMaxLength(255);  // Độ dài tối đa cho CourseName là 255 ký tự

                entity.Property(t => t.Price)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");  // Định dạng cho cột Price

                entity.Property(t => t.UserId)
                    .IsRequired();

                // Thiết lập cho TransactionStatus enum, lưu dưới dạng số (int)
                entity.Property(t => t.Status)
                    .IsRequired()
                    .HasConversion<int>();  // Chuyển đổi enum thành kiểu int
            });
        }
    }

}
