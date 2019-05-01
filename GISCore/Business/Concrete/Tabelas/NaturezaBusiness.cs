using GISCore.Business.Abstract.Tabelas;
using GISModel.Entidades.OBJ.Tabelas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace GISCore.Business.Concrete.Tabelas
{
    public class NaturezaBusiness : BaseBusiness<Natureza>, INaturezaBusiness
    {

        private readonly MemoryCache memoryCacheDefault = MemoryCache.Default;

        public List<Natureza> ListarTodos()
        {
            List<Natureza> lista = new List<Natureza>();

            var memoryCacheDefault = MemoryCache.Default;
            if (memoryCacheDefault.Contains("List<Natureza>"))
                return (List<Natureza>)memoryCacheDefault["List<Natureza>"];

            lista = Repository.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList();
            memoryCacheDefault.Add("List<Natureza>", lista, DateTime.Today.AddHours(96));

            return lista;
        }

    }
}
