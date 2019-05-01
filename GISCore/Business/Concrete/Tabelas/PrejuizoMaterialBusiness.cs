using GISCore.Business.Abstract.Tabelas;
using GISModel.Entidades.OBJ.Tabelas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace GISCore.Business.Concrete.Tabelas
{
    public class PrejuizoMaterialBusiness : BaseBusiness<PrejuizoMaterial>, IPrejuizoMaterialBusiness
    {

        private readonly MemoryCache memoryCacheDefault = MemoryCache.Default;

        public List<PrejuizoMaterial> ListarTodos()
        {
            List<PrejuizoMaterial> lista = new List<PrejuizoMaterial>();

            var memoryCacheDefault = MemoryCache.Default;
            if (memoryCacheDefault.Contains("List<PrejuizoMaterial>"))
                return (List<PrejuizoMaterial>)memoryCacheDefault["List<PrejuizoMaterial>"];

            lista = Repository.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList();
            memoryCacheDefault.Add("List<PrejuizoMaterial>", lista, DateTime.Today.AddHours(96));

            return lista;
        }

    }
}
