namespace SharePointHelperBOT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_Entity_Column_Action : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FAQ", "Action", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FAQ", "Action");
        }
    }
}
