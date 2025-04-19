using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace G4_CasoEstudio2.App.Models
{
    public class Usuario : IdentityUser
    {
        [Required]
        [Display(Name = "Nombre Completo")]
        [StringLength(100, ErrorMessage = "El nombre completo no puede exceder 100 caracteres.")]
        public string NombreCompleto { get; set; }

        [Required]
        [Display(Name = "Teléfono")]
        [Phone(ErrorMessage = "Ingrese un número de teléfono válido.")]
        public override string PhoneNumber { get; set; }

        public List<Asistencia> Asistencias { get; set; } = new(); // <- Navegación
    }

}


