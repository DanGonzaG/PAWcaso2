namespace G4_CasoEstudio2.App.Models
{
    public class Evento
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public DateOnly Fecha { get; set; }
        public TimeOnly Hora { get; set; }
        public int Duracion { get; set; } // en minutos
        public string Ubicacion { get; set; }
        public int CupoMaximo { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public string UsuarioRegistro { get; set; } = "Sistema"; // Asignar un valor por defecto
        public int CategoriaId { get; set; }

        public Categoria Categoria { get; set; } // Relación con la entidad Categoria
    }
}

