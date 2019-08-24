using System.ComponentModel.DataAnnotations;

namespace GISModel.DTO.Inbox
{
    public class ItemInbox
    {

        public string Responsavel { get; set; }

        public string UniqueKey { get; set; }


        public string UKWorkflow { get; set; }


        [Display(Name = "Comentários")]
        public string Comentarios { get; set; }

        public string Tipo { get; set; }

    }
}
