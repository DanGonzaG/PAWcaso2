using System.ComponentModel.DataAnnotations;

namespace G4_CasoEstudio2.App.Models
{
    public class Evento
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "El título no puede exceder 100 caracteres.")]
        public string Titulo { get; set; }

        [Required]
        [StringLength(1000, ErrorMessage = "La descripción no puede exceder 1000 caracteres.")]
        public string Descripcion { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [FutureDate(ErrorMessage = "La fecha del evento debe ser en el futuro.")]
        public DateTime Fecha { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan Hora { get; set; }

        [Required]
        [Range(1, 24, ErrorMessage = "La duración debe estar entre 1 y 24 horas.")]
        public int Duration { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "La ubicación no puede exceder 200 caracteres.")]
        public string Ubicacion { get; set; }

        [Required]
        [Display(Name = "Cupo Máximo")]
        [Range(1, int.MaxValue, ErrorMessage = "El cupo máximo debe ser mayor a 0.")]
        public int CupoMaximo { get; set; }

        [Required]
        public bool Estado { get; set; } = true;

        [DataType(DataType.DateTime)]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        public string UsuarioRegistro { get; set; }
        public Usuario Usuario { get; set; } // <- Navegación

        [Required]
        [Display(Name = "Categoría")]
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }

        public List<Asistencia> Asistencias { get; set; } = new(); // <- Navegación
    }
    public class FutureDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return value is DateTime date && date > DateTime.Now;
        }
    }

}

