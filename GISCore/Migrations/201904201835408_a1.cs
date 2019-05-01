namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class a1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.OBJEmpregadoContratado", "Nascimento", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.OBJEmpregadoContratado", "Nascimento", c => c.DateTime(nullable: false));
        }
    }
}
