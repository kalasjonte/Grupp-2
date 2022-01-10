namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class clicks : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CVs", "Clicks", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CVs", "Clicks");
        }
    }
}
