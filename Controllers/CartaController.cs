using Microsoft.AspNetCore.Mvc;
using Oficcios360.Data;
using Oficcios360.Helpers;
using Oficcios360.Models;

namespace Oficcios360.Controllers
{
    public class CartaController : Controller
    {
        private readonly AppDbContext _context;

        public CartaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(Estudiante modeloRecuperado)
        {
            // Recarga la vista con los datos previamente escritos
            return View(modeloRecuperado);
        }

        [HttpPost]
        public IActionResult Previsualizar(Estudiante modelo)
        {
            Validacion validador = new Validacion();

            if (!validador.ValidarCamposCarta(modelo))
            {
                ViewBag.Error = "Verifique los datos ingresados. El nombre solo debe contener letras, teléfono y cédula solo números, y el perfil profesional debe tener más de 20 caracteres."; ViewBag.Error = "Error de formato: El nombre solo debe tener letras, y la cédula/teléfono solo números.";
                return View("Index", modelo);
            }

            // GUARDAR DATOS EN LA BASE DE DATOS
            Carta nuevaCarta = new Carta
            {
                Nombre = modelo.Nombre.ToUpper(),
                Identificacion = modelo.Identificacion,
                Telefono = modelo.Telefono,
                ModalidadGrado = modelo.ModalidadGrado,
                PerfilProfesional = modelo.PerfilProfesional,
                FechaGeneracion = DateTime.UtcNow
            };

            _context.Cartas.Add(nuevaCarta);
            _context.SaveChanges();

            return View("Previsualizacion", modelo);
        }

        [HttpPost]
        public IActionResult GenerarPdfFinal(string contenido)
        {
            ExportarPDF exportador = new ExportarPDF();

            byte[] pdfBytes = exportador.GenerarCartaPdf(contenido);

            return File(pdfBytes,
                        "application/pdf",
                        "Carta_Presentacion.pdf");
        }
    }
}