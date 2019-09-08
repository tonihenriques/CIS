using System.ComponentModel.DataAnnotations;

namespace GISModel.Enums
{
    public enum ECusto
    {
        //SEM AFASTAMENTO 
        [Display(Name = "001 - LEVE")]
        LEVE = 1,

        //AFASTAMENTO ATÉ 15 DIAS
        [Display(Name = "002 - GRAVE")]
        GRAVE = 2,

        //AFASTAMENTO ACIMA DE 15 DIAS
        [Display(Name = "003 - GRAVISSIMO")]
        GRAVISSIMO = 3,

        //CASO NAO SE ENQUADRE
        [Display(Name = "004 - NÃO SE APLICA")]
        NAO_SE_APLICA = 4

    }
}
