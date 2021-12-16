namespace Grupp_2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class losen : DbMigration
    {
        public override void Up()
        {
            AddColumn("Users", "Password", c => c.String(nullable: true, defaultValue: ""));
           // DropColumn("users", "password");
        }
        
        public override void Down()
        {
        }
    }
}
