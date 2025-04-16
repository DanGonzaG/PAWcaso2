namespace G4_CasoEstudio2.App.Models
{
    public class AsistenciaDTO
    {
        public int Id { get; set; }
        public int EventoId { get; set; }
        public int UsuarioId { get; set; }
        public string NombreUsuario { get; set; }
        public string Correo { get; set; }
    }
}
