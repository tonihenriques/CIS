using System.ComponentModel.DataAnnotations;

namespace GISModel.Enums
{
    public enum ENatureza
    {

        [Display(Name = "01: MANUSEIO INCORRETO DE FERRAMENTAS/EQUIP.")]
        Manuseio_incorreto_de_ferramentas = 1,

        [Display(Name = "02: QUEDA DE POSTE / TORRE / ESTRUTURA")]
        Queda_de_poste_torre_estrutura = 2,

        [Display(Name = "03: QUEDA DE ÁRVORE, ESCADAS, ETC")]
        Queda_de_arvore_escadas_etc = 3,

        [Display(Name = "04: VEÍCULOS")]
        Veiculos = 4,

        [Display(Name = "05: MOTOS")]
        Motos = 5,

        [Display(Name = "06: ATAQUE DE CÃES")]
        Ataque_de_caes = 6,

        [Display(Name = "07: ATAQUE DE SERES VIVOS")]
        Ataque_de_seres_vivos = 7,

        [Display(Name = "08: REAÇÃO DO CORPO EM MOV. VOLUNT. / INVOL.")]
        Reacao_do_corpo_em_mov_voluntario_involuntario = 8,

        [Display(Name = "09: ESFORÇO REPETITIVO")]
        Esforco_repetitivo = 9,

        [Display(Name = "10: ESFORÇO EXCESSIVO")]
        Esforco_excessivo = 10,

        [Display(Name = "11 - IMPACTO SOFRIDO POR PESSOA")]
        Impacto_sofrido_por_pessoa = 11,

        [Display(Name = "12: IMPACTO DE PESSOA CONTRA")]
        Impacto_de_pessoa_contra = 12,

        [Display(Name = "13: QUEDA DE OBJETOS, MATERIAIS E EQUIP.")]
        Queda_de_objetos_materiais_e_equip = 13,

        [Display(Name = "14: NATUREZA ELETRÍCA")]
        Natureza_eletrica = 14,

        [Display(Name = "15: PROJEÇÃO DE PARTÍCULAS")]
        Projecao_de_particulas = 15,

        [Display(Name = "16: ORIGEM QUÍMICA")]
        Origem_quimica = 16,

        [Display(Name = "17: ORIGEM FÍSICA")]
        Origem_fisica = 17,

        [Display(Name = "18: ORIGEM BIOLÓGICA")]
        Origem_biologica = 18,

        [Display(Name = "19: EXPLOSOES")]
        Explosoes = 19,

        [Display(Name = "20: OUTROS")]
        Outros = 20

    }
}
