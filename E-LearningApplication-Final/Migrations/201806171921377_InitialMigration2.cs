namespace E_LearningApplication_Final.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration2 : DbMigration
    {
        public override void Up()
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
        
        public override void Down()
        {
        }
    }
}
