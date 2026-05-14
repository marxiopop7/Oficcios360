using System.ComponentModel.DataAnnotations;

namespace Oficcios360.Models
{
    public class Carta
    {
        [Key]
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Identificacion { get; set; }

        public string Telefono { get; set; }

        public string ModalidadGrado { get; set; }

        public string PerfilProfesional { get; set; }

        public DateTime FechaGeneracion { get; set; } = DateTime.UtcNow;
    }
}