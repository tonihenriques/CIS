namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class r1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OBJIncidente", "HouveLesao", c => c.Boolean(nullable: false));
            AlterColumn("dbo.OBJEmpregadoContratado", "CPF", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.OBJEmpregadoContratado", "CPF", c => c.String());
            DropColumn("dbo.OBJIncidente", "HouveLesao");
        }
    }
}
