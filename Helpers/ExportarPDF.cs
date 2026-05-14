using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Font;
using iText.IO.Font.Constants;
using System.IO;

namespace Oficcios360.Helpers
{
    public class ExportarPDF
    {
        public byte[] GenerarCartaPdf(string contenido)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(ms);
                using (PdfDocument pdf = new PdfDocument(writer))
                {
                    // Configuración de página tamaño Carta (Letter)
                    Document document = new Document(pdf, iText.Kernel.Geom.PageSize.LETTER);

                    // Definir márgenes institucionales (en puntos: 72 pts = 1 pulgada)
                    document.SetMargins(70, 70, 70, 70);

                    // Tipografía profesional (Times Roman es el estándar para cartas)
                    PdfFont font = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);
                    document.SetFont(font);
                    document.SetFontSize(12);

                    // Separar el contenido por líneas para aplicar estilos individuales
                    string[] lineas = contenido.Split(new[] { "\n", "\r\n" }, StringSplitOptions.None);

                    foreach (var linea in lineas)
                    {
                        if (string.IsNullOrWhiteSpace(linea))
                        {
                            document.Add(new Paragraph("\n")); // Espaciado entre párrafos
                            continue;
                        }

                        Paragraph p = new Paragraph(linea);

                        // Lógica de alineación
                        if (linea.Contains("Bogotá D.C.") || linea.Contains("Atentamente"))
                        {
                            p.SetTextAlignment(TextAlignment.LEFT);
                        }
                        else if (linea.Contains("Asunto:"))
                        {
                            
                            p.SetMarginBottom(10);
                        }
                        else
                        {
                            p.SetTextAlignment(TextAlignment.JUSTIFIED); // Cuerpo del texto justificado
                        }

                        document.Add(p);
                    }

                    document.Close();
                }
                return ms.ToArray();
            }
        }
    }
}