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
        public virtual DbSet<Doctor> Doctors { get; set; }
        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<Messenger> Messages { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            UserAuthenConfiguration.Config(modelBuilder);
            UserProfileConfiguration.Config(modelBuilder);
            LearningCourseConfiguration.Config(modelBuilder);
            DoctorConfiguration.Config(modelBuilder);
            BookingConfiguration.Config(modelBuilder);
            MessageConfiguration.Config(modelBuilder);
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
                if (entry.State == EntityState.Added)
                {
                    ((CommonEntity)entry.Entity).CreatedAt = DateTime.UtcNow;
                }

                ((CommonEntity)entry.Entity).UpdatedAt = DateTime.UtcNow;
            }
        }
    }
}
