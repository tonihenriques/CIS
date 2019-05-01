using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.DTO.Account
{
    public class NovaSenhaViewModel
    {

        public string IDUsuario { get; set; }

        public string Email { get; set; }

        [Required(ErrorMessage = "Informe a nova senha")]
        [Display(Name = "Nova Senha")]
        [DataType(DataType.Password)]
        public string NovaSenha { get; set; }

        [Required(ErrorMessage = "Informe a nova senha novamente")]
        [Display(Name = "Confirmar Nova Senha")]
        [DataType(DataType.Password)]
        public string ConfirmarNovaSenha { get; set; }

    }
}
