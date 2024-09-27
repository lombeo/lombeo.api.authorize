using Lombeo.Api.Authorize.Infra.Enums;
using Microsoft.EntityFrameworkCore;

namespace Lombeo.Api.Authorize.Infra.Entities
{
    public class Question
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public string Content { get; set; }
        public QuizContentType Type { get; set; }
        public EntryLevel EntryLevel { get; set; }
    }

    public static class QuestionConfiguration
    {
        public static void Config(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Question>(entity =>
            {
                entity.ToTable("Questions");

                // Khóa chính
                entity.HasKey(e => e.Id);

                // Cấu hình các thuộc tính
                entity.Property(e => e.QuizId)
                    .IsRequired();

                entity.Property(e => e.Content)
                    .IsRequired(); // Giới hạn độ dài nội dung câu hỏi là 500 ký tự

                entity.Property(c => c.Type)
                   .HasConversion<int>() // Sử dụng kiểu int để lưu trữ enum
                   .IsRequired();

                entity.Property(c => c.EntryLevel)
                   .HasConversion<int>() // Sử dụng kiểu int để lưu trữ enum
                   .IsRequired();// Bắt buộc xác định loại câu hỏi
            });
        }
    }


}
