using GISModel.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades.OBJ
{

    [Table("OBJWorkflow")]
    public class Workflow : EntidadeBase
    {

        [Display(Name = "Incidente")]
        public string UKObject { get; set; }

        public string Nome { get; set; }

        public string Status { get; set; }

        public int MajorVersion { get; set; }

        public int MinorVersion { get; set; }

        public string MensagemPasso { get; set; }

        public string Responsavel { get; set; }

    }
}
