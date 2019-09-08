using System.ComponentModel.DataAnnotations;

namespace GISModel.Enums
{
    public enum NaturezaVeiculo
    {
        [Display(Name = "1 - MANOBRA DE PATIO")]
        Manobra_Patio = 1,

        [Display(Name = "2 - MANOBRA DE RUA")]
        Manobra_Rua = 2,

        [Display(Name = "3 - MANOBRA EM CAMPO DE TREINAMENTO")]
        Manobra_Campo = 3,

        [Display(Name = "4 - COLISÃO")]
        Colisao = 4,

        [Display(Name = "5 - ABALROAMENTO")]
        Abalroamento = 5,

        [Display(Name = "6 - ATROPELAMENTO")]
        Atropelamento = 6,

        [Display(Name = "7 - CAPOTAMENTO")]
        Capotamento = 7,

        [Display(Name = "8 - CHOQUE")]
        Choque = 8,

        [Display(Name = "9 - TOMBAMENTO")]
        Tombamento = 9,

        [Display(Name = "10 - QUEDA")]
        Queda = 10,

        [Display(Name = "11 - OUTROS DE TRÂNSITO")]
        Outros_de_transito = 11,

        [Display(Name = "12 - NÃO DE TRÂNSITO")]
        Nao_de_transito = 12,

        [Display(Name = "13 - OBJETO LANÇADO NO PARA-BRISAS DO VEÍCULO")]
        Objeto_Lancado_Parabrisa = 13,

        [Display(Name = "14 - OBJETO LANÇADO NO VEÍCULO")]
        Objeto_Lancado_Veiculo = 14,

        [Display(Name = "15 - OBJETO PERFURO CORTANTE - DANO AO TANQUE")]
        Objeto_Perfuro_Cortante = 15,

        [Display(Name = "16 - DANO EM EQUIPAMENTO ACOPLADO AO VEÍCULO")]
        Dano_equip_acoplado = 16,

        [Display(Name = "17 - MECÂNICA - QUEBRA DE EIXO/RODA/BARRA DIREÇÃO")]
        Mec_quebra_eixo = 17,
    }
}
