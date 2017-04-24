namespace SharePointHelperBOT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FAQ",
                c => new
                    {
                        QuestionID = c.Int(nullable: false, identity: true),
                        Question = c.String(),
                        Answer = c.String(),
                        Classification = c.String(),
                    })
                .PrimaryKey(t => t.QuestionID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.FAQ");
        }
    }
}
