using Oficcios360.Models;

namespace Oficcios360.Helpers
{
    public class Configuracion
    {
        public string ProcesarDatosParaCarta(Estudiante estudiante, string perfil)
        {
            // Prepara los datos para el formato preestablecido
            string encabezado = $"Bogotá D.C., {DateTime.Now.ToShortDateString()}\n\n";
            string cuerpo = $"Yo, {estudiante.Nombre.ToUpper()}, identificado con CC {estudiante.Identificacion}...\n";
            return encabezado + cuerpo + perfil;
        }
    }
}
