using GISModel.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISCore.Business.Abstract
{
    public interface IFerramentasBusiness : IBaseBusiness<EntidadeBase>
    {

        List<string> BuscarSheetsExcel();
        
        List<string[]> CarregarDadosTabelasAuxiliaresFromExcel(string UsuarioLogado);

    }
}
