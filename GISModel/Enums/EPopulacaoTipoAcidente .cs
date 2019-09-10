using System.ComponentModel.DataAnnotations;

namespace GISModel.Enums
{
    public enum EPopulacaoTipoAcidente
    {

        [Display(Name = "001 - Elétrico ")]
        Eletrico = 1,

        [Display(Name = "002 - Atropelamento")]
        Atropelamento = 2,

        [Display(Name = "003 - Outros de Trânsito")]
        Outros_de_Transito = 3,

        [Display(Name = "004 - Impacto de Pessoa Contra")]
        Impacto_de_pessoa_contra = 4,

        [Display(Name = "005 - Impacto sofrido por Pessoa")]
        Impacto_sofrido_por_pessoa = 5,

        [Display(Name = "006 - Queda de altura")]
        Queda_de_altura = 6,

        [Display(Name = "007 - Aprisionamento em, sob, ou entre estrutura")]
        Aprisionamento_em_sob_ou_entre = 7,

        [Display(Name = "008 - Queda de mesmo nível")]
        Queda_de_mesmo_nivel = 8,

        [Display(Name = "009 -Afogamento")]
        Afogamento = 9,

        [Display(Name = "010 - Outros")]
        Outros = 0



    }
}

