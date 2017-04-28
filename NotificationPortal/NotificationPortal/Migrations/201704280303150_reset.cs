namespace NotificationPortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reset : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Servers", "Description", c => c.String());
            DropColumn("dbo.Servers", "Discription");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Servers", "Discription", c => c.String());
            DropColumn("dbo.Servers", "Description");
        }
    }
}
