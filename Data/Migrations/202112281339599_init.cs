namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SchoolEducations", "School_SchoolID", "dbo.Schools");
            DropForeignKey("dbo.SchoolEducations", "Education_EduID", "dbo.Educations");
            DropForeignKey("dbo.Schools", "Type", "dbo.School_Type");
            DropIndex("dbo.Schools", new[] { "Type" });
            DropIndex("dbo.SchoolEducations", new[] { "School_SchoolID" });
            DropIndex("dbo.SchoolEducations", new[] { "Education_EduID" });
            DropTable("dbo.Schools");
            DropTable("dbo.School_Type");
            DropTable("dbo.SchoolEducations");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SchoolEducations",
                c => new
                    {
                        School_SchoolID = c.Int(nullable: false),
                        Education_EduID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.School_SchoolID, t.Education_EduID });
            
            CreateTable(
                "dbo.School_Type",
                c => new
                    {
                        School_TypeID = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                    })
                .PrimaryKey(t => t.School_TypeID);
            
            CreateTable(
                "dbo.Schools",
                c => new
                    {
                        SchoolID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Place = c.String(),
                        Type = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SchoolID);
            
            CreateIndex("dbo.SchoolEducations", "Education_EduID");
            CreateIndex("dbo.SchoolEducations", "School_SchoolID");
            CreateIndex("dbo.Schools", "Type");
            AddForeignKey("dbo.Schools", "Type", "dbo.School_Type", "School_TypeID", cascadeDelete: true);
            AddForeignKey("dbo.SchoolEducations", "Education_EduID", "dbo.Educations", "EduID", cascadeDelete: true);
            AddForeignKey("dbo.SchoolEducations", "School_SchoolID", "dbo.Schools", "SchoolID", cascadeDelete: true);
        }
    }
}
