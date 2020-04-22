using GISModel.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades.OBJ
{
    [Table("OBJMaterial")]
    public class Material : EntidadeBase
    {

        [Display(Name = "Incidente com Veículo")]
        public string UKIncidente { get; set; }


        [Display(Name = "Tipo Ocorrência")]
        public ETipoOcorrencia TipoOcorrencia { get; set; }

        [Display(Name = "Tipo Material")]
        public ETipoMaterial TipoMaterial { get; set; }

        [Display(Name = "Material Danificado")]
        public string MaterialDanificado { get; set; }

        [Display(Name = "Custo")]
        public string Custo { get; set; }

    }
}
