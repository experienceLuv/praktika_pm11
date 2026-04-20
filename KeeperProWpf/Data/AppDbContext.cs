using KeeperProWpf.Data;
using KeeperProWpf.Models;
using KeeperProWpf.Office.Models;
using Microsoft.EntityFrameworkCore;

namespace KeeperProWpf.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<Department> Departments => Set<Department>();
        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<Visitor> Visitors => Set<Visitor>();
        public DbSet<ApplicationEntity> Applications => Set<ApplicationEntity>();
        public DbSet<ApplicationType> ApplicationTypes => Set<ApplicationType>();
        public DbSet<ApplicationStatus> ApplicationStatuses => Set<ApplicationStatus>();
        public DbSet<ApplicationVisitor> ApplicationVisitors => Set<ApplicationVisitor>();
        public DbSet<DocumentEntity> Documents => Set<DocumentEntity>();
        public DbSet<VisitLog> VisitLogs => Set<VisitLog>();
        public DbSet<BlackListEntry> BlackListEntries => Set<BlackListEntry>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(DbHelper.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(x => x.Role)
                .WithMany()
                .HasForeignKey(x => x.RoleId);

            modelBuilder.Entity<Employee>()
                .HasOne(x => x.Department)
                .WithMany()
                .HasForeignKey(x => x.DepartmentId);

            modelBuilder.Entity<ApplicationEntity>()
                .HasOne(x => x.Department)
                .WithMany()
                .HasForeignKey(x => x.DepartmentId);

            modelBuilder.Entity<ApplicationEntity>()
                .HasOne(x => x.Employee)
                .WithMany()
                .HasForeignKey(x => x.EmployeeId);

            modelBuilder.Entity<ApplicationEntity>()
                .HasOne(x => x.Status)
                .WithMany()
                .HasForeignKey(x => x.StatusId);

            modelBuilder.Entity<ApplicationEntity>()
                .HasOne(x => x.ApplicationType)
                .WithMany()
                .HasForeignKey(x => x.ApplicationTypeId);

            modelBuilder.Entity<VisitLog>()
                .HasOne(v => v.Application)
                .WithMany()
                .HasForeignKey(v => v.ApplicationId);

            modelBuilder.Entity<VisitLog>()
                .HasOne(v => v.Visitor)
                .WithMany()
                .HasForeignKey(v => v.VisitorId);
        }
    }
}