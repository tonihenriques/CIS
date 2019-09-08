using System.ComponentModel.DataAnnotations;

namespace GISModel.Enums
{
    public enum EPopulacaoLesao
    {


        [Display(Name = "001 - MORTE ")]
        MORTE = 1,

        [Display(Name = "002 - QUEIMADURAS DE 1. GRAU")]
        QUEIMADURAS_DE_1_GRAU = 2,

        [Display(Name = "003 - QUEIMADURAS DE 2. GRAU")]
        QUEIMADURAS_DE_2_GRAU = 3,

        [Display(Name = "004 - QUEIMADURAS DE 3. GRAU")]
        QUEIMADURAS_DE_3_GRAU = 4,

        [Display(Name = "005 - PERDA DE MEMBROS")]
        PERDA_DE_MEMBROS = 5,

        [Display(Name = "006 -FRATURAS / FERIMENTOS")]
        FRATURA_FERIMENTOS = 6,

        [Display(Name = "007 - CONTUSOES, CORTES, ESCORIACOES")]
        CONTUSOES_CORTES_ESCORIACOES = 7,

        [Display(Name = "008 - CHOQUE ELETRICO SEM CONSEQUENCIAS")]
        CHOQUE_ELETRICO_SEM_CONSEQUENCIAS = 8,

        [Display(Name = "009 - ASFIXIA")]
        ASFIXIA = 9,            

        [Display(Name = "010 - LESOES MULTIPLAS ")]
        LESOES_MULTIPLAS = 10,

        [Display(Name = "011 - OUTRAS")]
        OUTRAS = 11

       


    }
}

