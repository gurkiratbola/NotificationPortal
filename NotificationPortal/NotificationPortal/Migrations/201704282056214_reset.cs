namespace NotificationPortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reset : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notifications", "NotificationHeading", c => c.String());
            AddColumn("dbo.Notifications", "NotificationDescription", c => c.String());
            DropColumn("dbo.Notifications", "NotificaionHeading");
            DropColumn("dbo.Notifications", "NotificaionDescription");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Notifications", "NotificaionDescription", c => c.String());
            AddColumn("dbo.Notifications", "NotificaionHeading", c => c.String());
            DropColumn("dbo.Notifications", "NotificationDescription");
            DropColumn("dbo.Notifications", "NotificationHeading");
        }
    }
}
