using GISModel.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISCore.Business.Abstract
{
    public interface IBaseBusiness<T> where T : EntidadeBase
    {

        void Inserir(T entidade);

        void Alterar(T entidade);

        void Excluir(T entidade);

        IQueryable<T> Consulta { get; }


        void Terminar(T entidade);

        long GetNextNumber(string modulo, string query);

        string ExecuteQuery(string query);

        DataTable GetDataTable(string sqlQuery);

    }
}
