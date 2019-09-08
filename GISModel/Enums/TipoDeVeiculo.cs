using System.ComponentModel.DataAnnotations;

namespace GISModel.Enums
{
    public enum TipoDeVeiculo
    {
        [Display(Name = "1 - PASSAGEIRO")]
        Passageiro = 1,

        [Display(Name = "3 - CAMINHONETE")]
        Caminhonete = 3,

        [Display(Name = "4 - CAMINHÃO")]
        Caminhao = 4,

        [Display(Name = "5 - CAVALO MECÂNICO")]
        Cavalo_Mecanico = 5,

        [Display(Name = "6 - TRANSP. COLETIVO")]
        Transp_Coletivo = 6,

        [Display(Name = "7 - CAMINHÃO LEVE")]
        Caminhao_Leve = 7,

        [Display(Name = "8 - CAMINHÃO C/EQUIP HIDRÁULICO")]
        Caminhao_Equip_Hidraulico = 8,

        [Display(Name = "9 - MOTOCICLETA")]
        Motocicleta = 9
    }
}
