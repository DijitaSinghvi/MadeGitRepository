namespace CollegeWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nullcourse : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.User", "CourseId", "dbo.Course");
            DropIndex("dbo.User", new[] { "CourseId" });
            AlterColumn("dbo.User", "CourseId", c => c.Int());
            CreateIndex("dbo.User", "CourseId");
            AddForeignKey("dbo.User", "CourseId", "dbo.Course", "CourseId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.User", "CourseId", "dbo.Course");
            DropIndex("dbo.User", new[] { "CourseId" });
            AlterColumn("dbo.User", "CourseId", c => c.Int(nullable: false));
            CreateIndex("dbo.User", "CourseId");
            AddForeignKey("dbo.User", "CourseId", "dbo.Course", "CourseId", cascadeDelete: true);
        }
    }
}
