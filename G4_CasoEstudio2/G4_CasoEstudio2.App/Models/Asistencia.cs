using System.ComponentModel.DataAnnotations;

namespace G4_CasoEstudio2.App.Models
{
    public class Asistencia
    {
        public int Id { get; set; }
        [Required]
        public int EventoId { get; set; }
        public Evento Evento { get; set; }
        [Required]
        public string UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
    }

}
