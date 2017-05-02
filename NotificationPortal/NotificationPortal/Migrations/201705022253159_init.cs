namespace NotificationPortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Applications",
                c => new
                    {
                        ApplicationID = c.Int(nullable: false, identity: true),
                        ApplicationName = c.String(),
                        Description = c.String(),
                        URL = c.String(),
                        ClientID = c.Int(nullable: false),
                        StatusID = c.Int(nullable: false),
                        ReferenceID = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.ApplicationID)
                .ForeignKey("dbo.Clients", t => t.ClientID, cascadeDelete: true)
                .ForeignKey("dbo.Status", t => t.StatusID)
                .Index(t => t.ClientID)
                .Index(t => t.StatusID)
                .Index(t => t.ReferenceID, unique: true);
            
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        ClientID = c.Int(nullable: false, identity: true),
                        ClientName = c.String(),
                        StatusID = c.Int(nullable: false),
                        ReferenceID = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.ClientID)
                .ForeignKey("dbo.Status", t => t.StatusID)
                .Index(t => t.StatusID)
                .Index(t => t.ReferenceID, unique: true);
            
            CreateTable(
                "dbo.Status",
                c => new
                    {
                        StatusID = c.Int(nullable: false, identity: true),
                        StatusName = c.String(),
                        StatusTypeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.StatusID)
                .ForeignKey("dbo.StatusTypes", t => t.StatusTypeID, cascadeDelete: true)
                .Index(t => t.StatusTypeID);
            
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        NotificationID = c.Int(nullable: false, identity: true),
                        ThreadID = c.String(),
                        NotificationHeading = c.String(),
                        NotificationDescription = c.String(),
                        SentDateTime = c.DateTime(nullable: false),
                        StartDateTime = c.DateTime(nullable: false),
                        EndDateTime = c.DateTime(nullable: false),
                        StatusID = c.Int(nullable: false),
                        SendMethodID = c.Int(nullable: false),
                        LevelOfImpactID = c.Int(nullable: false),
                        NotificationTypeID = c.Int(nullable: false),
                        ReferenceID = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.NotificationID)
                .ForeignKey("dbo.LevelOfImpacts", t => t.LevelOfImpactID, cascadeDelete: true)
                .ForeignKey("dbo.NotificationTypes", t => t.NotificationTypeID, cascadeDelete: true)
                .ForeignKey("dbo.SendMethods", t => t.SendMethodID, cascadeDelete: true)
                .ForeignKey("dbo.Status", t => t.StatusID)
                .Index(t => t.StatusID)
                .Index(t => t.SendMethodID)
                .Index(t => t.LevelOfImpactID)
                .Index(t => t.NotificationTypeID)
                .Index(t => t.ReferenceID, unique: true);
            
            CreateTable(
                "dbo.LevelOfImpacts",
                c => new
                    {
                        LevelOfImpactID = c.Int(nullable: false, identity: true),
                        Level = c.String(),
                    })
                .PrimaryKey(t => t.LevelOfImpactID);
            
            CreateTable(
                "dbo.NotificationTypes",
                c => new
                    {
                        NotificationTypeID = c.Int(nullable: false, identity: true),
                        NotificationTypeName = c.String(),
                    })
                .PrimaryKey(t => t.NotificationTypeID);
            
            CreateTable(
                "dbo.SendMethods",
                c => new
                    {
                        SendMethodID = c.Int(nullable: false, identity: true),
                        SendMethodName = c.String(),
                    })
                .PrimaryKey(t => t.SendMethodID);
            
            CreateTable(
                "dbo.Servers",
                c => new
                    {
                        ServerID = c.Int(nullable: false, identity: true),
                        ServerName = c.String(),
                        Description = c.String(),
                        StatusID = c.Int(nullable: false),
                        LocationID = c.Int(nullable: false),
                        ReferenceID = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.ServerID)
                .ForeignKey("dbo.DataCenterLocations", t => t.LocationID, cascadeDelete: true)
                .ForeignKey("dbo.Status", t => t.StatusID)
                .Index(t => t.StatusID)
                .Index(t => t.LocationID)
                .Index(t => t.ReferenceID, unique: true);
            
            CreateTable(
                "dbo.DataCenterLocations",
                c => new
                    {
                        LocationID = c.Int(nullable: false, identity: true),
                        Location = c.String(),
                    })
                .PrimaryKey(t => t.LocationID);
            
            CreateTable(
                "dbo.StatusTypes",
                c => new
                    {
                        StatusTypeID = c.Int(nullable: false, identity: true),
                        StatusTypeName = c.String(),
                    })
                .PrimaryKey(t => t.StatusTypeID);
            
            CreateTable(
                "dbo.UserDetails",
                c => new
                    {
                        UserID = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                        BusinessTitle = c.String(),
                        BusinessPhone = c.String(),
                        MobilePhone = c.String(),
                        HomePhone = c.String(),
                        ClientID = c.Int(),
                        StatusID = c.Int(nullable: false),
                        ReferenceID = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.UserID)
                .ForeignKey("dbo.Clients", t => t.ClientID)
                .ForeignKey("dbo.Status", t => t.StatusID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID)
                .Index(t => t.UserID)
                .Index(t => t.ClientID)
                .Index(t => t.StatusID)
                .Index(t => t.ReferenceID, unique: true);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        GroupID = c.Int(nullable: false, identity: true),
                        GroupName = c.String(),
                        GroupDescription = c.String(),
                    })
                .PrimaryKey(t => t.GroupID);
            
            CreateTable(
                "dbo.RoleDetails",
                c => new
                    {
                        RoleID = c.String(nullable: false, maxLength: 128),
                        RoleDescription = c.String(),
                        GroupID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RoleID)
                .ForeignKey("dbo.Groups", t => t.GroupID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleID)
                .Index(t => t.RoleID)
                .Index(t => t.GroupID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.NotificationApplications",
                c => new
                    {
                        Notification_NotificationID = c.Int(nullable: false),
                        Application_ApplicationID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Notification_NotificationID, t.Application_ApplicationID })
                .ForeignKey("dbo.Notifications", t => t.Notification_NotificationID, cascadeDelete: true)
                .ForeignKey("dbo.Applications", t => t.Application_ApplicationID, cascadeDelete: true)
                .Index(t => t.Notification_NotificationID)
                .Index(t => t.Application_ApplicationID);
            
            CreateTable(
                "dbo.ServerApplications",
                c => new
                    {
                        Server_ServerID = c.Int(nullable: false),
                        Application_ApplicationID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Server_ServerID, t.Application_ApplicationID })
                .ForeignKey("dbo.Servers", t => t.Server_ServerID, cascadeDelete: true)
                .ForeignKey("dbo.Applications", t => t.Application_ApplicationID, cascadeDelete: true)
                .Index(t => t.Server_ServerID)
                .Index(t => t.Application_ApplicationID);
            
            CreateTable(
                "dbo.ServerNotifications",
                c => new
                    {
                        Server_ServerID = c.Int(nullable: false),
                        Notification_NotificationID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Server_ServerID, t.Notification_NotificationID })
                .ForeignKey("dbo.Servers", t => t.Server_ServerID, cascadeDelete: true)
                .ForeignKey("dbo.Notifications", t => t.Notification_NotificationID, cascadeDelete: true)
                .Index(t => t.Server_ServerID)
                .Index(t => t.Notification_NotificationID);
            
            CreateTable(
                "dbo.UserDetailApplications",
                c => new
                    {
                        UserDetail_UserID = c.String(nullable: false, maxLength: 128),
                        Application_ApplicationID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserDetail_UserID, t.Application_ApplicationID })
                .ForeignKey("dbo.UserDetails", t => t.UserDetail_UserID, cascadeDelete: true)
                .ForeignKey("dbo.Applications", t => t.Application_ApplicationID, cascadeDelete: true)
                .Index(t => t.UserDetail_UserID)
                .Index(t => t.Application_ApplicationID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RoleDetails", "RoleID", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.RoleDetails", "GroupID", "dbo.Groups");
            DropForeignKey("dbo.Applications", "StatusID", "dbo.Status");
            DropForeignKey("dbo.Applications", "ClientID", "dbo.Clients");
            DropForeignKey("dbo.Clients", "StatusID", "dbo.Status");
            DropForeignKey("dbo.UserDetails", "UserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserDetails", "StatusID", "dbo.Status");
            DropForeignKey("dbo.UserDetails", "ClientID", "dbo.Clients");
            DropForeignKey("dbo.UserDetailApplications", "Application_ApplicationID", "dbo.Applications");
            DropForeignKey("dbo.UserDetailApplications", "UserDetail_UserID", "dbo.UserDetails");
            DropForeignKey("dbo.Status", "StatusTypeID", "dbo.StatusTypes");
            DropForeignKey("dbo.Notifications", "StatusID", "dbo.Status");
            DropForeignKey("dbo.Servers", "StatusID", "dbo.Status");
            DropForeignKey("dbo.ServerNotifications", "Notification_NotificationID", "dbo.Notifications");
            DropForeignKey("dbo.ServerNotifications", "Server_ServerID", "dbo.Servers");
            DropForeignKey("dbo.Servers", "LocationID", "dbo.DataCenterLocations");
            DropForeignKey("dbo.ServerApplications", "Application_ApplicationID", "dbo.Applications");
            DropForeignKey("dbo.ServerApplications", "Server_ServerID", "dbo.Servers");
            DropForeignKey("dbo.Notifications", "SendMethodID", "dbo.SendMethods");
            DropForeignKey("dbo.Notifications", "NotificationTypeID", "dbo.NotificationTypes");
            DropForeignKey("dbo.Notifications", "LevelOfImpactID", "dbo.LevelOfImpacts");
            DropForeignKey("dbo.NotificationApplications", "Application_ApplicationID", "dbo.Applications");
            DropForeignKey("dbo.NotificationApplications", "Notification_NotificationID", "dbo.Notifications");
            DropIndex("dbo.UserDetailApplications", new[] { "Application_ApplicationID" });
            DropIndex("dbo.UserDetailApplications", new[] { "UserDetail_UserID" });
            DropIndex("dbo.ServerNotifications", new[] { "Notification_NotificationID" });
            DropIndex("dbo.ServerNotifications", new[] { "Server_ServerID" });
            DropIndex("dbo.ServerApplications", new[] { "Application_ApplicationID" });
            DropIndex("dbo.ServerApplications", new[] { "Server_ServerID" });
            DropIndex("dbo.NotificationApplications", new[] { "Application_ApplicationID" });
            DropIndex("dbo.NotificationApplications", new[] { "Notification_NotificationID" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.RoleDetails", new[] { "GroupID" });
            DropIndex("dbo.RoleDetails", new[] { "RoleID" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.UserDetails", new[] { "ReferenceID" });
            DropIndex("dbo.UserDetails", new[] { "StatusID" });
            DropIndex("dbo.UserDetails", new[] { "ClientID" });
            DropIndex("dbo.UserDetails", new[] { "UserID" });
            DropIndex("dbo.Servers", new[] { "ReferenceID" });
            DropIndex("dbo.Servers", new[] { "LocationID" });
            DropIndex("dbo.Servers", new[] { "StatusID" });
            DropIndex("dbo.Notifications", new[] { "ReferenceID" });
            DropIndex("dbo.Notifications", new[] { "NotificationTypeID" });
            DropIndex("dbo.Notifications", new[] { "LevelOfImpactID" });
            DropIndex("dbo.Notifications", new[] { "SendMethodID" });
            DropIndex("dbo.Notifications", new[] { "StatusID" });
            DropIndex("dbo.Status", new[] { "StatusTypeID" });
            DropIndex("dbo.Clients", new[] { "ReferenceID" });
            DropIndex("dbo.Clients", new[] { "StatusID" });
            DropIndex("dbo.Applications", new[] { "ReferenceID" });
            DropIndex("dbo.Applications", new[] { "StatusID" });
            DropIndex("dbo.Applications", new[] { "ClientID" });
            DropTable("dbo.UserDetailApplications");
            DropTable("dbo.ServerNotifications");
            DropTable("dbo.ServerApplications");
            DropTable("dbo.NotificationApplications");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.RoleDetails");
            DropTable("dbo.Groups");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.UserDetails");
            DropTable("dbo.StatusTypes");
            DropTable("dbo.DataCenterLocations");
            DropTable("dbo.Servers");
            DropTable("dbo.SendMethods");
            DropTable("dbo.NotificationTypes");
            DropTable("dbo.LevelOfImpacts");
            DropTable("dbo.Notifications");
            DropTable("dbo.Status");
            DropTable("dbo.Clients");
            DropTable("dbo.Applications");
        }
    }
}
