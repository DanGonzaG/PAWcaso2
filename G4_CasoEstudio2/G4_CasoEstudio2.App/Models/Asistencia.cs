namespace G4_CasoEstudio2.App.Models
{
    public class Asistencia
    {
        public int Id { get; set; }

        public int EventoId { get; set; }
        public Evento Evento { get; set; }

        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
    }

}
