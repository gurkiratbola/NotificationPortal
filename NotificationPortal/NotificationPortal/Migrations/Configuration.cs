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
        private string sampleAdminEmail = "admin@portal.com";
        private string sampleClientEmail = "client@portal.com";
        private string sameplUserEmail = "user@portal.com";
        private string sampleApplicationName1 = "Bank App";
        private string sampleApplicationName2 = "Traffic App";
        private string sampleApplicationName3 = "Domain App";
        private string sampleServerName1 = "DNS Server";
        private string sampleServerName2 = "App Server";
        private string sampleServerName3 = "Web-App Server";

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
            SeedClient(context);
            SeedUsers(context);
            SeedServer(context);
            SeedApplication(context);
            SeedNotification(context);
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

        private void SeedClient(ApplicationDbContext context)
        {
            var bcitStatus = context.Status
                .Where(s => s.StatusType.StatusTypeName == Key.STATUS_TYPE_CLIENT
                && s.StatusName == Key.STATUS_CLIENT_DISABLED)
                .FirstOrDefault();
            var bcitClient = new Client()
            {
                ClientName = "BCIT",
                ReferenceID = Guid.NewGuid().ToString()
            };
            bcitClient.Status = bcitStatus;
            context.Client.Add(bcitClient);
            context.SaveChanges();

            var ubcStatus = context.Status
                .Where(s => s.StatusType.StatusTypeName == Key.STATUS_TYPE_CLIENT
                && s.StatusName == Key.STATUS_CLIENT_ENABLED)
                .FirstOrDefault();
            var ubcClient = new Client()
            {
                ClientName = "UBC",
                ReferenceID = Guid.NewGuid().ToString()
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
            var userEnabledStatus = context.Status
                .Where(s => s.StatusType.StatusTypeName == Key.STATUS_TYPE_USER
                && s.StatusName == Key.STATUS_USER_ENABLED)
                .FirstOrDefault();
            var userDisabledStatus = context.Status
                .Where(s => s.StatusType.StatusTypeName == Key.STATUS_TYPE_USER
                && s.StatusName == Key.STATUS_USER_DISABLED)
                .FirstOrDefault();

            // Get send method
            var sendMethodEmail = context.SendMethod.Where(m => m.SendMethodName == Key.SEND_METHOD_EMAIL).FirstOrDefault();

            // Create a user on start
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var admin = new ApplicationUser()
            {
                UserName = sampleAdminEmail,
                Email = sampleAdminEmail,
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
                ReferenceID = Guid.NewGuid().ToString(),
                SendMethodID = sendMethodEmail.SendMethodID

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
                ReferenceID = Guid.NewGuid().ToString(),
                SendMethodID = sendMethodEmail.SendMethodID
            };

            var client = new ApplicationUser()
            {
                UserName = sampleClientEmail,
                Email = sampleClientEmail,
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
                ReferenceID = Guid.NewGuid().ToString(),
                SendMethodID = sendMethodEmail.SendMethodID
            };

            var user = new ApplicationUser()
            {
                UserName = sameplUserEmail,
                Email = sameplUserEmail,
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
                ReferenceID = Guid.NewGuid().ToString(),
                SendMethodID = sendMethodEmail.SendMethodID
            };

            userManager.Create(admin, "password");
            userManager.Create(staff, "password");
            userManager.Create(client, "password");
            userManager.Create(user, "password");

            userManager.AddToRoles(admin.Id, Key.ROLE_ADMIN);
            userManager.AddToRoles(staff.Id, Key.ROLE_STAFF);
            userManager.AddToRoles(client.Id, Key.ROLE_CLIENT);
            userManager.AddToRoles(user.Id, Key.ROLE_USER);
        }

        private void SeedServer(ApplicationDbContext context)
        {
            // get locationId
            var location = context.DataCenterLocation
                .Where(l => l.Location == Key.DATA_CENTER_LOCATION_VANCOUVER)
                .FirstOrDefault();

            // Get server Status
            var serverOfflineStatus = context.Status
                .Where(s => s.StatusType.StatusTypeName == Key.STATUS_TYPE_SERVER
                && s.StatusName == Key.STATUS_SERVER_OFFLINE).FirstOrDefault();

            // Get server types
            var serverTypeApplication = context.ServerType
                .Where(t => t.ServerTypeName == Key.SERVER_TYPE_APPLICATION)
                .FirstOrDefault();
            var serverTypeDirectory = context.ServerType
                .Where(t => t.ServerTypeName == Key.SERVER_TYPE_DIRECTORY)
                .FirstOrDefault();
            var server = new Server()
            {
                ServerName = sampleServerName1,
                Description = "Server for Domains",
                LocationID = location.LocationID,
                StatusID = serverOfflineStatus.StatusID,
                ServerTypeID = serverTypeDirectory.ServerTypeID,
                ReferenceID = Guid.NewGuid().ToString()
            };
            context.Server.Add(server);

            server = new Server()
            {
                ServerName = sampleServerName2,
                Description = "Server for Applications",
                LocationID = location.LocationID,
                StatusID = serverOfflineStatus.StatusID,
                ServerTypeID = serverTypeApplication.ServerTypeID,
                ReferenceID = Guid.NewGuid().ToString()
            };
            context.Server.Add(server);

            server = new Server()
            {
                ServerName = sampleServerName3,
                Description = "Server for more Applications",
                LocationID = location.LocationID,
                StatusID = serverOfflineStatus.StatusID,
                ServerTypeID = serverTypeApplication.ServerTypeID,
                ReferenceID = Guid.NewGuid().ToString()
            };
            context.Server.Add(server);

            context.SaveChanges();
        }

        private void SeedApplication(ApplicationDbContext context)
        {
            // Get client userDetail
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var clientUser = userManager.FindByName(sampleClientEmail);
            var clientUserDetail = context.UserDetail.Where(u => u.UserID == clientUser.Id);

            // Get first client id
            var clientID = context.Client.FirstOrDefault().ClientID;

            // Get user userDetail
            var userUser = userManager.FindByName(sameplUserEmail);
            var userUserDetail = context.UserDetail.Where(u => u.UserID == userUser.Id);

            // Get app Status
            var appOfflineStatus = context.Status
                .Where(s => s.StatusType.StatusTypeName == Key.STATUS_TYPE_APPLICATION
                && s.StatusName == Key.STATUS_APPLICATION_OFFLINE).FirstOrDefault();
            var appOnlineStatus = context.Status
                .Where(s => s.StatusType.StatusTypeName == Key.STATUS_TYPE_APPLICATION
                && s.StatusName == Key.STATUS_APPLICATION_ONLINE).FirstOrDefault();

            // Get server
            var serverApplication = context.Server
                .Where(s => s.ServerType.ServerTypeName == Key.SERVER_TYPE_APPLICATION);
            var serverDirectory = context.Server
                .Where(s => s.ServerType.ServerTypeName == Key.SERVER_TYPE_DIRECTORY);

            var app = new Application()
            {
                ApplicationName = sampleApplicationName1,
                Description = "Portal to manage all your banking needs",
                URL = "http://bank-app.com/",
                ClientID = clientID,
                StatusID = appOfflineStatus.StatusID,
                ReferenceID = Guid.NewGuid().ToString()
            };
            app.UserDetails = clientUserDetail.ToList();
            app.Servers = serverApplication.ToList();
            context.Application.Add(app);

            app = new Application()
            {
                ApplicationName = sampleApplicationName2,
                Description = "App to handle all kinds of traffic",
                URL = "http://traffic-app.com/",
                ClientID = clientID,
                StatusID = appOfflineStatus.StatusID,
                ReferenceID = Guid.NewGuid().ToString()
            };
            app.UserDetails = clientUserDetail.ToList();
            app.UserDetails = userUserDetail.ToList();
            app.Servers = serverApplication.ToList();
            context.Application.Add(app);

            app = new Application()
            {
                ApplicationName = sampleApplicationName3,
                Description = "Get your new domains",
                URL = "http://domain-app.com/",
                ClientID = clientID,
                StatusID = appOnlineStatus.StatusID,
                ReferenceID = Guid.NewGuid().ToString()
            };
            app.UserDetails = clientUserDetail.ToList();
            app.Servers = serverDirectory.ToList();
            context.Application.Add(app);

            context.SaveChanges();
        }

        private void SeedNotification(ApplicationDbContext context)
        {
            // Get app
            var apps = context.Application
                .Where(a => a.ApplicationName == sampleApplicationName1);
            // Get notification type
            var notificationTypeMaintenance = context.NotificationType
                .Where(t => t.NotificationTypeName == Key.NOTIFICATION_TYPE_MAINTENANCE)
                .FirstOrDefault();
            var notificationTypeIncident = context.NotificationType
                .Where(t => t.NotificationTypeName == Key.NOTIFICATION_TYPE_INCIDENT)
                .FirstOrDefault();
            // Get levelOfImpact
            var levelOfImpactNonImpacting = context.LevelOfImpact
                .Where(l => l.LevelName == Key.LEVEL_OF_IMPACT_NON_IMPACTING)
                .FirstOrDefault();
            var levelOfImpactImpacting = context.LevelOfImpact
                .Where(l => l.LevelName == Key.LEVEL_OF_IMPACT_IMPACTING)
                .FirstOrDefault();
            // Get server
            var servers = context.Server.Where(s => s.ServerName == sampleServerName1);
            // Get status
            var statusOpen = context.Status
                .Where(s => s.StatusType.StatusTypeName == Key.STATUS_TYPE_NOTIFICATION
                && s.StatusName == Key.STATUS_NOTIFICATION_OPEN)
                .FirstOrDefault();
            var statusClosed = context.Status
                .Where(s => s.StatusType.StatusTypeName == Key.STATUS_TYPE_NOTIFICATION
                && s.StatusName == Key.STATUS_NOTIFICATION_CLOSED)
                .FirstOrDefault();
            // Get priority
            var priorityHigh = context.Priority
                .Where(p => p.PriorityName == Key.PRIORITY_NAME_HIGH)
                .FirstOrDefault();
            var priorityNormal = context.Priority
                .Where(p => p.PriorityName == Key.PRIORITY_NAME_NORMAL)
                .FirstOrDefault();
            var priorityLow = context.Priority
                .Where(p => p.PriorityName == Key.PRIORITY_NAME_LOW)
                .FirstOrDefault();
            // Get Creator
            var admin = context.Users.Where(u => u.Email == sampleAdminEmail).FirstOrDefault();

            string sampleThread1 = "MAI-4561483";
            var notification = new Notification()
            {
                StartDateTime = DateTime.Now.AddHours(-1),
                EndDateTime = DateTime.Now,
                SentDateTime = DateTime.Now.AddHours(-2),
                NotificationHeading = "Server offline",
                NotificationDescription = "Server will be offline for maintenance",
                NotificationTypeID = notificationTypeMaintenance.NotificationTypeID,
                LevelOfImpactID = levelOfImpactNonImpacting.LevelOfImpactID,
                StatusID = statusOpen.StatusID,
                IncidentNumber = sampleThread1,
                PriorityID = priorityHigh.PriorityID,
                ReferenceID = Guid.NewGuid().ToString(),
                UserID = admin.Id
            };
            notification.Servers = servers.ToList();
            context.Notification.Add(notification);

            notification = new Notification()
            {
                StartDateTime = DateTime.Now.AddHours(-1),
                EndDateTime = DateTime.Now,
                SentDateTime = DateTime.Now,
                NotificationHeading = "Server back online",
                NotificationDescription = "Maintenance complete",
                NotificationTypeID = notificationTypeMaintenance.NotificationTypeID,
                LevelOfImpactID = levelOfImpactNonImpacting.LevelOfImpactID,
                StatusID = statusClosed.StatusID,
                IncidentNumber = sampleThread1,
                PriorityID = priorityLow.PriorityID,
                ReferenceID = Guid.NewGuid().ToString(),
                UserID = admin.Id
            };
            notification.Servers = servers.ToList();
            context.Notification.Add(notification);

            notification = new Notification()
            {
                SentDateTime = DateTime.Now,
                NotificationHeading = "Crashed Application",
                NotificationDescription = "Application has crashed",
                NotificationTypeID = notificationTypeIncident.NotificationTypeID,
                LevelOfImpactID = levelOfImpactImpacting.LevelOfImpactID,
                StatusID = statusOpen.StatusID,
                IncidentNumber = "INC-6928376",
                PriorityID = priorityNormal.PriorityID,
                ReferenceID = Guid.NewGuid().ToString(),
                UserID = admin.Id
            };
            notification.Applications = apps.ToList();
            notification.Servers = servers.ToList();
            context.Notification.Add(notification);

            context.SaveChanges();
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
        }
    }
}