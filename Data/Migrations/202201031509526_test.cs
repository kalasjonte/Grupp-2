namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Educations", "Title", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Skills", "Title", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Work_Experience", "Titel", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Work_Experience", "Titel", c => c.String());
            AlterColumn("dbo.Skills", "Title", c => c.String());
            AlterColumn("dbo.Educations", "Title", c => c.String());
        }
    }
}
