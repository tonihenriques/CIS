namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class t21 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OBJIncidenteVeiculo", "UKMunicipio", c => c.String());
            AddColumn("dbo.OBJIncidenteVeiculo", "Estado", c => c.String());
            AddColumn("dbo.OBJIncidenteVeiculo", "UKESocial", c => c.String());
            AddColumn("dbo.OBJIncidenteVeiculo", "AcidenteGraveIP102", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OBJIncidenteVeiculo", "AcidenteGraveIP102");
            DropColumn("dbo.OBJIncidenteVeiculo", "UKESocial");
            DropColumn("dbo.OBJIncidenteVeiculo", "Estado");
            DropColumn("dbo.OBJIncidenteVeiculo", "UKMunicipio");
        }
    }
}
