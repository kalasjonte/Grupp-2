namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CVs",
                c => new
                    {
                        CVID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        ImageID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CVID)
                .ForeignKey("dbo.Images", t => t.ImageID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID)
                .Index(t => t.ImageID);
            
            CreateTable(
                "dbo.Educations",
                c => new
                    {
                        EduID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.EduID);
            
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        ImageID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Path = c.String(),
                    })
                .PrimaryKey(t => t.ImageID);
            
            CreateTable(
                "dbo.Skills",
                c => new
                    {
                        SkillID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.SkillID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        Firstname = c.String(),
                        Lastname = c.String(),
                        Adress = c.String(),
                        Email = c.String(),
                        PrivateProfile = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UserID);
            
            CreateTable(
                "dbo.Projects_Users",
                c => new
                    {
                        UserID = c.Int(nullable: false),
                        ProjectID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserID, t.ProjectID })
                .ForeignKey("dbo.Projects", t => t.ProjectID)
                .ForeignKey("dbo.Users", t => t.UserID)
                .Index(t => t.UserID)
                .Index(t => t.ProjectID);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        ProjectID = c.Int(nullable: false, identity: true),
                        Titel = c.String(),
                        Description = c.String(),
                        Creator = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProjectID)
                .ForeignKey("dbo.Users", t => t.Creator, cascadeDelete: true)
                .Index(t => t.Creator);
            
            CreateTable(
                "dbo.Work_Experience",
                c => new
                    {
                        WorkExpID = c.Int(nullable: false, identity: true),
                        Titel = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.WorkExpID);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        MessageID = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                    })
                .PrimaryKey(t => t.MessageID);
            
            CreateTable(
                "dbo.User_Message",
                c => new
                    {
                        RecievingUser = c.Int(nullable: false),
                        MessageID = c.Int(nullable: false),
                        Read = c.Boolean(nullable: false),
                        Sender = c.String(),
                    })
                .PrimaryKey(t => new { t.RecievingUser, t.MessageID })
                .ForeignKey("dbo.Messages", t => t.MessageID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.RecievingUser, cascadeDelete: true)
                .Index(t => t.RecievingUser)
                .Index(t => t.MessageID);
            
            CreateTable(
                "dbo.EducationCVs",
                c => new
                    {
                        Education_EduID = c.Int(nullable: false),
                        CV_CVID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Education_EduID, t.CV_CVID })
                .ForeignKey("dbo.Educations", t => t.Education_EduID, cascadeDelete: true)
                .ForeignKey("dbo.CVs", t => t.CV_CVID, cascadeDelete: true)
                .Index(t => t.Education_EduID)
                .Index(t => t.CV_CVID);
            
            CreateTable(
                "dbo.SkillCVs",
                c => new
                    {
                        Skill_SkillID = c.Int(nullable: false),
                        CV_CVID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Skill_SkillID, t.CV_CVID })
                .ForeignKey("dbo.Skills", t => t.Skill_SkillID, cascadeDelete: true)
                .ForeignKey("dbo.CVs", t => t.CV_CVID, cascadeDelete: true)
                .Index(t => t.Skill_SkillID)
                .Index(t => t.CV_CVID);
            
            CreateTable(
                "dbo.Work_ExperienceCV",
                c => new
                    {
                        Work_Experience_WorkExpID = c.Int(nullable: false),
                        CV_CVID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Work_Experience_WorkExpID, t.CV_CVID })
                .ForeignKey("dbo.Work_Experience", t => t.Work_Experience_WorkExpID, cascadeDelete: true)
                .ForeignKey("dbo.CVs", t => t.CV_CVID, cascadeDelete: true)
                .Index(t => t.Work_Experience_WorkExpID)
                .Index(t => t.CV_CVID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.User_Message", "RecievingUser", "dbo.Users");
            DropForeignKey("dbo.User_Message", "MessageID", "dbo.Messages");
            DropForeignKey("dbo.Work_ExperienceCV", "CV_CVID", "dbo.CVs");
            DropForeignKey("dbo.Work_ExperienceCV", "Work_Experience_WorkExpID", "dbo.Work_Experience");
            DropForeignKey("dbo.CVs", "UserID", "dbo.Users");
            DropForeignKey("dbo.Projects_Users", "UserID", "dbo.Users");
            DropForeignKey("dbo.Projects_Users", "ProjectID", "dbo.Projects");
            DropForeignKey("dbo.Projects", "Creator", "dbo.Users");
            DropForeignKey("dbo.SkillCVs", "CV_CVID", "dbo.CVs");
            DropForeignKey("dbo.SkillCVs", "Skill_SkillID", "dbo.Skills");
            DropForeignKey("dbo.CVs", "ImageID", "dbo.Images");
            DropForeignKey("dbo.EducationCVs", "CV_CVID", "dbo.CVs");
            DropForeignKey("dbo.EducationCVs", "Education_EduID", "dbo.Educations");
            DropIndex("dbo.Work_ExperienceCV", new[] { "CV_CVID" });
            DropIndex("dbo.Work_ExperienceCV", new[] { "Work_Experience_WorkExpID" });
            DropIndex("dbo.SkillCVs", new[] { "CV_CVID" });
            DropIndex("dbo.SkillCVs", new[] { "Skill_SkillID" });
            DropIndex("dbo.EducationCVs", new[] { "CV_CVID" });
            DropIndex("dbo.EducationCVs", new[] { "Education_EduID" });
            DropIndex("dbo.User_Message", new[] { "MessageID" });
            DropIndex("dbo.User_Message", new[] { "RecievingUser" });
            DropIndex("dbo.Projects", new[] { "Creator" });
            DropIndex("dbo.Projects_Users", new[] { "ProjectID" });
            DropIndex("dbo.Projects_Users", new[] { "UserID" });
            DropIndex("dbo.CVs", new[] { "ImageID" });
            DropIndex("dbo.CVs", new[] { "UserID" });
            DropTable("dbo.Work_ExperienceCV");
            DropTable("dbo.SkillCVs");
            DropTable("dbo.EducationCVs");
            DropTable("dbo.User_Message");
            DropTable("dbo.Messages");
            DropTable("dbo.Work_Experience");
            DropTable("dbo.Projects");
            DropTable("dbo.Projects_Users");
            DropTable("dbo.Users");
            DropTable("dbo.Skills");
            DropTable("dbo.Images");
            DropTable("dbo.Educations");
            DropTable("dbo.CVs");
        }
    }
}
