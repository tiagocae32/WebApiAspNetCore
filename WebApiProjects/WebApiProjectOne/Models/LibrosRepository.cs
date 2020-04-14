using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiProjectOne.Context;


namespace WebApiProjectOne.Models
{
    public class LibrosRepository : ILibrosRepository
    {
        private readonly DBLibroContext Context;

        public LibrosRepository(DBLibroContext context)
        {
            Context = context;
        }


        public async Task<IEnumerable<Libros>> searchBook(string nombre, GeneroLibro? genero)
        {

            IQueryable<Libros> query = Context.dataLibros;


            if (!string.IsNullOrEmpty(nombre))
            {
                query = query.Where(l => l.Nombre.Contains(nombre)); ///||
            }

            if (genero != null)
            {
                query = query.Where(l => l.Genero == genero);
            }

            return await query.ToListAsync();

        }


        ///Verificando que un libro ya exista o no en la DB/////////////////////
        public async Task<Libros> getLibroDb(string nombre)
        {
            /*  var libroDb = nombre;

              var request = await Context.dataTareas
                   .FromSqlInterpolated($"EXECUTE dbo.ChequearLibroExistente {libroDb}")
                   .ToListAsync();

              return request.Any() ? (IEnumerable<Libros>)request : null  ;*/
            return await Context.dataLibros.FirstOrDefaultAsync(t => t.Nombre == nombre);
        }
        ////////////////////--------------///////////////////////////////////


        ///////Obtener todos los libros//////////////////
        public async Task<IEnumerable<Libros>> getAllBooks()
        {
            //return await Context.dataTareas.ToListAsync();
            return await Context.dataLibros
           .FromSqlRaw("SELECT * FROM dbo.dataLibros")
           .ToListAsync();
        }
        ////////////---------------///////////////////////


        //////Obteniendo un libro por su id////////////
        public async Task<Libros>getBook(int id)
        {
            var userId = id;

             var tarea = await Context.dataLibros
                  .FromSqlInterpolated($"EXECUTE dbo.ObtenerLibroPorId {userId}")
                  .ToListAsync();

            return tarea.Count > 0 ? tarea.First() : null; 

            //return await Context.dataLibros.FirstOrDefaultAsync(l => l.Id == id);
        }
        ////////-------------------------/////////////


        //////Agregando un libro///////////////////////
        public async Task<Libros> addBook(Libros libro)
        {
            var result = await Context.dataLibros.AddAsync(libro);
            await Context.SaveChangesAsync();
            return result.Entity;
        }
        //////////////-------------///////////////////


        ////Actualizando un libro///////////////////
        public async Task<Libros> updateBook(Libros libroEdit)
        {

            var Libro = Context.dataLibros.Attach(libroEdit);
            Libro.State = EntityState.Modified;
            await Context.SaveChangesAsync();
            return libroEdit;
        }
        /////////---------------//////////////////////


        //////Eliminando un libro///////////////////
        public async Task<Libros> deleteBook(int id)
        {
            var tareaId = await Context.dataLibros.FirstOrDefaultAsync(x => x.Id == id);

            if (tareaId != null)
            {
                Context.dataLibros.Remove(tareaId);
                await Context.SaveChangesAsync();
                return tareaId;
            }
            return null;
        }

    }

}
