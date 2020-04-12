using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiProjectOne.Models
{
    public class Libros
    {
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Descripcion { get; set; }

        public string Autor { get; set; }

        [Required]
        public GeneroLibro Genero { get; set; }
    }
}
