namespace SharePointHelperBOT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_Entity_Columns : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FAQ", "Task", c => c.String());
            AddColumn("dbo.FAQ", "Subject", c => c.String());
            AddColumn("dbo.FAQ", "Platform", c => c.String());
            AddColumn("dbo.FAQ", "DataStructure", c => c.String());
            AddColumn("dbo.FAQ", "Location", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FAQ", "Location");
            DropColumn("dbo.FAQ", "DataStructure");
            DropColumn("dbo.FAQ", "Platform");
            DropColumn("dbo.FAQ", "Subject");
            DropColumn("dbo.FAQ", "Task");
        }
    }
}
