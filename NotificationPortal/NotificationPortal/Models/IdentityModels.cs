using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace NotificationPortal.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        public virtual UserDetail UserDetail { get; set; }
    }
    
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

        public DbSet<Application> Application { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<DataCenterLocation> DataCenterLocation { get; set; }
        public DbSet<Group> Group { get; set; }
        public DbSet<LevelOfImpact> LevelOfImpact { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<NotificationType> NotificationType { get; set; }
        public DbSet<RoleDetail> RoleDetail { get; set; }
        public DbSet<SendMethod> SendMethod { get; set; }
        public DbSet<Server> Server { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<StatusType> StatusType { get; set; }
        public DbSet<UserDetail> UserDetail { get; set; }
    }
}