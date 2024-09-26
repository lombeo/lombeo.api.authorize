using Microsoft.EntityFrameworkCore;

namespace Lombeo.Api.Authorize.Infra.Entities
{
    public class Score
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int QuizId { get; set; }
        public int Point { get; set; }
    }

    public static class ScoreConfiguration
    {
        public static void Config(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Score>(entity =>
            {
                entity.ToTable("Scores");

                // Khóa chính
                entity.HasKey(e => e.Id);

                // Cấu hình các thuộc tính
                entity.Property(e => e.UserId)
                    .IsRequired();

                entity.Property(e => e.QuizId)
                    .IsRequired();

                entity.Property(e => e.Point)
                    .IsRequired(); // Bắt buộc để lưu trữ số điểm
            });
        }
    }

}
