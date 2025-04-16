using System.ComponentModel.DataAnnotations;

namespace G4_CasoEstudio2.App.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Nombre Completo")]
        [StringLength(100, ErrorMessage = "El nombre completo no puede exceder 100 caracteres.")]
        public string NombreUsuario { get; set; }
        [Required]
        public string Correo { get; set; }
        [Required]
        [Display(Name = "Teléfono")]
        [Phone(ErrorMessage = "Ingrese un número de teléfono válido.")]
        public string Telefono { get; set; }
        [Required]
        public string Contraseña { get; set; }
        [Required]
        public string Rol { get; set; }

        public List<Asistencia> Asistencias { get; set; } = new(); // <- Navegación
    }

}


