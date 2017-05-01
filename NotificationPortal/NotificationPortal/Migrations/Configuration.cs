namespace NotificationPortal.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using NotificationPortal.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            RemoveAll(context);

            // Lockup tables
            SeedSendMethod(context);
            SeedNotificationType(context);
            SeedLevelOfImpact(context);
            SeedDataCenterLocation(context);
            SeedGroups(context);
            SeedStatusType(context);

            SeedRoles(context);
            SeedRoleDetail(context);
            SeedStatus(context);
            SeedClient(context);
            SeedUsers(context);
            SeedServer(context);
            SeedApplication(context);
            SeedNotification(context);
        }

        private void SeedSendMethod(ApplicationDbContext context)
        {
            string[] notificationTypes = new string[] { "Email", "SMS" };
            foreach (string type in notificationTypes)
            {
                context.SendMethod.Add(
                    new SendMethod()
                    {
                        SendMethodName = type
                    });
            }
            context.SaveChanges();
        }

        private void SeedNotificationType(ApplicationDbContext context)
        {
            string[] notificationTypes = new string[] { "Incident", "Maintenance" };
            foreach (string type in notificationTypes)
            {
                context.NotificationType.Add(
                    new NotificationType()
                    {
                        NotificationTypeName = type
                    });
            }
            context.SaveChanges();
        }

        private void SeedLevelOfImpact(ApplicationDbContext context)
        {
            string[] levelsOfImpact = new string[] { "Impacting", "Non-impacting", "Full service outage", "Loss of redundancy" };
            foreach (string level in levelsOfImpact)
            {
                context.LevelOfImpact.Add(
                    new LevelOfImpact()
                    {
                        Level = level
                    });
            }
            context.SaveChanges();
        }

        private void SeedDataCenterLocation(ApplicationDbContext context)
        {
            string[] locations = new string[] { "Toronto", "Vancouver" };
            foreach (string location in locations)
            {
                context.DataCenterLocation.Add(
                    new DataCenterLocation()
                    {
                        Location = location
                    });
            }
            context.SaveChanges();
        }

        private void SeedGroups(ApplicationDbContext context)
        {
            string[] groups = new string[] { "Internal", "External" };
            foreach (string group in groups)
            {
                context.Group.Add(
                    new Group()
                    {
                        GroupName = group,
                        GroupDescription = group == "Internal" ? "Admin, Staff" : "Client, User"
                    });
            }
            context.SaveChanges();
        }

        private void SeedRoles(ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            string[] roles = new string[] {
                "Admin","Staff", "Client", "User"};
            foreach (var role in roles)
            {
                var iRole = new IdentityRole() { Name = role };
                roleManager.Create(iRole);
            }
        }

        private void SeedRoleDetail(ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var adminRole = roleManager.FindByName("Admin");
            var staffRole = roleManager.FindByName("Staff");
            var clientRole = roleManager.FindByName("Client");
            var userRole = roleManager.FindByName("User");
            var groups = context.Group;
            var internalGroup = groups.Where(g => g.GroupName == "Internal").FirstOrDefault();
            var externalGroup = groups.Where(g => g.GroupName == "External").FirstOrDefault();

            context.RoleDetail.Add(new RoleDetail()
            {
                RoleID = adminRole.Id,
                GroupID = internalGroup.GroupID,
                RoleDescription = "Internal Administrator"
            });

            context.RoleDetail.Add(new RoleDetail()
            {
                RoleID = staffRole.Id,
                GroupID = internalGroup.GroupID,
                RoleDescription = "Internal Staff"
            });

            context.RoleDetail.Add(new RoleDetail()
            {
                RoleID = clientRole.Id,
                GroupID = externalGroup.GroupID,
                RoleDescription = "External Client Administrator"
            });

            context.RoleDetail.Add(new RoleDetail()
            {
                RoleID = userRole.Id,
                GroupID = externalGroup.GroupID,
                RoleDescription = "External User"
            });
            context.SaveChanges();
        }

        private void SeedStatusType(ApplicationDbContext context)
        {
            string[] statusTypes = new string[] {
                "Application","Client", "Notification", "Server", "User"};
            foreach (string statusType in statusTypes)
            {
                context.StatusType.Add(
                    new StatusType()
                    {
                        StatusTypeName = statusType
                    });
            }
            context.SaveChanges();
        }

        private void SeedStatus(ApplicationDbContext context)
        {
            // Application Status
            var statusTypeApplication = context.StatusType.Where(s => s.StatusTypeName == "Application").FirstOrDefault();
            var status = new Status() { StatusName = "Offline" };
            status.StatusType = statusTypeApplication;
            context.Status.Add(status);
            status = new Status() { StatusName = "Online" };
            status.StatusType = statusTypeApplication;
            context.Status.Add(status);

            // Client Satatus
            var statusTypeClient = context.StatusType.Where(s => s.StatusTypeName == "Client").FirstOrDefault();
            status = new Status() { StatusName = "Disabled" };
            status.StatusType = statusTypeClient;
            context.Status.Add(status);
            status = new Status() { StatusName = "Enabled" };
            status.StatusType = statusTypeClient;
            context.Status.Add(status);

            // Notification Satatus
            var statusTypeNotification = context.StatusType.Where(s => s.StatusTypeName == "Notification").FirstOrDefault();
            status = new Status() { StatusName = "Incomplete" };
            status.StatusType = statusTypeNotification;
            context.Status.Add(status);
            status = new Status() { StatusName = "Complete" };
            status.StatusType = statusTypeNotification;
            context.Status.Add(status);

            // Server Satatus
            var statusTypeServer = context.StatusType.Where(s => s.StatusTypeName == "Server").FirstOrDefault();
            status = new Status() { StatusName = "Offline" };
            status.StatusType = statusTypeServer;
            context.Status.Add(status);
            status = new Status() { StatusName = "Online" };
            status.StatusType = statusTypeServer;
            context.Status.Add(status);

            // User Satatus
            var statusTypeUser = context.StatusType.Where(s => s.StatusTypeName == "User").FirstOrDefault();
            status = new Status() { StatusName = "Disabled" };
            status.StatusType = statusTypeUser;
            context.Status.Add(status);
            status = new Status() { StatusName = "Enabled" };
            status.StatusType = statusTypeUser;
            context.Status.Add(status);

            context.SaveChanges();
        }

        private void SeedClient(ApplicationDbContext context)
        {
            var bcitStatus = context.Status.Where(s => s.StatusName == "Disabled").FirstOrDefault();
            var bcitClient = new Client()
            {
                ClientName = "BCIT"
            };
            bcitClient.Status = bcitStatus;
            context.Client.Add(bcitClient);
            context.SaveChanges();

            var ubcStatus = context.Status.Where(s => s.StatusName == "Enabled").FirstOrDefault();
            var ubcClient = new Client()
            {
                ClientName = "UBC"
            };

            ubcClient.Status = ubcStatus;
            context.Client.Add(ubcClient);
            context.SaveChanges();
        }

        private void SeedUsers(ApplicationDbContext context)
        {
            // Get first client id
            var clientID = context.Client.FirstOrDefault().ClientID;

            // Get user Statuses
            var userStatusTypes = context.StatusType.Where(t => t.StatusTypeName == "User").FirstOrDefault();
            var userStatuses = context.Status.Where(s => s.StatusTypeID == userStatusTypes.StatusTypeID);
            var userEnabledStatus = userStatuses.Where(s => s.StatusName == "Enabled").FirstOrDefault();
            var userDisabledStatus = userStatuses.Where(s => s.StatusName == "Disabled").FirstOrDefault();

            // Create a user on start
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var admin = new ApplicationUser()
            {
                UserName = "admin@portal.com",
                Email = "admin@portal.com",
                EmailConfirmed = true,
            };
            admin.UserDetail = new UserDetail()
            {
                UserID = admin.Id,
                StatusID = userEnabledStatus.StatusID,
                FirstName = "John",
                LastName = "White",
                BusinessPhone = "604-778-9909",
                MobilePhone = "778-990-2234",
                HomePhone = "604-773-9908",
                BusinessTitle = "Network Administrator",
                
            };

            var staff = new ApplicationUser()
            {
                UserName = "staff@portal.com",
                Email = "staff@portal.com",
                EmailConfirmed = true,
            };
            staff.UserDetail = new UserDetail()
            {
                UserID = staff.Id,
                StatusID = userDisabledStatus.StatusID,
                FirstName = "Amy",
                LastName = "Lang",
                BusinessPhone = "604-756-1239",
                MobilePhone = "778-230-2349",
                HomePhone = "604-433-9945",
                BusinessTitle = "Client Manager",
            };

            var client = new ApplicationUser()
            {
                UserName = "client@portal.com",
                Email = "client@portal.com",
                EmailConfirmed = true,
            };
            client.UserDetail = new UserDetail()
            {
                UserID = client.Id,
                StatusID = userDisabledStatus.StatusID,
                ClientID = clientID,
                FirstName = "Roland",
                LastName = "Tang",
                BusinessPhone = "604-223-2445",
                MobilePhone = "778-223-4456",
                HomePhone = "604-253-5567",
                BusinessTitle = "Technical Director",
            };

            var user = new ApplicationUser()
            {
                UserName = "user@portal.com",
                Email = "user@portal.com",
                EmailConfirmed = true,
            };
            user.UserDetail = new UserDetail()
            {
                UserID = user.Id,
                StatusID = userDisabledStatus.StatusID,
                FirstName = "Patrick",
                LastName = "McDonald",
                BusinessPhone = "604-447-2356",
                MobilePhone = "604-456-0090",
                HomePhone = "778-123-2445",
                BusinessTitle = "Project Manager",
            };

            userManager.Create(admin, "password");
            userManager.Create(staff, "password");
            userManager.Create(client, "password");
            userManager.Create(user, "password");

            userManager.AddToRoles(admin.Id, "Admin");
            userManager.AddToRoles(staff.Id, "Staff");
            userManager.AddToRoles(client.Id, "Client");
            userManager.AddToRoles(user.Id, "User");
        }

        private void SeedServer(ApplicationDbContext context)
        {
            // get locationId
            var location = context.DataCenterLocation.Where(l => l.Location == "Vancouver").FirstOrDefault();

            // Get server Status
            var serverStatusType = context.StatusType.Where(t => t.StatusTypeName == "Server").FirstOrDefault();
            var serverStatuses = context.Status.Where(s => s.StatusTypeID == serverStatusType.StatusTypeID);
            var serverOfflineStatus = serverStatuses.Where(s => s.StatusName == "Offline").FirstOrDefault();

            var server = new Server()
            {
                ServerName = "DNS Server",
                Description = "Server for Domain Name System",
                LocationID = location.LocationID,
                StatusID = serverOfflineStatus.StatusID
            };
            context.Server.Add(server);
            context.SaveChanges();
        }

        private void SeedApplication(ApplicationDbContext context)
        {
            // Get client userDetail
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var clientUser = userManager.FindByName("client@portal.com");
            var clientUserDetail = context.UserDetail.Where(u => u.UserID == clientUser.Id);

            // Get first client id
            var clientID = context.Client.FirstOrDefault().ClientID;

            // Get app Status
            var appStatusType = context.StatusType.Where(t => t.StatusTypeName == "Application").FirstOrDefault();
            var appStatuses = context.Status.Where(s => s.StatusTypeID == appStatusType.StatusTypeID);
            var appOfflineStatus = appStatuses.Where(s => s.StatusName == "Offline").FirstOrDefault();

            // Get server
            var server = context.Server;

            var app = new Application()
            {
                ApplicationName = "Notification Portal",
                Description = "Portal to manage all notifications",
                URL = "http://notification-portal.com/",
                ClientID = clientID,
                StatusID = appOfflineStatus.StatusID
            };
            app.UserDetail = clientUserDetail.ToList();
            app.Servers = server.ToList();
            context.Application.Add(app);
            context.SaveChanges();
        }

        private void SeedNotification(ApplicationDbContext context)
        {
            // Get app
            var app = context.Application.Where(a => a.ApplicationName == "Notification Portal").FirstOrDefault();
            // Get notification type
            var notificationType = context.NotificationType.Where(t => t.NotificationTypeName == "Maintenance").FirstOrDefault();
            // Get levelOfImpact
            var levelOfImpact = context.LevelOfImpact.Where(l => l.Level == "Non-impacting").FirstOrDefault();
            // Get server
            var server = context.Server.Where(s => s.ServerName == "DNS Server").FirstOrDefault();
            // Get server
            var sendMethod = context.SendMethod.Where(s => s.SendMethodName == "Email").FirstOrDefault();
            // Get status
            var statusType = context.StatusType.Where(t => t.StatusTypeName == "Notification").FirstOrDefault();
            var status = context.Status.Where(s => s.StatusTypeID == statusType.StatusTypeID && s.StatusName == "Incomplete").FirstOrDefault();

            var notification = new Notification()
            {
                StartDateTime = DateTime.Now.AddMinutes(5),
                EndDateTime = DateTime.Now.AddHours(1),
                SentDateTime = DateTime.Now,
                NotificationHeading = "Application offline",
                NotificationDescription = "Application will be offline for maintenance",
                ThreadID = 0,
                ReferenceID = "ref-0",
                ApplicationID = app.ApplicationID,
                NotificationTypeID = notificationType.NotificationTypeID,
                LevelOfImpactID = levelOfImpact.LevelOfImpactID,
                ServerID = server.ServerID,
                SendMethodID = sendMethod.SendMethodID,
                StatusID = status.StatusID
            };
            context.Notification.Add(notification);
            context.SaveChanges();
        }

        private void RemoveAll(ApplicationDbContext context)
        {
            // delete bridges first
            context.Database.ExecuteSqlCommand("DELETE FROM ServerApplications");
            context.Database.ExecuteSqlCommand("DELETE FROM UserDetailApplications");

            context.Database.ExecuteSqlCommand("DELETE FROM Notifications");
            context.Database.ExecuteSqlCommand("DELETE FROM Servers");
            context.Database.ExecuteSqlCommand("DELETE FROM Applications");

            // delete detail tables before AspNet tables
            context.Database.ExecuteSqlCommand("DELETE FROM UserDetails");
            context.Database.ExecuteSqlCommand("DELETE FROM RoleDetails");

            // delete AspNet tables after detail tables
            context.Database.ExecuteSqlCommand("DELETE FROM AspNetUsers");
            context.Database.ExecuteSqlCommand("DELETE FROM AspNetUserClaims");
            context.Database.ExecuteSqlCommand("DELETE FROM AspNetUserLogins");
            context.Database.ExecuteSqlCommand("DELETE FROM AspNetUserRoles");
            context.Database.ExecuteSqlCommand("DELETE FROM AspNetRoles");

            // delete after delete users
            context.Database.ExecuteSqlCommand("DELETE FROM Clients");

            // delete lookup tables
            context.Database.ExecuteSqlCommand("DELETE FROM Status");
            context.Database.ExecuteSqlCommand("DELETE FROM Groups");
            context.Database.ExecuteSqlCommand("DELETE FROM NotificationTypes");
            context.Database.ExecuteSqlCommand("DELETE FROM SendMethods");
            context.Database.ExecuteSqlCommand("DELETE FROM LevelOfImpacts");
            context.Database.ExecuteSqlCommand("DELETE FROM DataCenterLocations");
            context.Database.ExecuteSqlCommand("DELETE FROM StatusTypes");
        }
    }
}
