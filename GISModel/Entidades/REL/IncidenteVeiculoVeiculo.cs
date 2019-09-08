using GISModel.Entidades.OBJ;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades.REL
{

    [Table("RELIncidenteVeiculoVeiculo")]
    public class IncidenteVeiculoVeiculo : EntidadeBase
    {

        [Required(ErrorMessage = "Informe o incidente com veículo")]
        public string UKIncidenteVeiculo { get; set; }

        public virtual IncidenteVeiculo IncidenteVeiculo { get; set; }


        [Required(ErrorMessage = "Informe um veículo")]
        public string UKVeiculo { get; set; }

        public virtual Veiculo Veiculo { get; set; }

    }
}
