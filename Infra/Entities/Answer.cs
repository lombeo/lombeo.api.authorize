using Microsoft.EntityFrameworkCore;

namespace Lombeo.Api.Authorize.Infra.Entities
{
    public class Answer
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string Content { get; set; }
        public bool IsCorrect { get; set; }
    }

    public static class AnswerConfiguration
    {
        public static void Config(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Answer>(entity =>
            {
                entity.ToTable("Answers");

                // Khóa chính
                entity.HasKey(e => e.Id);

                // Cấu hình các thuộc tính
                entity.Property(e => e.QuestionId)
                    .IsRequired();

                entity.Property(e => e.Content)
                    .IsRequired();

                entity.Property(e => e.IsCorrect)
                    .IsRequired(); // Bắt buộc xác định câu trả lời có đúng hay không
            });
        }
    }

}
