using GISCore.Business.Abstract.Tabelas;
using GISModel.Entidades.OBJ.Tabelas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace GISCore.Business.Concrete.Tabelas
{
    public class AgenteAcidenteBusiness : BaseBusiness<AgenteAcidente>, IAgenteAcidenteBusiness
    {

        private readonly MemoryCache memoryCacheDefault = MemoryCache.Default;

        public List<AgenteAcidente> ListarTodos()
        {
            List<AgenteAcidente> lista = new List<AgenteAcidente>();

            var memoryCacheDefault = MemoryCache.Default;
            if (memoryCacheDefault.Contains("List<AgenteAcidente>"))
                return (List<AgenteAcidente>)memoryCacheDefault["List<AgenteAcidente>"];

            lista = Repository.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList();
            memoryCacheDefault.Add("List<AgenteAcidente>", lista, DateTime.Today.AddHours(96));

            return lista;
        }
    }
}
