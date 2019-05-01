using GISCore.Business.Abstract.Tabelas;
using GISModel.Entidades.OBJ.Tabelas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace GISCore.Business.Concrete.Tabelas
{
    public class FuncaoGridsBusiness : BaseBusiness<FuncaoGrids>, IFuncaoGridsBusiness
    {

        private readonly MemoryCache memoryCacheDefault = MemoryCache.Default;

        public List<FuncaoGrids> ListarTodos()
        {
            List<FuncaoGrids> lista = new List<FuncaoGrids>();

            var memoryCacheDefault = MemoryCache.Default;
            if (memoryCacheDefault.Contains("List<FuncaoGrids>"))
                return (List<FuncaoGrids>)memoryCacheDefault["List<FuncaoGrids>"];

            lista = Repository.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList();
            memoryCacheDefault.Add("List<FuncaoGrids>", lista, DateTime.Today.AddHours(96));

            return lista;
        }

    }
}
