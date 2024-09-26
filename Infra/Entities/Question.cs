using Lombeo.Api.Authorize.Infra.Enums;
using Microsoft.EntityFrameworkCore;

namespace Lombeo.Api.Authorize.Infra.Entities
{
    public class Question
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public string Content { get; set; }
        public ContentType Type { get; set; }
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

                entity.Property(e => e.Type)
                    .IsRequired(); // Bắt buộc xác định loại câu hỏi
            });
        }
    }


}
