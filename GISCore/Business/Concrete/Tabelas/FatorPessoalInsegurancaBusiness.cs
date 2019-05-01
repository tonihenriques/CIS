using GISCore.Business.Abstract.Tabelas;
using GISModel.Entidades.OBJ.Tabelas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace GISCore.Business.Concrete.Tabelas
{
    public class FatorPessoalInsegurancaBusiness : BaseBusiness<FatorPessoalInseguranca>, IFatorPessoalInsegurancaBusiness
    {

        private readonly MemoryCache memoryCacheDefault = MemoryCache.Default;

        public List<FatorPessoalInseguranca> ListarTodos()
        {
            List<FatorPessoalInseguranca> lista = new List<FatorPessoalInseguranca>();

            var memoryCacheDefault = MemoryCache.Default;
            if (memoryCacheDefault.Contains("List<FatorPessoalInseguranca>"))
                return (List<FatorPessoalInseguranca>)memoryCacheDefault["List<FatorPessoalInseguranca>"];

            lista = Repository.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList();
            memoryCacheDefault.Add("List<FatorPessoalInseguranca>", lista, DateTime.Today.AddHours(96));

            return lista;
        }

    }
}
