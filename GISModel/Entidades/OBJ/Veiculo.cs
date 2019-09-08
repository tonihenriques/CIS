using GISModel.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades.OBJ
{
    [Table("OBJVeiculo")]
    public class Veiculo : EntidadeBase
    {

        public TipoDeVeiculo TipoVeiculo { get; set; }
        
        public string Placa { get; set; }

        public TipoFrota TipoFrota { get; set; }

    }
}
