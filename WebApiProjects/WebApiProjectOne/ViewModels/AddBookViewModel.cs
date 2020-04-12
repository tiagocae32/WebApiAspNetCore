using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApiProjectOne.Models;

namespace WebApiProjectOne.ViewModels
{
    public class AddBookViewModel
    {

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Descripcion { get; set; }

        public string Autor { get; set; }

        [Required]
        public GeneroLibro Genero { get; set; }
    }
}
