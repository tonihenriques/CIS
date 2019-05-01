using GISCore.Business.Abstract.Tabelas;
using GISModel.Entidades.OBJ.Tabelas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace GISCore.Business.Concrete.Tabelas
{
    public class AtoInseguroBusiness : BaseBusiness<AtoInseguro>, IAtoInseguroBusiness
    {
        private readonly MemoryCache memoryCacheDefault = MemoryCache.Default;

        public List<AtoInseguro> ListarTodos()
        {
            List<AtoInseguro> lista = new List<AtoInseguro>();

            var memoryCacheDefault = MemoryCache.Default;
            if (memoryCacheDefault.Contains("List<AtoInseguro>"))
                return (List<AtoInseguro>)memoryCacheDefault["List<AtoInseguro>"];

            lista = Repository.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList();
            memoryCacheDefault.Add("List<AtoInseguro>", lista, DateTime.Today.AddHours(96));

            return lista;
        }

    }
}
