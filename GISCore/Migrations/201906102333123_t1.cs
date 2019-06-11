namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class t1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.OBJIncidente", "DataIncidente", c => c.DateTime(nullable: false, storeType: "date"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.OBJIncidente", "DataIncidente", c => c.DateTime(nullable: false));
        }
    }
}
