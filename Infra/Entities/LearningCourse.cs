using Lombeo.Api.Authorize.Infra.Enums;
using Microsoft.EntityFrameworkCore;

namespace Lombeo.Api.Authorize.Infra.Entities
{
	public class LearningCourse : CommonEntity
	{
        public int Id { get; set; }
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }
        public int AuthorId { get; set; }
        public double Price { get; set; }
        public bool HasCert {  get; set; }
        public ContentType ContentType { get; set; } = ContentType.LearningCourse;
    }

	public static class LearningCourseConfiguration
	{
		public static void Config(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<LearningCourse>(entity =>
			{
				entity.ToTable("LearningCourses");
				entity.HasKey(e => e.Id);

				entity.Property(e => e.CourseName)
					.IsRequired()
					.HasMaxLength(100);

				entity.Property(e => e.CourseDescription)
					.HasMaxLength(500);

				entity.Property(e => e.Price)
					.HasColumnType("decimal(18,2)");

				entity.Property(e => e.UpdatedAt)
					.HasColumnType("timestamp without time zone");

				entity.Property(e => e.CreatedAt)
					.HasColumnType("timestamp without time zone");

				entity.HasIndex(e => e.Deleted);

				// Define foreign key relationship
				entity.HasOne<User>()
					.WithMany()
					.HasForeignKey(e => e.AuthorId)
					.HasPrincipalKey(u => u.Id)
					.OnDelete(DeleteBehavior.Restrict);
			});
		}
	}
}
