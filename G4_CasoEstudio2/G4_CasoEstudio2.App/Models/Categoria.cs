using static System.Runtime.InteropServices.JavaScript.JSType;

namespace G4_CasoEstudio2.App.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string UsuarioRegistro { get; set; }

        public List<Evento> Eventos { get; set; } = new(); // <- Navegación
    }

}
