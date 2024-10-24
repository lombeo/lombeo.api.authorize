using Microsoft.EntityFrameworkCore;

namespace Lombeo.Api.Authorize.Infra.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string SubDescription { get; set; }
        public decimal Price { get; set; }
        public int PercentOff { get; set; }
        public decimal StudyTime { get; set; }
        public int NumberSection { get; set; }
        public string Introduction { get; set; }
        public string[] WhatWillYouLearn { get; set; }
        public string[] Skill { get; set; }
        public int ActivityId { get; set; }
    }

    public class Activity
    {
        public int Id { get; set; }
        public string ActivityTitle { get; set; }
        public int Duration { get; set; }
        public int Priority { get; set; }
        public int SectionPriority { get; set; }
        public int ActivityType { get; set; }
        public int ActivityStatus { get; set; }
        public bool Major { get; set; }
        public bool AllowPreview { get; set; }
        public int? SectionId { get; set; }
    }

    public class Section
    {
        public int Id { get; set; }
        public string SectionName { get; set; }
        public int SectionStatus { get; set; }
        public List<int> ActivitiesId { get; set; }
    }

    public static class CourseConfiguration
    {
        public static void Config(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>(entity =>
            {
                // Tên bảng
                entity.ToTable("Courses");

                // Khóa chính
                entity.HasKey(e => e.Id);

                // Thuộc tính
                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.ImageUrl)
                    .HasMaxLength(500);

                entity.Property(e => e.SubDescription)
                    .HasMaxLength(1000);

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.PercentOff)
                    .HasDefaultValue(0);

                entity.Property(e => e.StudyTime)
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.NumberSection)
                    .IsRequired();

                entity.Property(e => e.Introduction)
                    .HasMaxLength(2000);

                entity.Property(e => e.WhatWillYouLearn)
                    .HasColumnType("jsonb");

                entity.Property(e => e.Skill)
                    .HasColumnType("jsonb");

                // Khóa ngoại cho ActivityId (không cần thiết lập quan hệ)
                entity.Property(e => e.ActivityId)
                    .IsRequired();
            });
        }
    }

    public static class ActivityConfiguration
    {
        public static void Config(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Activity>(entity =>
            {
                // Tên bảng
                entity.ToTable("Activities");

                // Khóa chính
                entity.HasKey(e => e.Id);

                // Thuộc tính
                entity.Property(e => e.ActivityTitle)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Duration)
                    .IsRequired();

                entity.Property(e => e.Priority)
                    .IsRequired();

                entity.Property(e => e.SectionPriority)
                    .IsRequired();

                entity.Property(e => e.ActivityType)
                    .IsRequired();

                entity.Property(e => e.ActivityStatus)
                    .IsRequired();

                entity.Property(e => e.Major)
                    .IsRequired();

                entity.Property(e => e.AllowPreview)
                    .IsRequired();

                // Khóa ngoại SectionId (nullable, không cần thiết lập quan hệ)
                entity.Property(e => e.SectionId);
            });
        }
    }

    public static class SectionConfiguration
    {
        public static void Config(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Section>(entity =>
            {
                // Tên bảng
                entity.ToTable("Sections");

                // Khóa chính
                entity.HasKey(e => e.Id);

                // Thuộc tính
                entity.Property(e => e.SectionName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.SectionStatus)
                    .IsRequired();

                // Lưu trữ mảng `ActivitiesId` dưới dạng JSON
                entity.Property(e => e.ActivitiesId)
                    .HasColumnType("jsonb");
            });
        }
    }
}
