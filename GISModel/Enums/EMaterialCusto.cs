using System.ComponentModel.DataAnnotations;

namespace GISModel.Enums
{
    public enum EMaterialCusto
    {

       //ATÉ R$ 3000,00

        //ATÉ R$ 3000,00

        [Display(Name = "001 - BAIXO")]
        BAIXO = 1,

        //DE R$ 3001,00 A R$ 5000,00
        [Display(Name = "002 - LEVE")]
        LEVE = 2,

        //DE R$ 5001,00 A R$ 10.000,00
        [Display(Name = "003 - MODERADO")]
        MODERADO = 3,

        //ACIMA DE R$ 10.000,00
        [Display(Name = "004 - ALTO")]
        ALTO = 4,


        [Display(Name = "005 - NÃO SE APLICA")]
        NAO_SE_APLICA = 5,

    }
}


        