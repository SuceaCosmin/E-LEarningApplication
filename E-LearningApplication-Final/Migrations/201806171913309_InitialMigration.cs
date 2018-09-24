namespace E_LearningApplication_Final.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Courses", "Author_Id", "dbo.User");
            DropForeignKey("dbo.Questions", "CourseID_Id", "dbo.Courses");
            DropForeignKey("dbo.Questions", "RightAnswer_Id", "dbo.Choices");
            DropForeignKey("dbo.Subscriptions", "Course_Id", "dbo.Courses");
            DropForeignKey("dbo.Subscriptions", "Student_Id", "dbo.User");
            DropIndex("dbo.Courses", new[] { "Author_Id" });
            DropIndex("dbo.Questions", new[] { "CourseID_Id" });
            DropIndex("dbo.Questions", new[] { "RightAnswer_Id" });
            DropIndex("dbo.Subscriptions", new[] { "Course_Id" });
            DropIndex("dbo.Subscriptions", new[] { "Student_Id" });
            DropTable("dbo.Choices");
            DropTable("dbo.Courses");
            DropTable("dbo.Questions");
            DropTable("dbo.Subscriptions");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Subscriptions",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Course_Id = c.Int(),
                        Student_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Body = c.String(nullable: false),
                        AnswerA = c.String(nullable: false),
                        AnswerB = c.String(nullable: false),
                        AnswerC = c.String(nullable: false),
                        AnswerD = c.String(nullable: false),
                        CourseID_Id = c.Int(),
                        RightAnswer_Id = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        Description = c.String(nullable: false),
                        Content = c.Binary(nullable: false),
                        CoverPohoto = c.Binary(nullable: false),
                        Rate = c.String(),
                        Author_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Choices",
                c => new
                    {
                        Id = c.Byte(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.Subscriptions", "Student_Id");
            CreateIndex("dbo.Subscriptions", "Course_Id");
            CreateIndex("dbo.Questions", "RightAnswer_Id");
            CreateIndex("dbo.Questions", "CourseID_Id");
            CreateIndex("dbo.Courses", "Author_Id");
            AddForeignKey("dbo.Subscriptions", "Student_Id", "dbo.User", "Id");
            AddForeignKey("dbo.Subscriptions", "Course_Id", "dbo.Courses", "Id");
            AddForeignKey("dbo.Questions", "RightAnswer_Id", "dbo.Choices", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Questions", "CourseID_Id", "dbo.Courses", "Id");
            AddForeignKey("dbo.Courses", "Author_Id", "dbo.User", "Id");
        }
    }
}
