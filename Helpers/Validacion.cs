using System.Text.RegularExpressions;

namespace Oficcios360.Helpers
{
    public class Validacion
    {
        public bool ValidarCamposCarta(Oficcios360.Models.Estudiante e)
        {
           
            if (string.IsNullOrEmpty(e.Nombre) || string.IsNullOrEmpty(e.Identificacion) || string.IsNullOrEmpty(e.Telefono))
                return false;

            
            bool nombreValido = Regex.IsMatch(e.Nombre, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$");

            
            bool cedulaValida = Regex.IsMatch(e.Identificacion, @"^[0-9]+$");
            bool telefonoValido = Regex.IsMatch(e.Telefono, @"^[0-9]+$");

            
            bool perfilValido = e.PerfilProfesional != null && e.PerfilProfesional.Length > 20;

            return nombreValido && cedulaValida && telefonoValido && perfilValido;
        }
        public bool ValidarCamposSeguimiento(Oficcios360.Models.RegistroActividad actividad)
        {
            if (actividad == null) return false;

           
            if (string.IsNullOrWhiteSpace(actividad.Descripcion) || string.IsNullOrWhiteSpace(actividad.EstudianteIdentificacion))
                return false;

            
            if (actividad.HorasDedicadas <= 0 || actividad.HorasDedicadas > 24)
                return false;

           
            if (actividad.Fecha > DateTime.Today)
                return false;

            return true;
        }


        public List<string> ValidarCamposHojaDeVida(Oficcios360.Models.Estudiante e)
        {
            var errores = new List<string>();

            
            if (string.IsNullOrWhiteSpace(e.Nombre))
                errores.Add("El nombre completo es obligatorio.");

            if (string.IsNullOrWhiteSpace(e.Identificacion))
                errores.Add("La cédula de ciudadanía (Identificación) es obligatoria.");

            if (string.IsNullOrWhiteSpace(e.Correo))
                errores.Add("El correo electrónico es obligatorio.");

            if (string.IsNullOrWhiteSpace(e.Telefono))
                errores.Add("El número de teléfono es obligatorio.");

            if (string.IsNullOrWhiteSpace(e.PerfilProfesional))
            {
                errores.Add("La descripción del perfil profesional es obligatoria.");
            }
            else if (e.PerfilProfesional.Length < 20)
            {
                errores.Add("El perfil profesional debe tener una descripción mínima de 20 caracteres.");
            }

            
            if (e.Formaciones == null || e.Formaciones.Count == 0)
            {
                errores.Add("Debe registrar al menos una formación académica.");
            }
            else
            {
                int anioActual = DateTime.Now.Year;
                foreach (var formacion in e.Formaciones)
                {
                    if (string.IsNullOrWhiteSpace(formacion.Institucion) || string.IsNullOrWhiteSpace(formacion.Titulo))
                    {
                        errores.Add("Todos los campos de la formación académica deben estar diligenciados.");
                    }

                    if (formacion.AnioFin < 1950 || formacion.AnioFin > anioActual + 10)
                    {
                        errores.Add($"El año '{formacion.AnioFin}' no es válido. Debe estar entre 1950 y {anioActual + 10}.");
                    }
                }
            }

            return errores;
        }
    }
}