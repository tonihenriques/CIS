using GISCore.Business.Abstract.Tabelas;
using GISModel.Entidades.OBJ.Tabelas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace GISCore.Business.Concrete.Tabelas
{
    public class CondicaoAmbientalInsegBusiness : BaseBusiness<CondicaoAmbientalInseg>, ICondicaoAmbientalInsegBusiness
    {

        private readonly MemoryCache memoryCacheDefault = MemoryCache.Default;

        public List<CondicaoAmbientalInseg> ListarTodos()
        {
            List<CondicaoAmbientalInseg> lista = new List<CondicaoAmbientalInseg>();

            var memoryCacheDefault = MemoryCache.Default;
            if (memoryCacheDefault.Contains("List<CondicaoAmbientalInseg>"))
                return (List<CondicaoAmbientalInseg>)memoryCacheDefault["List<CondicaoAmbientalInseg>"];

            lista = Repository.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList();
            memoryCacheDefault.Add("List<CondicaoAmbientalInseg>", lista, DateTime.Today.AddHours(96));

            return lista;
        }

    }
}
