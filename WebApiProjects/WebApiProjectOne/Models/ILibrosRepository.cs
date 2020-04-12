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

        Task<Libros> getBook(int id);

        Task<Libros> addBook(Libros libro);

        Task<Libros> updateBook(Libros libro);

        Task<Libros> deleteBook(int id);


    }
}
