using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace NotificationPortal.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<IdentityRole>().ToTable("AspNetRoles");

            modelBuilder.Entity<Application>()
                .HasRequired(s => s.Status)
                .WithMany(s => s.Applications)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Client>()
                .HasRequired(s => s.Status)
                .WithMany(s => s.Clients)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Notification>()
                .HasRequired(s => s.Status)
                .WithMany(s => s.Notifications)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<UserDetail>()
                .HasRequired(s => s.Status)
                .WithMany(s => s.UserDetails)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Server>()
                .HasRequired(s => s.Status)
                .WithMany(s => s.Servers)
                .WillCascadeOnDelete(false);
        }

        // Generate all the database tables 
        public DbSet<Application> Application { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<DataCenterLocation> DataCenterLocation { get; set; }
        public DbSet<Group> Group { get; set; }
        public DbSet<LevelOfImpact> LevelOfImpact { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<NotificationType> NotificationType { get; set; }
        public DbSet<Priority> Priority { get; set; }
        public DbSet<RoleDetail> RoleDetail { get; set; }
        public DbSet<SendMethod> SendMethod { get; set; }
        public DbSet<Server> Server { get; set; }
        public DbSet<ServerType> ServerType { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<StatusType> StatusType { get; set; }
        public DbSet<UserDetail> UserDetail { get; set; }
    }
}