namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class a1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OBJEstado",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        Codigo = c.String(),
                        Descricao = c.String(),
                        NomeCompleto = c.String(),
                        UniqueKey = c.String(),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.OBJEstado");
        }
    }
}
