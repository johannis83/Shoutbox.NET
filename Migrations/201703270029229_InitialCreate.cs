namespace Shoutbox.NET.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Message",
                c => new
                    {
                        MessageID = c.Int(nullable: false, identity: true),
                        Timestamp = c.DateTime(),
                        Tag = c.String(),
                        Text = c.String(),
                        User_UserID = c.Int(),
                    })
                .PrimaryKey(t => t.MessageID)
                .ForeignKey("dbo.User", t => t.User_UserID)
                .Index(t => t.User_UserID);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Name = c.String(),
                        Domain = c.String(),
                        Settings = c.String(),
                        Division = c.String(),
                    })
                .PrimaryKey(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Message", "User_UserID", "dbo.User");
            DropIndex("dbo.Message", new[] { "User_UserID" });
            DropTable("dbo.User");
            DropTable("dbo.Message");
        }
    }
}
