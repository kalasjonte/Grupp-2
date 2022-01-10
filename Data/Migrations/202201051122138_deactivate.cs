namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deactivate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Deactivate", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Deactivate");
        }
    }
}
