using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oficcios360.Models
{
    
    [Table("registro_actividades", Schema = "public")]
    public class RegistroActividad
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public string Descripcion { get; set; }

        public string? Observaciones { get; set; }

        [Required]
        [Range(0.1, 24.0, ErrorMessage = "Las horas deben ser mayores a 0.")]
        public double HorasDedicadas { get; set; }

        [Required]
        public string EstudianteIdentificacion { get; set; }

        public int? InformeId { get; set; }

        [ForeignKey("InformeId")]
        public virtual Informe? Informe { get; set; }
    }
}