namespace NotificationPortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ServerTypes",
                c => new
                    {
                        ServerTypeID = c.Int(nullable: false, identity: true),
                        ServerTypeName = c.String(),
                    })
                .PrimaryKey(t => t.ServerTypeID);
            
            AddColumn("dbo.Servers", "ServerTypeID", c => c.Int(nullable: false));
            AlterColumn("dbo.Notifications", "StartDateTime", c => c.DateTime());
            AlterColumn("dbo.Notifications", "EndDateTime", c => c.DateTime());
            CreateIndex("dbo.Servers", "ServerTypeID");
            AddForeignKey("dbo.Servers", "ServerTypeID", "dbo.ServerTypes", "ServerTypeID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Servers", "ServerTypeID", "dbo.ServerTypes");
            DropIndex("dbo.Servers", new[] { "ServerTypeID" });
            AlterColumn("dbo.Notifications", "EndDateTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Notifications", "StartDateTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.Servers", "ServerTypeID");
            DropTable("dbo.ServerTypes");
        }
    }
}
