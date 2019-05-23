using GISModel.Entidades;
using GISModel.Entidades.OBJ;
using GISModel.Entidades.OBJ.Tabelas;
using GISModel.Entidades.REL;
using System.Data.Entity;

namespace GISCore.Repository.Configuration
{

    public partial class GISContext : DbContext
    {

        public GISContext() : base("GISConnectionString")
        {
            Database.SetInitializer<GISContext>(null);
        }

        public DbSet<Arquivo> Arquivo { get; set; }

        public DbSet<AtoInseguro> AtoInseguro { get; set; }

        public DbSet<AgenteAcidente> AgenteAcidente { get; set; }

        public DbSet<CAT> CAT { get; set; }

        public DbSet<CondicaoAmbientalInseg> CondicaoAmbientalInseg { get; set; }

        public DbSet<Departamento> Departamento { get; set; }

        public DbSet<EmpregadoContratado> EmpregadoContratado { get; set; }

        public DbSet<EmpregadoProprio> EmpregadoProprio { get; set; }

        public DbSet<Empresa> Empresa { get; set; }

        public DbSet<ESocial> ESocial { get; set; }

        public DbSet<EspecieAcidenteImpessoal> EspecieAcidenteImpessoal { get; set; }

        public DbSet<Estabelecimento> Estabelecimento { get; set; }

        public DbSet<Estado> Estado { get; set; }

        public DbSet<FatorPessoalInseguranca> FatorPessoalInseguranca { get; set; }

        public DbSet<FonteLesao> FonteLesao { get; set; }

        public DbSet<Fornecedor> Fornecedor { get; set; }

        public DbSet<FuncaoGrids> FuncaoGrids { get; set; }

        public DbSet<Incidente> Incidente { get; set; }

        public DbSet<IncidenteVeiculo> IncidenteVeiculo { get; set; }

        public DbSet<LocalizacaoLesao> LocalizacaoLesao { get; set; }

        public DbSet<Municipio> Municipio { get; set; }

        public DbSet<NaturezaLesao> NaturezaLesao { get; set; }

        public DbSet<Natureza> Natureza { get; set; }

        public DbSet<NivelHierarquico> NivelHierarquico { get; set; }
        
        public DbSet<Perfil> Perfil { get; set; }
        
        public DbSet<QuaseIncidente> QuaseIncidente { get; set; }

        public DbSet<QuaseIncidenteVeiculo> QuaseIncidenteVeiculo { get; set; }

        public DbSet<RegistroEmpregadoContratado> RegistroEmpregadoContratado { get; set; }

        public DbSet<RegistroEmpregadoProprio> RegistroEmpregadoProprio { get; set; }
        
        public DbSet<PrejuizoMaterial> PrejMaterial { get; set; }

        public DbSet<TipoAcidentePessoal> TipoAcidentePessoal { get; set; }

        public DbSet<TipoAtividade> TipoAtividade { get; set; }

        public DbSet<Usuario> Usuario { get; set; }

        public DbSet<UsuarioPerfil> UsuarioPerfil { get; set; }

        public DbSet<LesaoDoenca> LesaoDoenca { get; set; }

        public DbSet<Codificacao> Codificacao { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)

        {
        }
    }
}
