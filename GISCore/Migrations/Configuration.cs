using GISModel.Entidades;
using System;
using System.Data.Entity.Migrations;

namespace GISCore.Migrations
{

    internal sealed class Configuration : DbMigrationsConfiguration<GISCore.Repository.Configuration.GISContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(GISCore.Repository.Configuration.GISContext context)
        {

            //context.Usuario.Add(new Usuario()
            //{
            //    ID = Guid.NewGuid().ToString(),
            //    UniqueKey = Guid.NewGuid().ToString(),
            //    Login = "E211041",
            //    Nome = "Johnnathan Rodrigo Pego de Almeida",
            //    DataInclusao = DateTime.Now,
            //    DataExclusao = DateTime.MaxValue,
            //    UsuarioInclusao = "SISTEMA",
            //    UsuarioExclusao = string.Empty,
            //    Email = "johnnathan.almeida@hexagon.com",
            //    Senha = "123",
            //    TipoDeAcesso = GISModel.Enums.TipoDeAcesso.Sistema,
            //    UKDepartamento = Guid.NewGuid().ToString(),
            //    UKEmpresa = Guid.NewGuid().ToString()
            //});

        }
    }
}
