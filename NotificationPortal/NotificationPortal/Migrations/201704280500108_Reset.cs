namespace NotificationPortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Reset : DbMigration
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
                        Status_StatusID = c.Int(),
                    })
                .PrimaryKey(t => t.ApplicationID)
                .ForeignKey("dbo.Status", t => t.Status_StatusID)
                .ForeignKey("dbo.Clients", t => t.ClientID, cascadeDelete: true)
                .Index(t => t.ClientID)
                .Index(t => t.Status_StatusID);
            
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        ClientID = c.Int(nullable: false, identity: true),
                        ClientName = c.String(),
                        Status_StatusID = c.Int(),
                    })
                .PrimaryKey(t => t.ClientID)
                .ForeignKey("dbo.Status", t => t.Status_StatusID)
                .Index(t => t.Status_StatusID);
            
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
                        NotificationID = c.Int(nullable: false),
                        ReferenceID = c.String(nullable: false, maxLength: 128),
                        ThreadID = c.Int(nullable: false),
                        NotificaionHeading = c.String(),
                        NotificaionDescription = c.String(),
                        SentDateTime = c.DateTime(nullable: false),
                        StartDateTime = c.DateTime(nullable: false),
                        EndDateTime = c.DateTime(nullable: false),
                        Application_ApplicationID = c.Int(),
                        LevelOfImpact_LevelOfImpactID = c.Int(),
                        NotificationType_NotificationTypeID = c.Int(),
                        SendMethod_SendMethodID = c.Int(),
                        Status_StatusID = c.Int(),
                    })
                .PrimaryKey(t => new { t.NotificationID, t.ReferenceID })
                .ForeignKey("dbo.Applications", t => t.Application_ApplicationID)
                .ForeignKey("dbo.LevelOfImpacts", t => t.LevelOfImpact_LevelOfImpactID)
                .ForeignKey("dbo.NotificationTypes", t => t.NotificationType_NotificationTypeID)
                .ForeignKey("dbo.SendMethods", t => t.SendMethod_SendMethodID)
                .ForeignKey("dbo.Status", t => t.Status_StatusID)
                .Index(t => t.Application_ApplicationID)
                .Index(t => t.LevelOfImpact_LevelOfImpactID)
                .Index(t => t.NotificationType_NotificationTypeID)
                .Index(t => t.SendMethod_SendMethodID)
                .Index(t => t.Status_StatusID);
            
            CreateTable(
                "dbo.LevelOfImpacts",
                c => new
                    {
                        LevelOfImpactID = c.Int(nullable: false, identity: true),
                        Type = c.String(),
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
                        Discription = c.String(),
                        StatusID = c.Int(nullable: false),
                        LocationID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ServerID)
                .ForeignKey("dbo.DataCenterLocations", t => t.LocationID, cascadeDelete: true)
                .ForeignKey("dbo.Status", t => t.StatusID, cascadeDelete: true)
                .Index(t => t.StatusID)
                .Index(t => t.LocationID);
            
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
                    })
                .PrimaryKey(t => t.UserID)
                .ForeignKey("dbo.Clients", t => t.ClientID)
                .ForeignKey("dbo.Status", t => t.StatusID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID)
                .Index(t => t.UserID)
                .Index(t => t.ClientID)
                .Index(t => t.StatusID);
            
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
                        Notification_ReferenceID = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Server_ServerID, t.Notification_NotificationID, t.Notification_ReferenceID })
                .ForeignKey("dbo.Servers", t => t.Server_ServerID, cascadeDelete: true)
                .ForeignKey("dbo.Notifications", t => new { t.Notification_NotificationID, t.Notification_ReferenceID }, cascadeDelete: true)
                .Index(t => t.Server_ServerID)
                .Index(t => new { t.Notification_NotificationID, t.Notification_ReferenceID });
            
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
            DropForeignKey("dbo.Applications", "ClientID", "dbo.Clients");
            DropForeignKey("dbo.UserDetails", "UserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserDetails", "StatusID", "dbo.Status");
            DropForeignKey("dbo.UserDetails", "ClientID", "dbo.Clients");
            DropForeignKey("dbo.UserDetailApplications", "Application_ApplicationID", "dbo.Applications");
            DropForeignKey("dbo.UserDetailApplications", "UserDetail_UserID", "dbo.UserDetails");
            DropForeignKey("dbo.Status", "StatusTypeID", "dbo.StatusTypes");
            DropForeignKey("dbo.Notifications", "Status_StatusID", "dbo.Status");
            DropForeignKey("dbo.Servers", "StatusID", "dbo.Status");
            DropForeignKey("dbo.ServerNotifications", new[] { "Notification_NotificationID", "Notification_ReferenceID" }, "dbo.Notifications");
            DropForeignKey("dbo.ServerNotifications", "Server_ServerID", "dbo.Servers");
            DropForeignKey("dbo.Servers", "LocationID", "dbo.DataCenterLocations");
            DropForeignKey("dbo.ServerApplications", "Application_ApplicationID", "dbo.Applications");
            DropForeignKey("dbo.ServerApplications", "Server_ServerID", "dbo.Servers");
            DropForeignKey("dbo.Notifications", "SendMethod_SendMethodID", "dbo.SendMethods");
            DropForeignKey("dbo.Notifications", "NotificationType_NotificationTypeID", "dbo.NotificationTypes");
            DropForeignKey("dbo.Notifications", "LevelOfImpact_LevelOfImpactID", "dbo.LevelOfImpacts");
            DropForeignKey("dbo.Notifications", "Application_ApplicationID", "dbo.Applications");
            DropForeignKey("dbo.Clients", "Status_StatusID", "dbo.Status");
            DropForeignKey("dbo.Applications", "Status_StatusID", "dbo.Status");
            DropIndex("dbo.UserDetailApplications", new[] { "Application_ApplicationID" });
            DropIndex("dbo.UserDetailApplications", new[] { "UserDetail_UserID" });
            DropIndex("dbo.ServerNotifications", new[] { "Notification_NotificationID", "Notification_ReferenceID" });
            DropIndex("dbo.ServerNotifications", new[] { "Server_ServerID" });
            DropIndex("dbo.ServerApplications", new[] { "Application_ApplicationID" });
            DropIndex("dbo.ServerApplications", new[] { "Server_ServerID" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.RoleDetails", new[] { "GroupID" });
            DropIndex("dbo.RoleDetails", new[] { "RoleID" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.UserDetails", new[] { "StatusID" });
            DropIndex("dbo.UserDetails", new[] { "ClientID" });
            DropIndex("dbo.UserDetails", new[] { "UserID" });
            DropIndex("dbo.Servers", new[] { "LocationID" });
            DropIndex("dbo.Servers", new[] { "StatusID" });
            DropIndex("dbo.Notifications", new[] { "Status_StatusID" });
            DropIndex("dbo.Notifications", new[] { "SendMethod_SendMethodID" });
            DropIndex("dbo.Notifications", new[] { "NotificationType_NotificationTypeID" });
            DropIndex("dbo.Notifications", new[] { "LevelOfImpact_LevelOfImpactID" });
            DropIndex("dbo.Notifications", new[] { "Application_ApplicationID" });
            DropIndex("dbo.Status", new[] { "StatusTypeID" });
            DropIndex("dbo.Clients", new[] { "Status_StatusID" });
            DropIndex("dbo.Applications", new[] { "Status_StatusID" });
            DropIndex("dbo.Applications", new[] { "ClientID" });
            DropTable("dbo.UserDetailApplications");
            DropTable("dbo.ServerNotifications");
            DropTable("dbo.ServerApplications");
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
