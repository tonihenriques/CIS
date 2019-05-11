using GISModel.Enums;
using System.ComponentModel.DataAnnotations;

namespace GISModel.DTO.Incidente
{
    public class VMNovaCodificacao
    {

        public string UniqueKey { get; set; }

        public string UKRelEnvolvido { get; set; }

        public string Tipo { get; set; }



        [Display(Name = "Tipo Acidente")]
        public ETipoAcidente TipoAcidente { get; set; }

        [Display(Name = "Atividade")]
        public EAtividades Atividade { get; set; }

        [Display(Name = "Tipo Atividade")]
        public string UKTipoAtividade { get; set; }

        [Display(Name = "Natureza")]
        public string UKNatureza { get; set; }

        [Display(Name = "Consequência Lesão")]
        public EConsequencia ConsequenciaLesao { get; set; }

        [Display(Name = "Função GRIDS")]
        public string UKFuncaoGRIDS { get; set; }

        [Display(Name = "Espécie Acid. Impressoal")]
        public string UKEspecieAcidImpessoal { get; set; }

        [Display(Name = "Tipo Acid. Pessoal")]
        public string UKTipoAcidPessoal { get; set; }

        [Display(Name = "Agente Acidente")]
        public string UKAgenteAcidente { get; set; }

        [Display(Name = "Fonte Lesão")]
        public string UKFonteLesao { get; set; }

        [Display(Name = "Fator Pessoal Inseg")]
        public string UKFatorPessoalInseg { get; set; }

        [Display(Name = "Ato Inseguro")]
        public string UKAtoInseguro { get; set; }

        [Display(Name = "Cond. Ambiental Inseg")]
        public string UKCondAmbientalInseg { get; set; }

        [Display(Name = "Prej. Material")]
        public string UKPrejMaterial { get; set; }

        public string Custo { get; set; }

        [Display(Name = "Dias Perdidos")]
        public string DiasPerdidos { get; set; }

        [Display(Name = "Dias Debitados")]
        public string DiasDebitados { get; set; }

        [Display(Name = "Data do Óbito")]
        public string DataObito { get; set; }

    }
}
