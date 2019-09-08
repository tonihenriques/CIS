using System.ComponentModel.DataAnnotations;

namespace GISModel.Enums
{
    public enum EPopulacaoAtividade
    {

        [Display(Name = "001 - Execução de Serviço de Telefonia ")]
        Execução_de_Serviço_de_Telefonia = 1,

        [Display(Name = "002 - Execução de Serviço Rurais")]
        Execução_de_Serviço_Rurais = 2,

        [Display(Name = "003 - Soltar papagaio / pipa")]
        Soltar_papagaio_pipa = 3,

        [Display(Name = "004 - Restabelecer ligação")]
        Restabelecer_ligação = 4,

        [Display(Name = "005 - Furto")]
        Furto = 5,

        [Display(Name = "006 - Instalação de Antena DE TV/Radio")]
        Instalação_de_Antena_TV = 6,

        [Display(Name = "007 - Construção Manutenção Predial")]
        Construção_Manutenção_Predial = 7,

        [Display(Name = "008 -Poda de Árvore")]
        Poda_de_Árvore = 8,

        [Display(Name = "009 - Obra em via pública")]
        Obra_em_via_publica = 9,

        [Display(Name = "010 - Outros")]
        Outros = 10

    }
}
