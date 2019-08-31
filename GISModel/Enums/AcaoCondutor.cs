using System.ComponentModel.DataAnnotations;

namespace GISModel.Enums
{
    public enum AcaoCondutor
    {

        [Display(Name = "1 - DESRESPEITO AO SEMAFORO")]
        Desrespeito_Semaforo = 1,

        [Display(Name = "2 - DESRESPEITO A PREFERENCIAL, FAIXAS ETC")]
        Desrespeito_Preferencial = 2,

        [Display(Name = "3 - ULTRAPASSAGEM PROÍBIDA")]
        Ultrapassagem_Proibida = 3,

        [Display(Name = "4 - ULTRAPASSAGEM INCORRETA")]
        Ultrapassagem_Incorreta = 4,

        [Display(Name = "5 - ALTA VELOCIDADE PARA AS CONDIÇÕES")]
        Alta_velocidade = 5,

        [Display(Name = "6 - NÃO SINALIZAR INTENSÃO")]
        Nao_sinalizar_intensao = 6,

        [Display(Name = "7 - MANOBRAR INCORRETAMENTE")]
        Manobrar_incorretamente = 7,

        [Display(Name = "8 - ESTACIONAR INADEQUADAMENTE")]
        Estacionar_inadequadamente = 8,

        [Display(Name = "9 - NÃO MANTER A DISTÂNCIA DE SEGURANÇA")]
        Nao_manter_distancia = 9,

        [Display(Name = "10 - CONVERSÃO INCORRETA / LOCAL PROÍBIDO")]
        Conversao_incorreta = 10,

        [Display(Name = "11 - MUDANÇA SUBITA DE FAIXA / DIREÇÃO")]
        Mudanca_subita_faixa = 11,

        [Display(Name = "12 - NÃO DIRIGIR COM CUIDADOS INDISPENSÁVEIS")]
        Nao_dirigir_cuidados = 12,

        [Display(Name = "13 - OUTROS")]
        Outros = 13,

        [Display(Name = "14 - INEXISTENTE")]
        Inexistente = 14,

    }
}
