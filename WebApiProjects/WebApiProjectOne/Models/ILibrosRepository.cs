using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiProjectOne.Models
{
    public interface ILibrosRepository
    {

        Task<IEnumerable<Libros>> searchBook(string nombre, GeneroLibro? libro);

        Task<Libros> getLibroDb(string nombre);

        Task<IEnumerable<Libros>> getAllBooks();

        Task<IEnumerable<Libros>> getBook(int id);

        Task<Libros> ingresarLibro(Libros libro);

        Task<Libros> actualizarLibro(Libros libro);

        Task<Libros> eliminarLibro(int id);


    }
}
