using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
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
                    
                    Document document = new Document(pdf, iText.Kernel.Geom.PageSize.LETTER);

                    
                    document.SetMargins(70, 70, 70, 70);

                    
                    PdfFont font = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);
                    document.SetFont(font);
                    document.SetFontSize(12);

                    
                    string[] lineas = contenido.Split(new[] { "\n", "\r\n" }, StringSplitOptions.None);

                    foreach (var linea in lineas)
                    {
                        if (string.IsNullOrWhiteSpace(linea))
                        {
                            document.Add(new Paragraph("\n")); 
                            continue;
                        }

                        Paragraph p = new Paragraph(linea);

                        
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
                            p.SetTextAlignment(TextAlignment.JUSTIFIED); 
                        }

                        document.Add(p);
                    }

                    document.Close();
                }
                return ms.ToArray();
            }
        }

        public byte[] GenerarHojaVidaPdf(Oficcios360.Models.Estudiante e)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(ms);
                using (PdfDocument pdf = new PdfDocument(writer))
                {
                    Document document = new Document(pdf, iText.Kernel.Geom.PageSize.LETTER);
                    document.SetMargins(45, 55, 45, 55);

                    PdfFont fontReg = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
                    PdfFont fontBld = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

                    
                    DeviceRgb colorGrisBloque = new DeviceRgb(122, 114, 112);

                    
                    Cell cellBarra1 = new Cell().SetBackgroundColor(colorGrisBloque)
                        .SetBorder(Border.NO_BORDER).SetPadding(5)
                        .SetTextAlignment(TextAlignment.CENTER);
                    cellBarra1.Add(new Paragraph("DATOS PERSONALES").SetFont(fontBld).SetFontSize(12).SetFontColor(ColorConstants.WHITE).SetCharacterSpacing(1.5f));

                    Table tBarra1 = new Table(1).SetWidth(UnitValue.CreatePercentValue(100)).SetMarginBottom(15);
                    tBarra1.AddCell(cellBarra1);
                    document.Add(tBarra1);

                   
                    Table tDatos = new Table(UnitValue.CreatePercentArray(new float[] { 40, 60 })).SetWidth(UnitValue.CreatePercentValue(100)).SetMarginBottom(20);

                    string[,] campos = {
                        { "NOMBRES Y APELLIDOS:", e.Nombre?.ToUpper() ?? "" },
                        { "LUGAR Y FECHA DE NACIMIENTO:", e.LugarFechaNacimiento ?? "" },
                        { "CÉDULA DE CIUDADANÍA:", e.Identificacion ?? "" },
                        { "SEXO:", e.Sexo ?? "" },
                        { "ESTADO CIVIL:", e.EstadoCivil ?? "" },
                        { "DIRECCIÓN:", e.Direccion ?? "" },
                        { "TELÉFONO:", e.Telefono ?? "" },
                        { "E-MAIL:", e.Correo ?? "" }
                    };

                    for (int i = 0; i < campos.GetLength(0); i++)
                    {
                        tDatos.AddCell(new Cell().SetBorder(Border.NO_BORDER).SetPaddingBottom(4).Add(new Paragraph(campos[i, 0]).SetFont(fontBld).SetFontSize(10).SetFontColor(ColorConstants.GRAY)));
                        tDatos.AddCell(new Cell().SetBorder(Border.NO_BORDER).SetPaddingBottom(4).Add(new Paragraph(campos[i, 1]).SetFont(fontReg).SetFontSize(11)));
                    }
                    document.Add(tDatos);

                    
                    Cell cellBarra2 = new Cell().SetBackgroundColor(colorGrisBloque).SetBorder(Border.NO_BORDER).SetPadding(5).SetTextAlignment(TextAlignment.CENTER);
                    cellBarra2.Add(new Paragraph("PERFIL PROFESIONAL").SetFont(fontBld).SetFontSize(12).SetFontColor(ColorConstants.WHITE).SetCharacterSpacing(1.5f));

                    Table tBarra2 = new Table(1).SetWidth(UnitValue.CreatePercentValue(100)).SetMarginBottom(10);
                    tBarra2.AddCell(cellBarra2);
                    document.Add(tBarra2);

                   
                    document.Add(new Paragraph(e.PerfilProfesional)
                        .SetFont(fontReg)
                        .SetFontSize(11)
                        .SetTextAlignment(TextAlignment.JUSTIFIED)
                        .SetMultipliedLeading(1.3f)
                        .SetMarginBottom(20));

                   
                    Cell cellBarra3 = new Cell().SetBackgroundColor(colorGrisBloque).SetBorder(Border.NO_BORDER).SetPadding(5).SetTextAlignment(TextAlignment.CENTER);
                    cellBarra3.Add(new Paragraph("FORMACIÓN ACADÉMICA").SetFont(fontBld).SetFontSize(12).SetFontColor(ColorConstants.WHITE).SetCharacterSpacing(1.5f));

                    
                    Table tBarra3 = new Table(1).SetWidth(UnitValue.CreatePercentValue(100)).SetMarginBottom(12);
                    tBarra3.AddCell(cellBarra3);
                    document.Add(tBarra3);

                   
                    foreach (var formacion in e.Formaciones)
                    {
                       
                        Paragraph pEstudio = new Paragraph()
                            .Add(new Text(formacion.Institucion.ToUpper()).SetFont(fontReg).SetFontSize(11))
                            .Add(new Text(" | ").SetFont(fontBld).SetFontSize(11).SetFontColor(ColorConstants.GRAY))
                            .Add(new Text(formacion.Titulo.ToUpper()).SetFont(fontBld).SetFontSize(11))
                            .SetMarginBottom(1);

                        document.Add(pEstudio);
                        document.Add(new Paragraph(formacion.AnioFin.ToString()).SetFont(fontBld).SetFontSize(10).SetFontColor(ColorConstants.GRAY).SetMarginBottom(8));
                    }

                  

                    document.Close();
                }
                return ms.ToArray();
            }
        }

    }
}