using System.ComponentModel.DataAnnotations;

namespace GISModel.Enums
{
    public enum ENivelTensao
    {

        [Display(Name = "001 - ATE 240 V ")]
        ATE_240_V = 1,

        [Display(Name = "002 - 380 E 440 V")]
        DE_380_A_440 = 2,

        [Display(Name = "003 - DE 2,3 A 13,8 KV")]
        DE_2_3_A_13_8_KV = 3,

        [Display(Name = "004 - 23,1 e 34,5 KV")]
        DE_23_1_A_34_5_KV = 4,

        [Display(Name = "005 - ACIMA DE 34,5 KV")]
        ACIMA_DE_34_5_KV = 5,

        [Display(Name = "006 - SEM TENSAO")]
        SEM_TENSAO = 6

    }
}
