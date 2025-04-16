using Microsoft.EntityFrameworkCore;

namespace G4_CasoEstudio2.App.Models
{
    public class Contexto : DbContext
    {
        public Contexto(DbContextOptions<Contexto> options) : base(options) { }

        
        //public DbSet<Usuarios> Usuarios { get; set; }
        //public DbSet<Categorias> Categorias { get; set; }
        //public DbSet<Eventos> Eventos { get; set; }        
        //public DbSet<Asistencia> Asistencia { get; set; }
       
    }
}
