namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class r1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OBJVeiculo",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        NPCondutor = c.String(),
                        NomeCondutor = c.String(),
                        TipoVeiculo = c.Int(nullable: false),
                        Zona = c.Int(nullable: false),
                        Natureza = c.Int(nullable: false),
                        Custo = c.Int(nullable: false),
                        Placa = c.String(),
                        TipoFrota = c.Int(nullable: false),
                        TipoCondutor = c.Int(nullable: false),
                        AcaoCondutor = c.Int(nullable: false),
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
            DropTable("dbo.OBJVeiculo");
        }
    }
}
