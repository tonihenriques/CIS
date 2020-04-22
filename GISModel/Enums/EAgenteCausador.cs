using System.ComponentModel.DataAnnotations;

namespace GISModel.Enums
{
    public enum EAgenteCausador
    {

        [Display(Name = "1 - REDE DE DISTRIBUIÇÃO AEREA")]
        Rede_de_distribuicao_aerea = 1,

        [Display(Name = "2 - REDE DE DISTRIBUIÇÃO SUBTERRANEA")]
        Rede_de_distribuicao_subterranea = 2,

        [Display(Name = "3 - PADRÃO DE MEDIÇÃO")]
        Padrao_de_medicao = 3,

        [Display(Name = "4 - LINHA DE TRANSMISSÃO AEREA")]
        Linha_de_transmissao_aerea = 4,

        [Display(Name = "5 - LINHA DE TRANSMISSÃO SUBTERRANEA")]
        Linha_de_transmissao_subterranea = 5,

        [Display(Name = "6 - SUBESTAÇÃO")]
        Subestacao = 6,

        [Display(Name = "7 - USINA")]
        Usina = 7,

        [Display(Name = "8 - FERRAMENTA / EQUIPAMENTO / MATERIAL")]
        Ferramenta_equipamento_material = 8,

        [Display(Name = "9 - VEÍCULO")]
        Veiculo = 9,

        [Display(Name = "10 - OUTRAS A CEMIG")]
        Outras_a_cemig = 10,

        [Display(Name = "11 - OUTRAS DE TERCEIRO")]
        Outras_de_terceiro = 11,

        [Display(Name = "12 - FENOMENO NATURAL")]
        Fenomeno_natural = 12,

        [Display(Name = "13 - RESERVATÓRIO / VERTEDOURO")]
        Reservatorio_vertedouro = 13,

        [Display(Name = "14 - TORRES / ESTRUTURAS DE TELECOMUNICAÇÕES")]
        Torres_estruturas_de_telecomunicacoes = 14
        
    }
}
