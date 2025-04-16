namespace G4_CasoEstudio2.App.Models
{
    public class Evento
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }
        public string Hora { get; set; }
        public string Ubicacion { get; set; }
        public int CupoMaximo { get; set; }
        public string Estado { get; set; }
        public DateTime FechaRegistro { get; set; }

        public string UsuarioRegistro { get; set; } // Puede ser ID o nombre según las reglas

        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }

        public List<Asistencia> Asistencias { get; set; } = new(); // <- Navegación
    }

}

