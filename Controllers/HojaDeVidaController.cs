using Microsoft.AspNetCore.Mvc;
using Oficcios360.Models;
using Oficcios360.Helpers;
using Oficcios360.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Oficcios360.Controllers
{
    public class HojaDeVidaController : Controller
    {
        private readonly Validacion _validador = new Validacion();
        private readonly GestorRegistro _gestor = new GestorRegistro();
        private readonly ExportarPDF _exportador = new ExportarPDF();

        
        private readonly AppDbContext _context;

        public HojaDeVidaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Registrar(Estudiante? estudianteAnterior)
        {
            
            if (estudianteAnterior == null || string.IsNullOrEmpty(estudianteAnterior.Identificacion))
            {
                TempData.Remove("MensajeConfirmacion");
            }

            return View(estudianteAnterior ?? new Estudiante());
        }

        [HttpPost]
        public async Task<IActionResult> ProcesarAccion(Estudiante modeloFormulario, string botonAccion)
        {
            
            List<string> erroresEncontrados = _validador.ValidarCamposHojaDeVida(modeloFormulario);

            if (erroresEncontrados.Count > 0)
            {
                ViewBag.AlertasValidacion = erroresEncontrados;
                
                return View("Registrar", modeloFormulario);
            }

            
            Estudiante estudianteEstructurado = _gestor.OrganizarEstructuraHojaVida(modeloFormulario);

            
            if (botonAccion == "Modificar")
            {
                return View("Registrar", estudianteEstructurado);
            }

            
            if (string.IsNullOrWhiteSpace(estudianteEstructurado.Identificacion))
            {
                ViewBag.AlertasValidacion = new List<string> { "Error interno: La identificación del estudiante es requerida para almacenar los datos." };
                return View("Registrar", modeloFormulario);
            }

            try
            {
                
                var registrosPrevios = _context.FormacionesAcademicas
                    .Where(f => f.EstudianteId == estudianteEstructurado.Identificacion);

                _context.FormacionesAcademicas.RemoveRange(registrosPrevios);
                await _context.SaveChangesAsync();

                
                foreach (var formacion in estudianteEstructurado.Formaciones)
                {
                    formacion.EstudianteId = estudianteEstructurado.Identificacion; 
                    _context.FormacionesAcademicas.Add(formacion);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ViewBag.AlertasValidacion = new List<string> { $"Error al guardar en la base de datos: {ex.Message}" };
                return View("Registrar", modeloFormulario);
            }

            
            if (botonAccion == "DescargarPDF")
            {
                byte[] pdfBytes = _exportador.GenerarHojaVidaPdf(estudianteEstructurado);
                
                string nombreDocumento = $"HojaDeVida_{estudianteEstructurado.Nombre?.Replace(" ", "_")}.pdf";

                return File(pdfBytes, "application/pdf", nombreDocumento);
            }

            
            TempData["MensajeConfirmacion"] = "Información guardada con éxito en PostgreSQL y estructurada profesionalmente.";
            return View("VistaPrevia", estudianteEstructurado);
        }

        [HttpGet]
        public IActionResult SalirYLimpiar()
        {
            
            TempData.Remove("MensajeConfirmacion");

            return RedirectToAction("Registrar");
        }
    }
}