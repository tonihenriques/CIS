using GISCore.Business.Abstract.Tabelas;
using GISModel.Entidades.OBJ.Tabelas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace GISCore.Business.Concrete.Tabelas
{
    public class EspecieAcidenteImpessoalBusiness : BaseBusiness<EspecieAcidenteImpessoal>, IEspecieAcidenteImpessoalBusiness
    {

        private readonly MemoryCache memoryCacheDefault = MemoryCache.Default;

        public List<EspecieAcidenteImpessoal> ListarTodos()
        {
            List<EspecieAcidenteImpessoal> lista = new List<EspecieAcidenteImpessoal>();

            var memoryCacheDefault = MemoryCache.Default;
            if (memoryCacheDefault.Contains("List<EspecieAcidenteImpessoal>"))
                return (List<EspecieAcidenteImpessoal>)memoryCacheDefault["List<EspecieAcidenteImpessoal>"];

            lista = Repository.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList();
            memoryCacheDefault.Add("List<EspecieAcidenteImpessoal>", lista, DateTime.Today.AddHours(96));

            return lista;
        }

    }
}
