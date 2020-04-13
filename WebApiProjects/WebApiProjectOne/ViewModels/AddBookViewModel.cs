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

        [Required(ErrorMessage = "El campo nombre es requerido")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Debe ingresar una descripcion")]
        [MinLength(10, ErrorMessage = "La descripcion ingresada debe contener al menos 10 caracteres")]
        public string Descripcion { get; set; }

        public string Autor { get; set; }

        [Required(ErrorMessage = "Debe seleccionar uno de los campos que hay")]
        public GeneroLibro Genero { get; set; }
    }
}
