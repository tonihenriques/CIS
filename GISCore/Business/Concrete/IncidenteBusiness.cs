using GISCore.Business.Abstract;
using GISModel.Entidades;
using System;
using System.Linq;

namespace GISCore.Business.Concrete
{
    public class IncidenteBusiness : BaseBusiness<Incidente>, IIncidenteBusiness
    {

        public override void Inserir(Incidente pRegistro)
        {
            
            base.Inserir(pRegistro);
        }

    }
}
