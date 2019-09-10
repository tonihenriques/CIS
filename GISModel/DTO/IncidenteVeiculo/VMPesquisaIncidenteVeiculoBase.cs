using GISModel.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.DTO.IncidenteVeiculo
{
    public class VMPesquisaIncidenteVeiculoBase
    {

        [Display(Name = "Código")]

        public string Codigo { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        [Display(Name = "Menssagem")]
        public string MensagemPasso { get; set; }

        [Display(Name = "Nº Smart")]
        public string NumeroSmart { get; set; }

        [Display(Name = "Tipo de Acidente")]
        public ETipoAcidente ETipoAcidente { get; set; }

        [Display(Name = "Orgão")]
        public string Orgao { get; set; }

        [Display(Name = "Diretoria")]
        public string Diretoria { get; set; }

        [Display(Name = "Data do Acidente")]
        public string DataIncidente { get; set; }

        [Display(Name = "Hora do Acidente")]
        public string HoraIncidente { get; set; }

        [Display(Name = "Municipio")]
        public string Municipio { get; set; }

        [Display(Name = "Estado")]
        public string Estado { get; set; }

        [Display(Name = "eSocial")]
        public string ESocial { get; set; }

        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Display(Name = "Local do Acidente")]
        public string LocalAcidente { get; set; }

        [Display(Name = "Logradouro")]
        public string Logradouro { get; set; }

        [Display(Name = "Numero do Logradouro")]
        public string NumeroLogradouro { get; set; }

        [Display(Name = "Local do Acidente")]
        public ETipoAcidente TipoLocalAcidente { get; set; }

        [Display(Name = "Ac. Grave conforme IP")]
        public string AcidenteGraveIP102 { get; set; }

        [Display(Name = "Acidente Fatal?")]
        public string AcidenteFatal { get; set; }

        [Display(Name = "Tipo de Entrada")]
        public string TipoEntrada { get; set; }

        [Display(Name = "Centro")]
        public ECentroEmpresa Centro { get; set; }

        [Display(Name = "Regional")]
        public Regional Regional { get; set; }


        public string UsuarioInclusao { get; set; }

        public string DataInclusao { get; set; }

        public string DataAtualizacao { get; set; }




    }
}
