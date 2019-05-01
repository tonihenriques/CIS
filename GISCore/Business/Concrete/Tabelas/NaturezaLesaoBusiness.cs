using GISCore.Business.Abstract.Tabelas;
using GISModel.Entidades.OBJ.Tabelas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace GISCore.Business.Concrete.Tabelas
{
    public class NaturezaLesaoBusiness : BaseBusiness<NaturezaLesao>, INaturezaLesaoBusiness
    {

        private readonly MemoryCache memoryCacheDefault = MemoryCache.Default;

        public List<NaturezaLesao> ListarTodos()
        {
            List<NaturezaLesao> lista = new List<NaturezaLesao>();

            var memoryCacheDefault = MemoryCache.Default;
            if (memoryCacheDefault.Contains("List<NaturezaLesao>"))
                return (List<NaturezaLesao>)memoryCacheDefault["List<NaturezaLesao>"];

            lista = Repository.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList();
            memoryCacheDefault.Add("List<NaturezaLesao>", lista, DateTime.Today.AddHours(96));

            return lista;
        }

    }
}
