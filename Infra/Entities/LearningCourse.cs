using Lombeo.Api.Authorize.Infra.Enums;
using Microsoft.EntityFrameworkCore;

namespace Lombeo.Api.Authorize.Infra.Entities
{
	public class LearningCourse : CommonEntity
	{
        public int Id { get; set; }
        public string CourseName { get; set; }
        public string SubDescription { get; set; }
        public string Description { get; set; }
        public string CourseImage { get; set; }
        public string[] Skills {  get; set; }
		public string[] WhatYouWillLearn {  get; set; }
		public decimal Price { get; set; }
        public int PercentOff { get; set; }
        
    }

    public static class LearningCourseConfiguration
    {
        public static void Config(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LearningCourse>(entity =>
            {
                entity.ToTable("LearningCourses");

                // Khóa chính
                entity.HasKey(e => e.Id);

                // Cấu hình các thuộc tính
                entity.Property(e => e.CourseName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Description);

                entity.Property(e => e.SubDescription);

                entity.Property(e => e.PercentOff);

                entity.Property(e => e.CourseImage);

                entity.Property(e => e.Skills)
                    .IsRequired()
                    .HasConversion(
                        v => string.Join(',', v),
                        v => v.Split(',', StringSplitOptions.RemoveEmptyEntries));

                entity.Property(e => e.WhatYouWillLearn)
                    .IsRequired()
                    .HasConversion(
                        v => string.Join(',', v),
                        v => v.Split(',', StringSplitOptions.RemoveEmptyEntries));

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(18,2)");
            });
        }
    }

}
