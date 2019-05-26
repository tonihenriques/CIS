namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class r2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.OBJIncidente", "HouveLesao");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OBJIncidente", "HouveLesao", c => c.Boolean(nullable: false));
        }
    }
}
