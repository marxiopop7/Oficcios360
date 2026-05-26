using System.Text.RegularExpressions;

namespace Oficcios360.Helpers
{
    public class Validacion
    {
        public bool ValidarCamposCarta(Oficcios360.Models.Estudiante e)
        {
            // 1. Validar que no haya nulos
            if (string.IsNullOrEmpty(e.Nombre) || string.IsNullOrEmpty(e.Identificacion) || string.IsNullOrEmpty(e.Telefono))
                return false;

            // 2. Validar que el Nombre SOLO contenga letras y espacios
            bool nombreValido = Regex.IsMatch(e.Nombre, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$");

            // 3. Validar que Identificación y Teléfono sean SOLO números
            bool cedulaValida = Regex.IsMatch(e.Identificacion, @"^[0-9]+$");
            bool telefonoValido = Regex.IsMatch(e.Telefono, @"^[0-9]+$");

            // 4. Validar longitud mínima del perfil (según tu requerimiento previo)
            bool perfilValido = e.PerfilProfesional != null && e.PerfilProfesional.Length > 20;

            return nombreValido && cedulaValida && telefonoValido && perfilValido;
        }
        public bool ValidarCamposSeguimiento(Oficcios360.Models.RegistroActividad actividad)
        {
            if (actividad == null) return false;

            // Validar campos obligatorios vacíos
            if (string.IsNullOrWhiteSpace(actividad.Descripcion) || string.IsNullOrWhiteSpace(actividad.EstudianteIdentificacion))
                return false;

            // Validar consistencia de horas en el día
            if (actividad.HorasDedicadas <= 0 || actividad.HorasDedicadas > 24)
                return false;

            // Validar que la fecha no sea una fecha futura incoherente
            if (actividad.Fecha > DateTime.Today)
                return false;

            return true;
        }
    }
}