using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Lombeo.Api.Authorize.Infra.Entities;

namespace Lombeo.Api.Authorize.Infra
{
    public partial class LombeoAuthorizeContext : DbContext
    {
        public LombeoAuthorizeContext()
        {
        }

        public LombeoAuthorizeContext(DbContextOptions<LombeoAuthorizeContext> options)
             : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserProfile> UserProfiles { get; set; }
        public virtual DbSet<LearningCourse> LearningCourses { get; set; }
        public virtual DbSet<CourseWeek> CourseWeeks { get; set; }
        public virtual DbSet<CourseChapter> CourseChapters { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<Score> Scores { get; set; }
        public virtual DbSet<Content> Contents { get; set; }
        public virtual DbSet<UserCourses> UserCourses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            UserAuthenConfiguration.Config(modelBuilder);
            UserProfileConfiguration.Config(modelBuilder);
            LearningCourseConfiguration.Config(modelBuilder);
            CourseWeekConfiguration.Config(modelBuilder);
            CourseChapterConfiguration.Config(modelBuilder);
            QuestionConfiguration.Config(modelBuilder);
            AnswerConfiguration.Config(modelBuilder);
            ScoreConfiguration.Config(modelBuilder);
            UserCoursesConfiguration.Config(modelBuilder);
            //OnModelCreatingPartial(modelBuilder);
        }

        //partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
                CancellationToken cancellationToken = new CancellationToken())
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected virtual void OnBeforeSaving()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry != null && entry.Entity is CommonEntity commonEntity)
                {
                    if (entry.State == EntityState.Added)
                    {
                        ((CommonEntity)entry.Entity).CreatedAt = DateTime.UtcNow;
                    }

                    ((CommonEntity)entry.Entity).UpdatedAt = DateTime.UtcNow;
                }

            }
        }
    }
}