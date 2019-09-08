using System.ComponentModel.DataAnnotations;

namespace GISModel.Enums
{
    public enum EPopulacaoCausa
    {



        [Display(Name = "001 - CONTATO COM PARTE ENERGIZADA ")]
        CONTATO_COM_PARTE_ENERGIZADA = 1,

        [Display(Name = "002 - ESCALAR ESTRUTURA / EQUIPAMENTO")]
        ESCALAR_ESTRUTURA_EQUIPAMENTO = 2,

        [Display(Name = "003 - QUEDA DE ALTURA")]
        QUEDA_DE_ALTURA = 3,

        [Display(Name = "004 - QUEDA DE OBJETOS SOBRE")]
        QUEDA_DE_OBJETOS_SOBRE = 4,

        [Display(Name = "005 - CONTATO COM PARTE ENERG. INDEVIDAMENTE")]
        CONTATO_COM_PARTE_ENERG_INDEVIDAMENTE = 5,

        [Display(Name = "006 - JOGAR OBJETOS")]
        JOGAR_OBJETOS = 6,

        [Display(Name = "007 - VANDALISMO")]
        VANDALISMO = 7,

        [Display(Name = "008 - INVASAO DE FAIXA")]
        INVASAO_DE_FAIXA = 8,

        [Display(Name = "9 - ABALROAMENTO DE POSTES / ESTRUTURAS")]
        ABALROAMENTO_DE_POSTES_ESTRUTURAS = 9,


        [Display(Name = "010 - INVASAO DE AREA DE SEGURANCA ")]
        INVASAO_DE_AREA_DE_SEGURANCA = 10,

        [Display(Name = "011 - ATROPELAMENTO POR VEICULO DA EMPRESA")]
        ATROPELAMENTO_POR_VEICULO_DA_EMPRESA = 11,

        [Display(Name = "012 - ACIDENTE DE TRANSITO C/ VEICULO DA EMPR.")]
        ACIDENTE_DE_TRANSITO_C_VEICULO_DA_EMPR = 12,

        [Display(Name = "13 - TOCAR NA REDE NA EXECUCAO DE OBRAS CIVIS")]
        TOCAR_NA_REDE_NA_EXECUCAO_DE_OBRAS_CIVIS = 13,


        [Display(Name = "014 - TOCAR NA REDE COM INTENSAO")]        

        TOCAR_NA_REDE_COM_INTENSAO = 14,

        [Display(Name = "015 - QUEDA DE CONDUTOR / EQUIP. SOBRE PESSOA")]
        QUEDA_DE_CONDUTOR_EQUIP_SOBRE_PESSOA = 15,

        [Display(Name = "016 - TOCAR EM CONDUTOR ENERGIZADO NO SOLO")]
        TOCAR_EM_CONDUTOR_ENERGIZADO_NO_SOLO = 16,

        [Display(Name = "017 - TOCAR NA REDE DURANTE PODA DE ARVORE")]
        TOCAR_NA_REDE_DURANTE_PODA_DE_ARVORE = 17,

        [Display(Name = "018 - ATAQUE DE INSETOS")]
        ATAQUE_DE_INSETOS = 18,

        [Display(Name = "019 - CHOQUE ELET A PARTIR DA CAIXA DE MEDICAO")]
        CHOQUE_ELET_A_PARTIR_DA_CAIXA_DE_MEDICAO = 19,

        [Display(Name = "020 - OUTROS")]

        OUTROS = 20,




    }
}
