namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CVs", "Clicks", c => c.Int(nullable: false));
            AddColumn("dbo.Users", "Deactivate", c => c.Boolean(nullable: false));
            AddColumn("dbo.Users", "GithubUsername", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "GithubUsername");
            DropColumn("dbo.Users", "Deactivate");
            DropColumn("dbo.CVs", "Clicks");
        }
    }
}
