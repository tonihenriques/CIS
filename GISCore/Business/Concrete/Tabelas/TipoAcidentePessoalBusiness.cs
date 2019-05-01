using GISCore.Business.Abstract.Tabelas;
using GISModel.Entidades.OBJ.Tabelas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace GISCore.Business.Concrete.Tabelas
{
    public class TipoAcidentePessoalBusiness : BaseBusiness<TipoAcidentePessoal>, ITipoAcidentePessoalBusiness
    {

        private readonly MemoryCache memoryCacheDefault = MemoryCache.Default;

        public List<TipoAcidentePessoal> ListarTodos()
        {
            List<TipoAcidentePessoal> lista = new List<TipoAcidentePessoal>();

            var memoryCacheDefault = MemoryCache.Default;
            if (memoryCacheDefault.Contains("List<TipoAcidentePessoal>"))
                return (List<TipoAcidentePessoal>)memoryCacheDefault["List<TipoAcidentePessoal>"];

            lista = Repository.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList();
            memoryCacheDefault.Add("List<TipoAcidentePessoal>", lista, DateTime.Today.AddHours(96));

            return lista;
        }

    }
}
