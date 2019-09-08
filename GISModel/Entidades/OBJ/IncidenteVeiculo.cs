using GISModel.CustomAttributes;
using GISModel.DTO;
using GISModel.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades.OBJ
{

    [Table("OBJIncidenteVeiculo")]
    public class IncidenteVeiculo : EntidadeBase
    {

        [Display(Name = "Código")]
        public string Codigo { get; set; }

        [Display(Name = "Nº SMART")]
        public string NumeroSmart { get; set; }

        [Display(Name = "Acidente?")]
        public bool Acidente { get; set; }

        [Display(Name = "Acidente Fatal?")]
        public bool AcidenteFatal { get; set; }

        [Display(Name = "Acidente Potencial?")]
        public bool AcidentePotencial { get; set; }

        [Display(Name = "Tipo de Entrada")]
        [RequiredEnum(ErrorMessage = "Informe o tipo de entrada.")]
        public ETipoEntradaVeiculo ETipoEntrada { get; set; }

        [Display(Name = "Centro (Empresa)")]
        [RequiredEnum(ErrorMessage = "Informe o centro.")]
        public ECentroEmpresa Centro { get; set; }

        [RequiredEnum(ErrorMessage = "A regional é obrigatória")]
        [Display(Name = "Área de Trabalho")]
        public Regional Regional { get; set; }

        [Display(Name = "Diretoria")]
        public string UKDiretoria { get; set; }

        [Display(Name = "Orgão")]
        public string UKOrgao { get; set; }

        [Display(Name = "Data do Incidente")]
        [Required(ErrorMessage = "Informe a data do incidente.")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "date")]
        public DateTime DataIncidente { get; set; }

        [Display(Name = "Horário do Incidente")]
        public string HoraIncidente { get; set; }

        [Display(Name = "Tipo de Acidente")]
        [RequiredEnum(ErrorMessage = "Informe o tipo do acidente")]
        public ETipoAcidenteVeiculo ETipoAcidente { get; set; }

        [Display(Name = "Local do Acidente")]
        public string LocalAcidente { get; set; }

        [Display(Name = "Tipo do Local do Acidente")]
        [RequiredEnum(ErrorMessage = "O Tipo do Local do Acidente é obrigatório.")]
        public ETipoLocalAcid TipoLocalAcidente { get; set; }

        [Display(Name = "Desc. Logradouro")]
        public string Logradouro { get; set; }

        [Display(Name = "Número do Logradouro ")]
        public string NumeroLogradouro { get; set; }

        //Cada gerência da Sede possui um CNPJ no msm endereço
        //Portanto, não tem como definirmos uma classe específica
        [Display(Name = "CNPJ do local do Acidente")]
        public string CNPJLocalAcidente { get; set; }

        [Display(Name = "Descrição do Acidente")]
        [StringLength(250, MinimumLength = 20, ErrorMessage = "Máximo de 250 caracteres e Mínimo de 20 caracteres!")]
        [DataType(DataType.MultilineText)]
        public string Descricao { get; set; }

        [Display(Name = "Nº Boletim de Ocorrência")]
        public string NumeroBoletimOcorrencia { get; set; }

        [Display(Name = "Data Boletim de Ocorrência")]
        [Column(TypeName = "date")]
        public DateTime? DataBoletimOcorrencia { get; set; }

        [Display(Name = "Cidade")]
        public string UKMunicipio { get; set; }

        public string Estado { get; set; }

        [Display(Name = "eSocial - Descrição da Situação")]
        public string UKESocial { get; set; }


        [Display(Name = "Acid. grave (IP 10.2)?")]
        public bool AcidenteGraveIP102 { get; set; }

        [Display(Name = "Data Atualização")]
        public DateTime? DataAtualizacao { get; set; }


        public StatusIncidente Status { get; set; }

        public OperacaoCollection Operacoes { get; set; }

        public Workflow PassoAtual { get; set; }

    }
}
