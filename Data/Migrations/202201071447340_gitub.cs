namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class gitub : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "GithubUsername", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "GithubUsername");
        }
    }
}
