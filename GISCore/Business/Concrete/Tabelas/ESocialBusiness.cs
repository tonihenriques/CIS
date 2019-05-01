using GISCore.Business.Abstract.Tabelas;
using GISModel.Entidades.OBJ.Tabelas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace GISCore.Business.Concrete.Tabelas
{
    public class ESocialBusiness : BaseBusiness<ESocial>, IESocialBusiness
    {

        private readonly MemoryCache memoryCacheDefault = MemoryCache.Default;

        public List<ESocial> ListarTodos()
        {
            List<ESocial> lista = new List<ESocial>();

            var memoryCacheDefault = MemoryCache.Default;
            if (memoryCacheDefault.Contains("List<ESocial>"))
                return (List<ESocial>)memoryCacheDefault["List<ESocial>"];

            lista = Repository.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList();
            memoryCacheDefault.Add("List<ESocial>", lista, DateTime.Today.AddHours(96));

            return lista;
        }

    }
}
