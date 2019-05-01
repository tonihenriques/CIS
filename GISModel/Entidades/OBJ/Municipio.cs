using GISModel.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GISModel.Entidades
{
    [Table("OBJMunicipio")]
    public class Municipio : EntidadeBase
    {
        
        [Display(Name ="Código")]
        public string Codigo { get; set; }

        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Display(Name = "Nome Completo")]
        public string NomeCompleto { get; set; }
        
        public virtual ICollection<Incidente> Incidentes { get; set; }

    }
}