namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _05 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ObserverSettings",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        EnableNotifications = c.Boolean(nullable: false),
                        EnableSounds = c.Boolean(nullable: false),
                        AlertOnChangesOnly = c.Boolean(nullable: false),
                        PollingPeriod = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ObserverServers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Enabled = c.Boolean(nullable: false),
                        Name = c.String(maxLength: 4000),
                        Url = c.String(maxLength: 4000),
                        ObserverSettings_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ObserverSettings", t => t.ObserverSettings_Id)
                .Index(t => t.ObserverSettings_Id);
            
            CreateTable(
                "dbo.ObserverJobs",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Enabled = c.Boolean(nullable: false),
                        Name = c.String(maxLength: 4000),
                        Description = c.String(maxLength: 4000),
                        DisplayName = c.String(maxLength: 4000),
                        Status = c.Int(nullable: false),
                        InProgress = c.Boolean(nullable: false),
                        ObserverServer_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ObserverServers", t => t.ObserverServer_Id)
                .Index(t => t.ObserverServer_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ObserverServers", "ObserverSettings_Id", "dbo.ObserverSettings");
            DropForeignKey("dbo.ObserverJobs", "ObserverServer_Id", "dbo.ObserverServers");
            DropIndex("dbo.ObserverJobs", new[] { "ObserverServer_Id" });
            DropIndex("dbo.ObserverServers", new[] { "ObserverSettings_Id" });
            DropTable("dbo.ObserverJobs");
            DropTable("dbo.ObserverServers");
            DropTable("dbo.ObserverSettings");
        }
    }
}
