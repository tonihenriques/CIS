namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class t11 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OBJIncidenteVeiculo", "Acidente", c => c.Boolean(nullable: false));
            AddColumn("dbo.OBJIncidenteVeiculo", "AcidentePotencial", c => c.Boolean(nullable: false));
            AddColumn("dbo.OBJIncidenteVeiculo", "ETipoEntrada", c => c.Int(nullable: false));
            AddColumn("dbo.OBJIncidenteVeiculo", "Regional", c => c.Int(nullable: false));
            AddColumn("dbo.OBJIncidenteVeiculo", "UKDiretoria", c => c.String());
            AddColumn("dbo.OBJIncidenteVeiculo", "UKOrgao", c => c.String());
            AddColumn("dbo.OBJIncidenteVeiculo", "DataIncidente", c => c.DateTime(nullable: false, storeType: "date"));
            AddColumn("dbo.OBJIncidenteVeiculo", "HoraIncidente", c => c.String());
            AddColumn("dbo.OBJIncidenteVeiculo", "ETipoAcidente", c => c.Int(nullable: false));
            AddColumn("dbo.OBJIncidenteVeiculo", "LocalAcidente", c => c.String());
            AddColumn("dbo.OBJIncidenteVeiculo", "TipoLocalAcidente", c => c.Int(nullable: false));
            AddColumn("dbo.OBJIncidenteVeiculo", "Logradouro", c => c.String());
            AddColumn("dbo.OBJIncidenteVeiculo", "NumeroLogradouro", c => c.String());
            AddColumn("dbo.OBJIncidenteVeiculo", "CNPJLocalAcidente", c => c.String());
            AddColumn("dbo.OBJIncidenteVeiculo", "Descricao", c => c.String(maxLength: 250));
            AddColumn("dbo.OBJIncidenteVeiculo", "NumeroBoletimOcorrencia", c => c.String());
            AddColumn("dbo.OBJIncidenteVeiculo", "DataBoletimOcorrencia", c => c.DateTime(storeType: "date"));
            AddColumn("dbo.OBJIncidenteVeiculo", "StatusWF", c => c.String());
            AddColumn("dbo.OBJIncidenteVeiculo", "MensagemPasso", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.OBJIncidenteVeiculo", "MensagemPasso");
            DropColumn("dbo.OBJIncidenteVeiculo", "StatusWF");
            DropColumn("dbo.OBJIncidenteVeiculo", "DataBoletimOcorrencia");
            DropColumn("dbo.OBJIncidenteVeiculo", "NumeroBoletimOcorrencia");
            DropColumn("dbo.OBJIncidenteVeiculo", "Descricao");
            DropColumn("dbo.OBJIncidenteVeiculo", "CNPJLocalAcidente");
            DropColumn("dbo.OBJIncidenteVeiculo", "NumeroLogradouro");
            DropColumn("dbo.OBJIncidenteVeiculo", "Logradouro");
            DropColumn("dbo.OBJIncidenteVeiculo", "TipoLocalAcidente");
            DropColumn("dbo.OBJIncidenteVeiculo", "LocalAcidente");
            DropColumn("dbo.OBJIncidenteVeiculo", "ETipoAcidente");
            DropColumn("dbo.OBJIncidenteVeiculo", "HoraIncidente");
            DropColumn("dbo.OBJIncidenteVeiculo", "DataIncidente");
            DropColumn("dbo.OBJIncidenteVeiculo", "UKOrgao");
            DropColumn("dbo.OBJIncidenteVeiculo", "UKDiretoria");
            DropColumn("dbo.OBJIncidenteVeiculo", "Regional");
            DropColumn("dbo.OBJIncidenteVeiculo", "ETipoEntrada");
            DropColumn("dbo.OBJIncidenteVeiculo", "AcidentePotencial");
            DropColumn("dbo.OBJIncidenteVeiculo", "Acidente");
        }
    }
}
