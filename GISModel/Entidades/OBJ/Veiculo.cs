using GISModel.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades.OBJ
{
    [Table("OBJVeiculo")]
    public class Veiculo : EntidadeBase
    {

        public string NPCondutor { get; set; }

        public string NomeCondutor { get; set; }

        [Required(ErrorMessage = "O perfil é obrigatório")]
        public TipoDeVeiculo TipoVeiculo { get; set; }

        public Zona Zona { get; set; }

        public NaturezaVeiculo Natureza { get; set; }

        public int Custo { get; set; }

        public string Placa { get; set; }

        public TipoFrota TipoFrota { get; set; }

        public TipoCondutor TipoCondutor { get; set; }

        public AcaoCondutor AcaoCondutor { get; set; }

    }
}
