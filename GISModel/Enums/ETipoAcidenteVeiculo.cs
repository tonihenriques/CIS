using System.ComponentModel.DataAnnotations;

namespace GISModel.Enums
{
    public enum ETipoAcidenteVeiculo
    {

        [Display(Name = "003 - Acidente com Estagiário")]
        Acidente_com_Estagiario = 1,

        [Display(Name = "005 - Acidente com População")]
        Acidente_com_Populacao = 2,

        [Display(Name = "006 - Acidente com Prestador de Serviços")]
        Acidente_com_Prestador_Servico = 3,

        [Display(Name = "008 - Acidente em Novos Négocios")]
        Acidente_em_Novos_Negocio = 5,

        [Display(Name = "009 - Acidente em Obra PART")]
        Acidente_em_Obra_PART = 6,

        [Display(Name = "012 - Doença Ocupacional")]
        Doença_Ocupacional = 7

    }
}
