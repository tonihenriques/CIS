namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class t2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.OBJIncidente", "DataBoletimOcorrencia", c => c.DateTime(storeType: "date"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.OBJIncidente", "DataBoletimOcorrencia", c => c.DateTime());
        }
    }
}
