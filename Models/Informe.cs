using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oficcios360.Models
{
    [Table("informes")]
    public class Informe
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime PeriodoInicio { get; set; }

        [Required]
        public DateTime PeriodoFin { get; set; }

        public DateTime FechaGeneracion { get; set; } = DateTime.UtcNow;

        
        public string? RutaFirmaTutor { get; set; }

        
        public string Estado { get; set; } = "Borrador";

        [Required]
        public string EstudianteIdentificacion { get; set; }
        public double HorasRequeridas { get; set; } = 0;

        public virtual ICollection<RegistroActividad> Actividades { get; set; } = new List<RegistroActividad>();
    }
}