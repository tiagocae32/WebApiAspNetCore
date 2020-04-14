using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiProjectOne.Models;


namespace WebApiProjectOne.Context
{
    public class DBLibroContext : IdentityDbContext
    {

        public DBLibroContext(DbContextOptions<DBLibroContext> options) : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<Libros> dataLibros { get; set; }
    }
}
