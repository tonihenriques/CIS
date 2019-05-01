using GISModel.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades.OBJ
{

    [Table("OBJQuaseIncidente")]
    public class QuaseIncidente : EntidadeBase
    {

        [Display(Name = "Código")]
        public string Codigo { get; set; }

        [Display(Name = "Centro (Empresa)")]
        public ECentroEmpresa Centro { get; set; }

        public string Status { get; set; }

        public string Responsavel { get; set; }

    }
}
