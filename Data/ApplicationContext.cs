using DirectoryService.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DirectoryService.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Workplace> Workplaces { get; set; }
        public DbSet<ServiceCategory> ServiceCategories { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Queue> Queues { get; set; }
        public DbSet<QueueSettings> QueueSettings { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Facility>()
                .Property(f => f.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<Department>()
                .Property(d => d.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<Workplace>()
                .Property(w => w.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<ServiceCategory>()
                .Property(c => c.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<Service>()
                .Property(s => s.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<Schedule>()
                .Property(s => s.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<Schedule.NonWorkingPeriod>()
                .Property(np => np.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<Queue>()
                .Property(u => u.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<QueueSettings>()
                .Property(s => s.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<Department>()
                .HasOne(d => d.Facility)          
                .WithMany(f => f.Departments)   
                .HasForeignKey(d => d.FacilityId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Workplace>()
                .HasOne(w => w.Department)
                .WithMany(d => d.Workplaces)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Schedule.NonWorkingPeriod>()
                .HasOne(np => np.Schedule)
                .WithMany(s => s.NonWorkingPeriods)
                .HasForeignKey(np => np.ScheduleId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            modelBuilder.Entity<Workplace>()
                .HasMany(w => w.ServiceCategories)
                .WithMany(c => c.Workplaces)
                .UsingEntity<Dictionary<string, object>>(
                    "WorkplaceServiceCategory",
                    j => j.HasOne<ServiceCategory>().WithMany().HasForeignKey("ServiceCategoryId"),
                    j => j.HasOne<Workplace>().WithMany().HasForeignKey("WorkplaceId"),
                    j => j.HasKey("ServiceCategoryId", "WorkplaceId"));
            
            modelBuilder.Entity<ServiceCategory>()
                .HasMany(c => c.Departments)
                .WithMany(d => d.ServiceCategories)
                .UsingEntity<Dictionary<string, object>>(
                    "DepartmentServiceCategory",
                    j => j.HasOne<Department>().WithMany().HasForeignKey("DepartmentId"),
                    j => j.HasOne<ServiceCategory>().WithMany().HasForeignKey("ServiceCategoryId"),
                    j => j.HasKey("ServiceCategoryId", "DepartmentId"));

            modelBuilder.Entity<Queue>()
                .HasOne(q => q.QueueSettings)
                .WithOne(s => s.Queue)
                .HasForeignKey<QueueSettings>(s => s.QueueId);

            modelBuilder.Entity<Service>()
                .HasOne(s => s.ServiceCategory)
                .WithMany(c => c.Services)
                .HasForeignKey(s => s.ServiceCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Schedule>()
                .HasOne(s => s.Department)
                .WithMany(d => d.Schedules)
                .HasForeignKey(s => s.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Facility>()
                .Property(f => f.CreatedAt)
                .HasDefaultValueSql("now()")
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Department>()
                .Property(d => d.CreatedAt)
                .HasDefaultValueSql("now()")
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Workplace>()
                .Property(w => w.CreatedAt)
                .HasDefaultValueSql("now()")
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<ServiceCategory>()
                .Property(w => w.CreatedAt)
                .HasDefaultValueSql("now()")
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Service>()
                .Property(w => w.CreatedAt)
                .HasDefaultValueSql("now()")
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Schedule>()
                .Property(s => s.CreatedAt)
                .HasDefaultValueSql("now()")
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Queue>()
                .Property(q => q.CreatedAt)
                .HasDefaultValueSql("now()")
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<QueueSettings>()
                .Property(q => q.CreatedAt)
                .HasDefaultValueSql("now()")
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Facility>().HasQueryFilter(f => f.DeletedAt == null);
            modelBuilder.Entity<Department>().HasQueryFilter(d => d.DeletedAt == null);
            modelBuilder.Entity<Workplace>().HasQueryFilter(w => w.DeletedAt == null);
            modelBuilder.Entity<ServiceCategory>().HasQueryFilter(c => c.DeletedAt == null);
            modelBuilder.Entity<Service>().HasQueryFilter(s => s.DeletedAt == null);
            modelBuilder.Entity<Schedule>().HasQueryFilter(s => s.DeletedAt == null);
            modelBuilder.Entity<Queue>().HasQueryFilter(s => s.DeletedAt == null);
            modelBuilder.Entity<QueueSettings>().HasQueryFilter(s => s.DeletedAt == null);
        }
    }
}