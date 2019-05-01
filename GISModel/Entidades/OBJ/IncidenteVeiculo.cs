using GISModel.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades.OBJ
{

    [Table("OBJIncidenteVeiculo")]
    public class IncidenteVeiculo : EntidadeBase
    {

        [Display(Name = "Código")]
        public string Codigo { get; set; }

        [Display(Name = "Nº SMART")]
        public string NumeroSmart { get; set; }

        [Display(Name = "Acidente Fatal?")]
        public bool AcidenteFatal { get; set; }



        [Display(Name = "Centro (Empresa)")]
        public ECentroEmpresa Centro { get; set; }

        public string Status { get; set; }

        public string Responsavel { get; set; }

    }
}
