using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace G4_CasoEstudio2.App.Models
{
    public class Contexto : IdentityDbContext<Usuario>
    {
        public Contexto(DbContextOptions<Contexto> options) : base(options) { }

        
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Evento> Eventos { get; set; }        
        public DbSet<Asistencia> Asistencias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);



            // Configuración para Categoria
            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.Property(c => c.Nombre)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(c => c.Descripcion)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(c => c.Estado)
                    .IsRequired()
                    .HasDefaultValue(true);

                entity.Property(c => c.FechaRegistro)
                    .HasColumnType("datetime");

                entity.Property(c => c.UsuarioRegistro)
                    .IsRequired()
                    .HasMaxLength(450);

                // Configuración de la relación con AspNetUsers
                entity.HasOne(c => c.Usuario)
                    .WithMany()
                    .HasForeignKey(c => c.UsuarioRegistro)
                    .OnDelete(DeleteBehavior.Restrict).IsRequired(false); ;

                entity.HasMany(c => c.Eventos)
                    .WithOne(e => e.Categoria)
                    .HasForeignKey(e => e.CategoriaId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuración para Evento
            modelBuilder.Entity<Evento>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Titulo)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(1000);
                entity.Property(e => e.Fecha)
                    .HasColumnType("date");
                entity.Property(e => e.Hora)
                    .HasColumnType("time");
                entity.Property(e => e.Duration)
                    .IsRequired();
                entity.Property(e => e.Ubicacion)
                    .IsRequired()
                    .HasMaxLength(200);
                entity.Property(e => e.CupoMaximo)
                    .IsRequired();
                entity.Property(e => e.Estado)
                    .IsRequired()
                    .HasDefaultValue(true);
                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("GETDATE()");

                // Relación con Usuario (UsuarioRegistro)
                entity.Property(e => e.UsuarioRegistro)
               .HasMaxLength(450);

                // Relación con Categoria
                entity.HasOne(e => e.Categoria)
                    .WithMany(c => c.Eventos)
                    .HasForeignKey(e => e.CategoriaId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Relación uno a muchos con Asistencia
                entity.HasMany(e => e.Asistencias)
                    .WithOne(a => a.Evento)
                    .HasForeignKey(a => a.EventoId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuración para Asistencia
            modelBuilder.Entity<Asistencia>(entity =>
            {
                entity.HasKey(a => a.Id);

                // Relación con Evento
                entity.HasOne(a => a.Evento)
                    .WithMany(e => e.Asistencias)
                    .HasForeignKey(a => a.EventoId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Relación con Usuario
                entity.Property(a => a.UsuarioId)
               .HasMaxLength(450);

                // Podrías considerar añadir una clave única compuesta para evitar duplicados
                entity.HasIndex(a => new { a.EventoId, a.UsuarioId })
                    .IsUnique();
            });

           
            
        }
    }
}
