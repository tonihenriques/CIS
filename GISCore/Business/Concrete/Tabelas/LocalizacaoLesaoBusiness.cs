using GISCore.Business.Abstract.Tabelas;
using GISModel.Entidades.OBJ.Tabelas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace GISCore.Business.Concrete.Tabelas
{
    public class LocalizacaoLesaoBusiness : BaseBusiness<LocalizacaoLesao>, ILocalizacaoLesaoBusiness
    {

        private readonly MemoryCache memoryCacheDefault = MemoryCache.Default;

        public List<LocalizacaoLesao> ListarTodos()
        {
            List<LocalizacaoLesao> lista = new List<LocalizacaoLesao>();

            var memoryCacheDefault = MemoryCache.Default;
            if (memoryCacheDefault.Contains("List<LocalizacaoLesao>"))
                return (List<LocalizacaoLesao>)memoryCacheDefault["List<LocalizacaoLesao>"];

            lista = Repository.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList();
            memoryCacheDefault.Add("List<LocalizacaoLesao>", lista, DateTime.Today.AddHours(96));

            return lista;
        }

    }
}
