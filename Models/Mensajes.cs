namespace Oficcios360.Models
{
    public class Mensajes
    {
        public string Tipo { get; set; } // "Success", "Error", "Wait"
        public string Encabezado { get; set; }
        public string CuerpoTexto { get; set; }

        public static Mensajes ErrorValidacion() => new Mensajes
        {
            Tipo = "Error",
            Encabezado = "Error de Formato",
            CuerpoTexto = "El sistema detectó campos vacíos o incorrectos. Por favor, corrija el formulario."
        };
    }
}
