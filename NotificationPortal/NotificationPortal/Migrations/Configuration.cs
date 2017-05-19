namespace NotificationPortal.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using NotificationPortal.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    // This is where the database gets seeded with the table values, refer to Models/Key.cs to see the constant values
    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        private string adminEmail = "admin@portal.com";
        private string adminPassword = "password";
        private string adminFirstName = "Elliot";
        private string adminLastName = "Alderson";

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            RemoveAll(context);

            // Lookup tables
            SeedSendMethod(context);
            SeedNotificationType(context);
            SeedLevelOfImpact(context);
            SeedPriority(context);
            SeedDataCenterLocation(context);
            SeedGroups(context);
            SeedStatusType(context);
            SeedServerType(context);
            SeedRoles(context);
            SeedRoleDetail(context);
            SeedStatus(context);

            SeedAdmin(context);
        }

        private void SeedSendMethod(ApplicationDbContext context)
        {
            string[] sendMethods = new string[] {
                Key.SEND_METHOD_EMAIL,
                Key.SEND_METHOD_SMS,
                Key.SEND_METHOD_EMAIL_AND_SMS
            };
            foreach (string method in sendMethods)
            {
                context.SendMethod.Add(
                    new SendMethod()
                    {
                        SendMethodName = method
                    });
            }
            context.SaveChanges();
        }

        private void SeedNotificationType(ApplicationDbContext context)
        {
            string[] notificationTypes = new string[] {
                Key.NOTIFICATION_TYPE_INCIDENT,
                Key.NOTIFICATION_TYPE_MAINTENANCE
            };
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
            LevelOfImpact[] levelsOfImpact = new LevelOfImpact[] {
                new LevelOfImpact()
                    {
                        LevelName = Key.LEVEL_OF_IMPACT_IMPACTING,LevelValue=Key.LEVEL_OF_IMPACT_IMPACTING_VALUE
                },
                new LevelOfImpact()
                    {
                        LevelName = Key.LEVEL_OF_IMPACT_OUTAGE,LevelValue=Key.LEVEL_OF_IMPACT_OUTAGE_VALUE
                },
                new LevelOfImpact()
                    {
                        LevelName = Key.LEVEL_OF_IMPACT_REDUNDANCY,LevelValue=Key.LEVEL_OF_IMPACT_REDUNDANCY_VALUE
                },
                new LevelOfImpact()
                    {
                        LevelName = Key.LEVEL_OF_IMPACT_NON_IMPACTING,LevelValue=Key.LEVEL_OF_IMPACT_NON_IMPACTING_VALUE
                },

            };

            foreach (LevelOfImpact level in levelsOfImpact)
            {
                context.LevelOfImpact.Add(level);
            }
            context.SaveChanges();
        }

        private void SeedPriority(ApplicationDbContext context)
        {
            Priority[] priorities = new Priority[] {
                new Priority()
                    {
                        PriorityName = Key.PRIORITY_NAME_HIGH,PriorityValue=Key.PRIORITY_VALUE_HIGH
                },
                new Priority()
                    {
                        PriorityName = Key.PRIORITY_NAME_NORMAL,PriorityValue=Key.PRIORITY_VALUE_NORMAL
                },
                new Priority()
                    {
                        PriorityName = Key.PRIORITY_NAME_LOW,PriorityValue=Key.PRIORITY_VALUE_LOW
                }
            };

            foreach (Priority priority in priorities)
            {
                context.Priority.Add(priority);
            }
            context.SaveChanges();
        }

        private void SeedDataCenterLocation(ApplicationDbContext context)
        {
            string[] locations = new string[] {
                Key.DATA_CENTER_LOCATION_TORONTO,
                Key.DATA_CENTER_LOCATION_VANCOUVER
            };
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
            string[] groups = new string[] {
                Key.GROUP_INTERNAL,
                Key.GROUP_EXTERNAL
            };
            foreach (string group in groups)
            {
                context.Group.Add(
                    new Group()
                    {
                        GroupName = group,
                        GroupDescription = group == Key.GROUP_INTERNAL ?
                        Key.ROLE_ADMIN + "," + Key.ROLE_STAFF :
                        Key.ROLE_CLIENT + "," + Key.ROLE_USER
                    });
            }
            context.SaveChanges();
        }

        private void SeedRoles(ApplicationDbContext context)
        {
            var roleManager = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(context));
            string[] roles = new string[] {
                Key.ROLE_ADMIN,
                Key.ROLE_STAFF,
                Key.ROLE_CLIENT,
                Key.ROLE_USER
            };
            foreach (var role in roles)
            {
                var iRole = new ApplicationRole() { Name = role };
                roleManager.Create(iRole);
            }
        }

        private void SeedRoleDetail(ApplicationDbContext context)
        {
            var roleManager = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(context));
            var adminRole = roleManager.FindByName(Key.ROLE_ADMIN);
            var staffRole = roleManager.FindByName(Key.ROLE_STAFF);
            var clientRole = roleManager.FindByName(Key.ROLE_CLIENT);
            var userRole = roleManager.FindByName(Key.ROLE_USER);
            var groups = context.Group;
            var internalGroup = groups.Where(g => g.GroupName == Key.GROUP_INTERNAL).FirstOrDefault();
            var externalGroup = groups.Where(g => g.GroupName == Key.GROUP_EXTERNAL).FirstOrDefault();

            adminRole.RoleDetail = new RoleDetail()
            {
                RoleID = adminRole.Id,
                GroupID = internalGroup.GroupID,
                RoleDescription = Key.GROUP_INTERNAL + " " + Key.ROLE_ADMIN
            };

            staffRole.RoleDetail = new RoleDetail()
            {
                RoleID = staffRole.Id,
                GroupID = internalGroup.GroupID,
                RoleDescription = Key.GROUP_INTERNAL + " " + Key.ROLE_STAFF
            };

            clientRole.RoleDetail = new RoleDetail()
            {
                RoleID = clientRole.Id,
                GroupID = externalGroup.GroupID,
                RoleDescription = Key.GROUP_EXTERNAL + " " + Key.ROLE_CLIENT
            };

            userRole.RoleDetail = new RoleDetail()
            {
                RoleID = userRole.Id,
                GroupID = externalGroup.GroupID,
                RoleDescription = Key.GROUP_EXTERNAL + " " + Key.ROLE_USER
            };
            context.SaveChanges();
        }

        private void SeedStatusType(ApplicationDbContext context)
        {
            string[] statusTypes = new string[] {
                Key.STATUS_TYPE_APPLICATION,
                Key.STATUS_TYPE_CLIENT,
                Key.STATUS_TYPE_NOTIFICATION,
                Key.STATUS_TYPE_SERVER,
                Key.STATUS_TYPE_USER
            };
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

        private void SeedServerType(ApplicationDbContext context)
        {
            string[] serverTypes = new string[] {
                Key.SERVER_TYPE_APPLICATION,
                Key.SERVER_TYPE_DATABASE,
                Key.SERVER_TYPE_DIRECTORY
            };
            foreach (string serverType in serverTypes)
            {
                context.ServerType.Add(
                    new ServerType()
                    {
                        ServerTypeName = serverType
                    });
            }
            context.SaveChanges();
        }

        private void SeedStatus(ApplicationDbContext context)
        {
            // Application Status
            var statusTypeApplication = context.StatusType
                .Where(s => s.StatusTypeName == Key.STATUS_TYPE_APPLICATION)
                .FirstOrDefault();
            var status = new Status() { StatusName = Key.STATUS_APPLICATION_OFFLINE };
            status.StatusType = statusTypeApplication;
            context.Status.Add(status);
            status = new Status() { StatusName = Key.STATUS_APPLICATION_ONLINE };
            status.StatusType = statusTypeApplication;
            context.Status.Add(status);

            // Client Satatus
            var statusTypeClient = context.StatusType
                .Where(s => s.StatusTypeName == Key.STATUS_TYPE_CLIENT)
                .FirstOrDefault();
            status = new Status() { StatusName = Key.STATUS_CLIENT_DISABLED };
            status.StatusType = statusTypeClient;
            context.Status.Add(status);
            status = new Status() { StatusName = Key.STATUS_CLIENT_ENABLED };
            status.StatusType = statusTypeClient;
            context.Status.Add(status);

            // Notification Satatus
            var statusTypeNotification = context.StatusType
                .Where(s => s.StatusTypeName == Key.STATUS_TYPE_NOTIFICATION)
                .FirstOrDefault();
            string[] notificationStatuses = new string[] {
                Key.STATUS_NOTIFICATION_OPEN,
                Key.STATUS_NOTIFICATION_INVESTIGATING,
                Key.STATUS_NOTIFICATION_RESOLVED,
                Key.STATUS_NOTIFICATION_CLOSED
            };
            foreach (string snotificationSatus in notificationStatuses)
            {
                context.Status.Add(
                     new Status()
                     {
                         StatusName = snotificationSatus,
                         StatusType = statusTypeNotification
                     });
            }

            // Server Satatus
            var statusTypeServer = context.StatusType
                .Where(s => s.StatusTypeName == Key.STATUS_TYPE_SERVER)
                .FirstOrDefault();
            status = new Status() { StatusName = Key.STATUS_SERVER_OFFLINE };
            status.StatusType = statusTypeServer;
            context.Status.Add(status);
            status = new Status() { StatusName = Key.STATUS_SERVER_ONLINE };
            status.StatusType = statusTypeServer;
            context.Status.Add(status);

            // User Satatus
            var statusTypeUser = context.StatusType
                .Where(s => s.StatusTypeName == Key.STATUS_TYPE_USER)
                .FirstOrDefault();
            status = new Status() { StatusName = Key.STATUS_USER_DISABLED };
            status.StatusType = statusTypeUser;
            context.Status.Add(status);
            status = new Status() { StatusName = Key.STATUS_USER_ENABLED };
            status.StatusType = statusTypeUser;
            context.Status.Add(status);

            context.SaveChanges();
        }

        private void SeedAdmin(ApplicationDbContext context)
        {
            // Get user Statuses
            var userEnabledStatus = context.Status
                .Where(s => s.StatusType.StatusTypeName == Key.STATUS_TYPE_USER
                && s.StatusName == Key.STATUS_USER_ENABLED)
                .FirstOrDefault();

            // Get default send method
            var sendMethodEmail = context.SendMethod.Where(m => m.SendMethodName == Key.SEND_METHOD_EMAIL).FirstOrDefault();

            // Create a user
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var admin = new ApplicationUser()
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
            };
            admin.UserDetail = new UserDetail()
            {
                UserID = admin.Id,
                StatusID = userEnabledStatus.StatusID,
                FirstName = adminFirstName,
                LastName = adminLastName,
                BusinessPhone = null,
                MobilePhone = null,
                HomePhone = null,
                BusinessTitle = null,
                ReferenceID = Guid.NewGuid().ToString(),
                SendMethodID = sendMethodEmail.SendMethodID

            };

            userManager.Create(admin, adminPassword);

            userManager.AddToRoles(admin.Id, Key.ROLE_ADMIN);
        }

        private void RemoveAll(ApplicationDbContext context)
        {
            // delete bridges first
            context.Database.ExecuteSqlCommand("DELETE FROM NotificationApplications");
            context.Database.ExecuteSqlCommand("DELETE FROM ServerApplications");
            context.Database.ExecuteSqlCommand("DELETE FROM ServerNotifications");
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
            context.Database.ExecuteSqlCommand("DELETE FROM Priorities");
            context.Database.ExecuteSqlCommand("DELETE FROM ServerTypes");
        }
    }
}