namespace G4_CasoEstudio2.App.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string Contraseña { get; set; }
        public string Rol { get; set; }

        public List<Asistencia> Asistencias { get; set; } = new(); // <- Navegación
    }

}


