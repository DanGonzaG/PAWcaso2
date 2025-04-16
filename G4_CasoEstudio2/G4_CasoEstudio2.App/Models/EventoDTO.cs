namespace G4_CasoEstudio2.App.Models
{
    public class EventoDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public string Ubicacion { get; set; }
        public int CupoMaximo { get; set; }
        public string Categoria { get; set; } // solo el nombre de la categoría
    }
}
