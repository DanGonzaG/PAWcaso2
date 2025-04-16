using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace G4_CasoEstudio2.App.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "El nombre no puede exceder 50 caracteres.")]
        public string Nombre { get; set; }
        [Required]
        [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres.")]
        public string Descripcion { get; set; }
        [Required]
        public bool Estado { get; set; } = true; // Activo por defecto
        [DataType(DataType.DateTime)]
        public DateTime FechaRegistro { get; set; }
        public int UsuarioRegistro { get; set; }
        public Usuario Usuario { get; set; }
        public List<Evento> Eventos { get; set; } = new(); // <- Navegación
    }

}
