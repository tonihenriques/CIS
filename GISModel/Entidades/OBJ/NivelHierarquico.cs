using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades.OBJ
{
    [Table("OBJNivelHierarquico")]
    public class NivelHierarquico : EntidadeBase
    {

        [Display(Name = "Nível Hierárquico")]
        [Required(ErrorMessage = "Informe o nome do nível")]
        public string Nome { get; set; }

    }
}
