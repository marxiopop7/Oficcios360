using System.ComponentModel.DataAnnotations;

namespace Oficcios360.Models
{
    public class Encuesta
    {
        [Key]
        public int Id { get; set; }

        public string Rating { get; set; }

        public string Completado { get; set; }

        public string? Comentario { get; set; }

        public DateTime Fecha { get; set; } = DateTime.UtcNow;
    }
}