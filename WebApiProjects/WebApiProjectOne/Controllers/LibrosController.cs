using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiProjectOne.Context;
using WebApiProjectOne.Models;
using WebApiProjectOne.ViewModels;

namespace WebApiProjectOne.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibrosController : ControllerBase
    {
        private ILibrosRepository librosRepository;
  
        public LibrosController(ILibrosRepository LibrosRepository)
        {
            librosRepository = LibrosRepository;

        }


        [HttpGet("{search}")]
        public async Task<ActionResult<IEnumerable<Libros>>> searchBook(string nombre, GeneroLibro? genero)
        {
            try
            {
                var result = await librosRepository.searchBook(nombre, genero);

                if (result.Any())
                {
                    return Ok(result);
                }
                return NotFound("Libro no encontrado");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error...");
            }
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Libros>>> getAllBooks()
        {
            try
            {
                return Ok(await librosRepository.getAllBooks());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error...");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Libros>> getBook(int id)
        {
            try
            {
                var result = await librosRepository.getBook(id);

                if (result == null)
                {
                    return NotFound($"El libro  con el id {id} no ha sido encontrado");
                }
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error...");
            }
        }


        [HttpPost]
        public async Task<ActionResult<Libros>> addBook([FromBody]AddBookViewModel libro)
        {
            try
            {
                if (libro == null)
                {
                    return BadRequest();
                }

                var libroDb = await librosRepository.getLibroDb(libro.Nombre);
                if (libroDb != null)
                {
                    ModelState.AddModelError("libro", "Este libro ya ha sido ingresado");
                    return BadRequest(ModelState);
                }

                Libros newLibro = new Libros()
                {
                    Nombre = libro.Nombre,
                    Descripcion = libro.Descripcion,
                    Genero = libro.Genero,
                    Autor = libro.Autor
                };

                var libroAAgregar = await librosRepository.addBook(newLibro);
                return CreatedAtAction(nameof(getBook), new { id = libroAAgregar.Id }, libroAAgregar);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error...");
            }

        }

        //Vista para editar un libro/////
        [HttpGet("actualizar/{id:int}")]
        public async Task<ActionResult<Libros>> updateBook(int id)
        {
            try
            {
                Libros libroAActualizar = await librosRepository.getBook(id);

                if (libroAActualizar == null)
                {
                    return NotFound($"El libro  con el id {id} no ha sido encontrado");
                }

                EditBookViewModel infoLibro = new EditBookViewModel()
                {
                    Id = libroAActualizar.Id,
                    Nombre = libroAActualizar.Nombre,
                    Descripcion = libroAActualizar.Descripcion,
                    Genero = libroAActualizar.Genero,
                    Autor = libroAActualizar.Autor
                };

                return Ok(infoLibro);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error...");
            }      
        }

        //Accion de editar un libro//////////
        [HttpPut("actualizar/{id:int}")]
        public async Task<ActionResult<Libros>> updateBook([FromBody]EditBookViewModel libro)
        {
            try
            {
                 /*if (id != libro.Id)
                 {
                    return BadRequest();
                 }*/

                Libros libroAActualizar = await librosRepository.getBook(libro.Id);

                if (libroAActualizar == null)
                {
                    return NotFound($"El libro  con el id {libro.Id} no ha sido encontrado");
                }

                libroAActualizar.Nombre = libro.Nombre;
                libroAActualizar.Descripcion = libro.Descripcion;
                libroAActualizar.Genero = libro.Genero;
                libroAActualizar.Autor = libro.Autor;
                            
               return Ok(await librosRepository.updateBook(libroAActualizar));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error...");
            }      

        }
        

        [HttpDelete("eliminar/{id:int}")]
        public async Task<ActionResult<Libros>> deleteBook(int id)
        {
            try
            {
                var libroAEliminar = await librosRepository.getBook(id);

                if (libroAEliminar == null)
                {
                    return NotFound($"El libro  con el id {id} no ha sido encontrado");
                }

                await librosRepository.deleteBook(id);
                return RedirectToAction("getAllBooks");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error...");
            }
        }


    }
}
