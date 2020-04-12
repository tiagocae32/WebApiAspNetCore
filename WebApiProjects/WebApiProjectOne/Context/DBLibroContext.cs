using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiProjectOne.Models;

namespace WebApiProjectOne.Context
{
    public class DBLibroContext : DbContext
    {

        public DBLibroContext(DbContextOptions<DBLibroContext> options) : base(options)
        {

        }


        public DbSet<Libros> dataLibros { get; set; }
    }
}
