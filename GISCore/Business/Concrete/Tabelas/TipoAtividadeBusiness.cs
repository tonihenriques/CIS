using GISCore.Business.Abstract.Tabelas;
using GISModel.Entidades.OBJ.Tabelas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace GISCore.Business.Concrete.Tabelas
{
    public class TipoAtividadeBusiness : BaseBusiness<TipoAtividade>, ITipoAtividadeBusiness
    {

        private readonly MemoryCache memoryCacheDefault = MemoryCache.Default;

        public List<TipoAtividade> ListarTodos()
        {
            List<TipoAtividade> lista = new List<TipoAtividade>();

            var memoryCacheDefault = MemoryCache.Default;
            if (memoryCacheDefault.Contains("List<TipoAtividade>"))
                return (List<TipoAtividade>)memoryCacheDefault["List<TipoAtividade>"];

            lista = Repository.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList();
            memoryCacheDefault.Add("List<TipoAtividade>", lista, DateTime.Today.AddHours(96));

            return lista;
        }

    }
}
