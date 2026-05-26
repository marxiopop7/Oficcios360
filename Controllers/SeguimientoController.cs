using Microsoft.AspNetCore.Mvc;
using Oficcios360.Data;
using Oficcios360.Helpers;
using Oficcios360.Models;

namespace Oficcios360.Controllers
{
    public class SeguimientoController : Controller
    {
        private readonly AppDbContext _context;
        private readonly GestorRegistro _gestor;

        public SeguimientoController(AppDbContext context)
        {
            _context = context;
            _gestor = new GestorRegistro();
        }

        
        [HttpGet]
        public IActionResult IngresarCedula()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Index(string cedula)
        {
            if (string.IsNullOrWhiteSpace(cedula))
            {
                return RedirectToAction("IngresarCedula");
            }

            ViewBag.CedulaActual = cedula.Trim();
            List<RegistroActividad> actividades = _context.RegistroActividades
                .Where(a => a.EstudianteIdentificacion == cedula.Trim())
                .ToList();

            double totalHoras = _gestor.SumarHorasAutomaticas(actividades);
            double horasRequeridas = 0;
            bool requiereConfigurarMeta = true;

            
            DateTime mesActualUtc = DateTime.SpecifyKind(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1), DateTimeKind.Utc);
            var informeMeta = _context.Informes
                .FirstOrDefault(i => i.EstudianteIdentificacion == cedula.Trim() && i.PeriodoInicio == mesActualUtc);

            if (informeMeta != null)
            {
                horasRequeridas = informeMeta.HorasRequeridas;

                if (horasRequeridas > 0 && totalHoras >= horasRequeridas)
                {
                    requiereConfigurarMeta = true;
                    ViewBag.MetaCompletadaAviso = true;
                }
                else if (horasRequeridas > 0)
                {
                    requiereConfigurarMeta = false;
                }
            }

            double porcentaje = 0;
            if (horasRequeridas > 0)
            {
                porcentaje = Math.Round((totalHoras / horasRequeridas) * 100, 1);
                if (porcentaje > 100) porcentaje = 100;
            }

            ViewBag.TotalHoras = totalHoras;
            ViewBag.HorasRequeridas = horasRequeridas;
            ViewBag.Porcentaje = porcentaje;
            ViewBag.RequiereMeta = requiereConfigurarMeta;

            return View(actividades);
        }


        [HttpPost]
        public async Task<IActionResult> ConfigurarMeta(string cedula, double horasMeta)
        {
            if (string.IsNullOrWhiteSpace(cedula) || horasMeta <= 0)
            {
                TempData["MensajeError"] = "Error: Cédula o cantidad de horas inválidas.";
                return RedirectToAction("Index");
            }

            DateTime mesActualUtc = DateTime.SpecifyKind(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1), DateTimeKind.Utc);
            
            var informe = _context.Informes
                .FirstOrDefault(i => i.EstudianteIdentificacion == cedula.Trim() && i.PeriodoInicio == mesActualUtc);

            if (informe == null)
            {
                informe = new Informe
                {
                    PeriodoInicio = mesActualUtc,
                    PeriodoFin = mesActualUtc.AddMonths(1).AddDays(-1),
                    EstudianteIdentificacion = cedula.Trim(),
                    HorasRequeridas = horasMeta,
                    Estado = "En Progreso",
                    FechaGeneracion = DateTime.UtcNow
                };
                _context.Informes.Add(informe);
            }
            else
            {
                informe.HorasRequeridas = horasMeta;
                _context.Informes.Update(informe);
            }

            await _context.SaveChangesAsync();
            TempData["MensajeSuccess"] = $"¡Meta de {horasMeta} horas configurada con éxito!";

            return RedirectToAction("Index", new { cedula = cedula });
        }


        [HttpPost]
        public IActionResult RegistrarActividad(RegistroActividad nuevaActividad)
        {
            if (string.IsNullOrWhiteSpace(nuevaActividad.EstudianteIdentificacion))
            {
                TempData["MensajeError"] = "Error: Debes cargar una cédula de estudiante primero.";
                return RedirectToAction("Index");
            }

            if (string.IsNullOrWhiteSpace(nuevaActividad.Descripcion))
            {
                TempData["MensajeError"] = "Error: La descripción es obligatoria.";
                return RedirectToAction("Index", new { cedula = nuevaActividad.EstudianteIdentificacion });
            }

            if (nuevaActividad.HorasDedicadas <= 0 || nuevaActividad.HorasDedicadas > 24)
            {
                TempData["MensajeError"] = "Error: Las horas deben estar entre 0.5 y 24.";
                return RedirectToAction("Index", new { cedula = nuevaActividad.EstudianteIdentificacion });
            }

            if (nuevaActividad.Fecha > DateTime.Today)
            {
                TempData["MensajeError"] = "Error: No puedes registrar fechas futuras.";
                return RedirectToAction("Index", new { cedula = nuevaActividad.EstudianteIdentificacion });
            }

            nuevaActividad.Fecha = DateTime.SpecifyKind(nuevaActividad.Fecha, DateTimeKind.Utc);

            _context.RegistroActividades.Add(nuevaActividad);
            _context.SaveChanges();

            TempData["MensajeSuccess"] = "Actividad registrada con éxito.";
            return RedirectToAction("Index", new { cedula = nuevaActividad.EstudianteIdentificacion });
        }

       [HttpPost]
        public async Task<IActionResult> SubirFirmaTutor(string cedula, string periodoMes, IFormFile archivoFirma)
        {
            if (string.IsNullOrWhiteSpace(cedula) || string.IsNullOrWhiteSpace(periodoMes))
            {
                TempData["MensajeError"] = "Error: Faltan datos obligatorios (Cédula o Mes).";
                return RedirectToAction("Index", new { cedula = cedula });
            }

            if (archivoFirma != null && archivoFirma.Length > 0)
            {
                
                string carpetaFirmas = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "firmas");
                if (!Directory.Exists(carpetaFirmas))
                {
                    Directory.CreateDirectory(carpetaFirmas);
                }

                 string nombreArchivo = $"firma_{cedula.Trim()}_{periodoMes.Trim()}.png";
                string rutaCompletaFisica = Path.Combine(carpetaFirmas, nombreArchivo);

                 
                using (var stream = new FileStream(rutaCompletaFisica, FileMode.Create))
                {
                    await archivoFirma.CopyToAsync(stream);
                }

               
                string rutaBD = $"/images/firmas/{nombreArchivo}";

                
                DateTime inicioMes = DateTime.Parse(periodoMes + "-01");
                DateTime inicioMesUtc = DateTime.SpecifyKind(inicioMes, DateTimeKind.Utc);
                DateTime finMesUtc = DateTime.SpecifyKind(inicioMes.AddMonths(1).AddDays(-1), DateTimeKind.Utc);

                
                var informe = _context.Informes
                    .FirstOrDefault(i => i.EstudianteIdentificacion == cedula && i.PeriodoInicio == inicioMesUtc);

                if (informe == null)
                {
                    informe = new Informe
                    {
                        PeriodoInicio = inicioMesUtc,
                        PeriodoFin = finMesUtc,
                        EstudianteIdentificacion = cedula,
                        RutaFirmaTutor = rutaBD,
                        Estado = "Validado",
                        FechaGeneracion = DateTime.UtcNow
                    };
                    _context.Informes.Add(informe);
                }
                else
                {
                    
                    informe.RutaFirmaTutor = rutaBD;
                    _context.Informes.Update(informe); 
                }

                await _context.SaveChangesAsync(); // Guarda los cambios finales
                TempData["MensajeSuccess"] = "La firma del tutor se ha guardado correctamente para el período seleccionado.";
            }

            return RedirectToAction("Index", new { cedula = cedula });
        }

        
        [HttpGet]
        public IActionResult DescargarPdf(string cedula, DateTime inicio, DateTime fin)
        {
            if (string.IsNullOrWhiteSpace(cedula))
            {
                TempData["MensajeError"] = "Error: Cédula requerida.";
                return RedirectToAction("Index");
            }

            
            DateTime inicioUtc = DateTime.SpecifyKind(inicio.Date, DateTimeKind.Utc);
            DateTime finUtc = DateTime.SpecifyKind(fin.Date.AddDays(1).AddTicks(-1), DateTimeKind.Utc);

            var actividades = _context.RegistroActividades
                .Where(a => a.EstudianteIdentificacion == cedula && a.Fecha >= inicioUtc && a.Fecha <= finUtc)
                .ToList();

            double totalHoras = _gestor.SumarHorasAutomaticas(actividades);


            var informePersistido = _context.Informes
                .AsEnumerable()
                .FirstOrDefault(i => i.EstudianteIdentificacion == cedula
                                  && i.PeriodoInicio.Year == inicio.Year
                                  && i.PeriodoInicio.Month == inicio.Month);

            
            string rutaFirmaParaPdf = null;

            if (informePersistido != null && !string.IsNullOrEmpty(informePersistido.RutaFirmaTutor))
            {
               
                string rutaRelativaLimpia = informePersistido.RutaFirmaTutor.Replace("/", "\\").TrimStart('\\');
                string rutaAbsolutaSistema = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", rutaRelativaLimpia);

                
                if (System.IO.File.Exists(rutaAbsolutaSistema))
                {
                    rutaFirmaParaPdf = rutaAbsolutaSistema;
                }
            }

             
            Informe informeTemporal = new Informe
            {
                PeriodoInicio = inicioUtc,
                PeriodoFin = finUtc,
                EstudianteIdentificacion = cedula,
                RutaFirmaTutor = rutaFirmaParaPdf 
            };

            
            Exportacion exportador = new Exportacion();
            byte[] pdfBytes = exportador.GenerarReporteSeguimientoPdf(informeTemporal, actividades, totalHoras);

            return File(pdfBytes, "application/pdf", $"Reporte_Practicas_{cedula}_{inicio:yyyyMMdd}.pdf");
        }
    }
}