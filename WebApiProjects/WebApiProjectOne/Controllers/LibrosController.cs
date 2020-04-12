using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiProjectOne.Models;

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
        public async Task<ActionResult<Libros>> addBook([FromBody]Libros libro)
        {
            try
            {
                if (libro == null)
                {
                    return BadRequest();
                }

                //Chequeando que ese libro no haya sido ingresado
                var libroDb = await librosRepository.getLibroDb(libro.Nombre);
                if (libroDb != null)
                {
                    ModelState.AddModelError("libro", "Este libro ya ha sido ingresado");
                    return BadRequest(ModelState);
                }

                var libroAAgregar = await librosRepository.ingresarLibro(libro);
                return CreatedAtAction(nameof(getBook), new { id = libroAAgregar.Id }, libroAAgregar);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error...");
            }
        }


        [HttpPut("actualizar/{id:int}")]
        public async Task<ActionResult<Libros>> updateBook(int id, [FromBody]Libros libro)
        {
            try
            {
                if (id != libro.Id)
                {
                   return BadRequest();
                }

                var libroAActualizar = librosRepository.getBook(id);
                if (libroAActualizar == null)
                {
                    return NotFound($"El libro  con el id {id} no ha sido encontrado");
                }

                await librosRepository.actualizarLibro(libro);
                return RedirectToAction("getAllBooks");
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

                await librosRepository.eliminarLibro(id);
                return RedirectToAction("getAllBooks");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error...");
            }
        }


    }
}
