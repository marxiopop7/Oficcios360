namespace Oficcios360.Models
{
    public class Estudiante : Usuario
    {
        public string? Telefono { get; set; }
        public string? Ciudad { get; set; }
        public string ModalidadGrado { get; set; } = "Pasantías"; // Pasantía, Proyecto, etc.
        public string? PerfilProfesional { get; set; }
    }
}
