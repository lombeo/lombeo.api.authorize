using Lombeo.Api.Authorize.Infra.Enums;
using Microsoft.EntityFrameworkCore;

namespace Lombeo.Api.Authorize.Infra.Entities
{
    public class Quiz
    {
        public int Id { get; set; }
        public int CourseChapterId { get; set; }
        public string Title { get; set; }
        public int Point { get; set; }
        public EntryLevel entryLevel { get; set; }
        public string Description { get; set; }
        public int TimeLimit { get; set; }
        public int Index { get; set; }

    }

    public static class QuizConfiguration
    {
        public static void Config(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Quiz>(entity =>
            {
                entity.ToTable("Quizzes");

                // Khóa chính
                entity.HasKey(e => e.Id);

                // Cấu hình các thuộc tính
                entity.Property(e => e.CourseChapterId)
                    .IsRequired();

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Point)
                    .IsRequired();

                entity.Property(e => e.entryLevel)
                    .IsRequired();

                entity.Property(e => e.Description);

                entity.Property(e => e.TimeLimit)
                    .IsRequired();
            });
        }
    }

}
