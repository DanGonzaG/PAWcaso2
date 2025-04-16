using Microsoft.EntityFrameworkCore;

namespace G4_CasoEstudio2.App.Models
{
    public class Contexto : DbContext
    {
        public Contexto(DbContextOptions<Contexto> options) : base(options) { }

        
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Evento> Eventos { get; set; }        
        public DbSet<Asistencia> Asistencias { get; set; }
       
    }
}
