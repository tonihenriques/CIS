using System.ComponentModel.DataAnnotations;

namespace GISModel.Enums
{
    public enum ETipoMaterial
    {
       
        [Display(Name = "001 - VEÍCULOS")]
        VEÍCULOS = 1,

        [Display(Name = "002 - INSTALAÇÕES PREDIAIS")]
        INSTALACOES_PREDIAIS = 2,

        [Display(Name = "003 - MÁQUINAS ou EQUIPAMENTOS")]
        MAQUINAS_EQUIPAMENTOS = 3,

        [Display(Name = "004 - MÓVEIS")]
        MOVEIS = 4,

        [Display(Name = "005 - FERRAMENTAS E ACESSÓRIOS")]
        FERRAMENTAS_ACESSORIOS = 5,

        [Display(Name = "006 - COMPONENTES DO SEP - Distribuição")]
        COMPONENTES_DO_SEP_Distribuição = 6,

        [Display(Name = "007 - COMPONENTES DO SEP - Transmissão")]
        COMPONENTES_DO_SEP_Transmissão = 7,

        [Display(Name = "008 - COMPONENTES DO SEP - Geração")]
        COMPONENTES_DO_SEP_Geração = 8

        


    }
}

