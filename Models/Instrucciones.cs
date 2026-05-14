namespace Oficcios360.Models
{
    public class Instrucciones
    {

        public string DescripcionPasoAPaso { get; set; }

        public string LinkOficial { get; set; } = "https://muisca.dian.gov.co/";
        public string QueEsElRut => "El Registro Único Tributario (RUT) es el mecanismo para identificar, ubicar y clasificar a las personas y entidades que tengan la calidad de contribuyentes declarantes del impuesto sobre la renta.";

        public string ObjetivoModulo => "Este módulo asiste al estudiante en la navegación de la plataforma DIAN, asegurando que el documento se genere bajo los parámetros legales vigentes.";

        public List<string> PasosDetallados => new List<string> {
            "1. Haga clic en el botón 'Acceder a Plataforma DIAN' para abrir el portal oficial.",
            "2. El sistema entrará en 'Modo Espera' para no interrumpir su trámite.",
            "3. En la página emergente, seleccione 'Inscripción RUT' y complete los datos solicitados por la DIAN.",
            "4. Una vez descargue su PDF oficial, cierre la ventana emergente.",
            "5. Al cerrar, el sistema se reactivará para que complete la encuesta de satisfacción."
        };
    }
}
