using GISCore.Business.Abstract;
using GISModel.DTO.Departamento;
using GISModel.Entidades;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISCore.Business.Concrete
{
    public class DepartamentoBusiness : BaseBusiness<Departamento>, IDepartamentoBusiness
    {

        [Inject]
        public IEmpresaBusiness EmpresaBusiness { get; set; }

        [Inject]
        public IEmpresaBusiness ContratoBusiness { get; set; }

        public override void Inserir(Departamento pDepartamento)
        {
            
            if (Consulta.Any(u => u.Codigo.Equals(pDepartamento.Codigo.Trim()) && u.UKEmpresa.Equals(pDepartamento.UKEmpresa)))
                throw new InvalidOperationException("Não é possível inserir o departamento, pois já existe um departamento com este código para esta empresa.");

            if (Consulta.Any(u => u.Sigla.ToUpper().Equals(pDepartamento.Sigla.Trim().ToUpper()) && u.UKEmpresa.Equals(pDepartamento.UKEmpresa)))
                throw new InvalidOperationException("Não é possível inserir o departamento, pois já existe um departamento com esta sigla para esta empresa.");

            pDepartamento.UniqueKey = Guid.NewGuid().ToString();

            base.Inserir(pDepartamento);

        }


        public override void Alterar(Departamento departamento)
        {

            Departamento tempDepartamento = Consulta.FirstOrDefault(p => p.UniqueKey.Equals(departamento.UniqueKey));
            if (tempDepartamento == null)
            {
                throw new Exception("Não foi possível encontrar o departamento através do ID.");
            }
            else
            {
                tempDepartamento.DataExclusao = DateTime.Now;
                tempDepartamento.UsuarioExclusao = departamento.UsuarioExclusao;
                base.Alterar(tempDepartamento);

                departamento.UniqueKey = tempDepartamento.UniqueKey;
                departamento.UsuarioExclusao = string.Empty;
                base.Inserir(departamento);
            }

        }

    }
}
