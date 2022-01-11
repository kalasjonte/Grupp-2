namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.CVs", "Clicks");
            DropColumn("dbo.Users", "Deactivate");
            DropColumn("dbo.Users", "GithubUsername");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "GithubUsername", c => c.String());
            AddColumn("dbo.Users", "Deactivate", c => c.Boolean(nullable: false));
            AddColumn("dbo.CVs", "Clicks", c => c.Int(nullable: false));
        }
    }
}
