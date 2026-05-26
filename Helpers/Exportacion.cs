using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Font;
using iText.IO.Image;
using System.IO;
using Oficcios360.Models;
using System.Collections.Generic; 

namespace Oficcios360.Helpers
{
    public class Exportacion
    {
        public byte[] GenerarReporteSeguimientoPdf(Informe informe, List<RegistroActividad> actividades, double totalHoras)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(ms);
                using (PdfDocument pdf = new PdfDocument(writer))
                {
                    Document document = new Document(pdf, iText.Kernel.Geom.PageSize.LETTER);

                    
                    document.SetMargins(70f, 70f, 70f, 70f);

                    PdfFont font = PdfFontFactory.CreateFont("Times-Roman");
                    document.SetFont(font);

                    document.Add(new Paragraph("UNIVERSIDAD DISTRITAL FRANCISCO JOSÉ DE CALDAS").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER).SetFontSize(14));
                    document.Add(new Paragraph("INFORME PERIÓDICO DE SEGUIMIENTO DE PRÁCTICAS").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER).SetFontSize(12));
                    document.Add(new Paragraph($"Periodo: {informe.PeriodoInicio.ToShortDateString()} al {informe.PeriodoFin.ToShortDateString()} \n\n").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));

                    document.Add(new Paragraph($"Identificación Estudiante: {informe.EstudianteIdentificacion}").SetFontSize(11));
                    document.Add(new Paragraph($"Total de Horas Acreditadas en el periodo: {totalHoras} Horas\n\n").SetFontSize(11));

                    Table tabla = new Table(UnitValue.CreatePercentArray(new float[] { 20, 60, 20 })).UseAllAvailableWidth();
                    tabla.AddHeaderCell("Fecha");
                    tabla.AddHeaderCell("Descripción de la Actividad");
                    tabla.AddHeaderCell("Horas");

                    foreach (var act in actividades)
                    {
                        tabla.AddCell(act.Fecha.ToShortDateString());
                        tabla.AddCell(act.Descripcion);
                        tabla.AddCell(act.HorasDedicadas.ToString());
                    }
                    document.Add(tabla);
                    document.Add(new Paragraph("\n\n"));

                    if (!string.IsNullOrEmpty(informe.RutaFirmaTutor) && File.Exists(informe.RutaFirmaTutor))
                    {
                        ImageData data = ImageDataFactory.Create(informe.RutaFirmaTutor);

                        iText.Layout.Element.Image imgFirma = new iText.Layout.Element.Image(data)
                            .SetWidth(120)
                            .SetHeight(50)
                            .SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);

                        document.Add(new Paragraph("Aprobado Electrónicamente por:").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER).SetFontSize(10));
                        document.Add(imgFirma);
                        document.Add(new Paragraph("_____________________________________\nTutor Empresarial / Supervisor").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER).SetFontSize(11));
                    }
                    else
                    {
                        document.Add(new Paragraph("\n\n\n_____________________________________\nFirma Pendiente Tutor Empresarial").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER).SetFontSize(11));
                    }

                    document.Close();
                }
                return ms.ToArray();
            }
        }
    }
}