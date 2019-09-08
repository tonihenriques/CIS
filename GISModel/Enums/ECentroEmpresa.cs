using System.ComponentModel.DataAnnotations;

namespace GISModel.Enums
{
    public enum ECentroEmpresa
    {
        [Display(Name = "5000 - CEMIG HOLDING")]
        CEMIG_HOLDING = 1,

        [Display(Name = "5100 Cemig Geração e Transmissão S/A")]
        CEMIG_GERAÇÃO_E_TRANSMISSÃO = 2,

        [Display(Name = "5300 - CEMIG DISTRIBUIÇÃO S.A")]
        CEMIG_DISTRIBUIÇÃO = 3

    }
}
