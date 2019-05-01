using GISCore.Business.Abstract.Tabelas;
using GISModel.Entidades.OBJ.Tabelas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace GISCore.Business.Concrete.Tabelas
{
    public class FonteLesaoBusiness : BaseBusiness<FonteLesao>, IFonteLesaoBusiness
    {

        private readonly MemoryCache memoryCacheDefault = MemoryCache.Default;

        public List<FonteLesao> ListarTodos()
        {
            List<FonteLesao> lista = new List<FonteLesao>();

            var memoryCacheDefault = MemoryCache.Default;
            if (memoryCacheDefault.Contains("List<FonteLesao>"))
                return (List<FonteLesao>)memoryCacheDefault["List<FonteLesao>"];

            lista = Repository.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList();
            memoryCacheDefault.Add("List<FonteLesao>", lista, DateTime.Today.AddHours(96));

            return lista;
        }

    }
}
