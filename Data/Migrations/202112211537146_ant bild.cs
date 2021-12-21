namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class antbild : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        ImageID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Path = c.String(),
                    })
                .PrimaryKey(t => t.ImageID);
            
            AddColumn("dbo.CVs", "ImageID", c => c.Int(nullable: false));
            CreateIndex("dbo.CVs", "ImageID");
            AddForeignKey("dbo.CVs", "ImageID", "dbo.Images", "ImageID", cascadeDelete: true);
            DropColumn("dbo.CVs", "ImgPath");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CVs", "ImgPath", c => c.String());
            DropForeignKey("dbo.CVs", "ImageID", "dbo.Images");
            DropIndex("dbo.CVs", new[] { "ImageID" });
            DropColumn("dbo.CVs", "ImageID");
            DropTable("dbo.Images");
        }
    }
}
