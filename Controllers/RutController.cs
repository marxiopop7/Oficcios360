using Microsoft.AspNetCore.Mvc;
using Oficcios360.Data;
using Oficcios360.Models;

namespace Oficcios360.Controllers
{
    public class RutController : Controller
    {
        private readonly AppDbContext _context;

        public RutController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var info = new Instrucciones
            {
                DescripcionPasoAPaso = "1. Revise los requisitos. 2. Haga clic en el botón para abrir la DIAN. 3. Siga las instrucciones en la ventana emergente."
            };

            return View(info);
        }
        public IActionResult ObtenerEncuestas()
        {
            var encuestas = _context.Encuestas
                .OrderByDescending(e => e.Fecha)
                .ToList();

            return PartialView("_PanelEncuestas", encuestas);
        }
        public IActionResult Encuesta()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GuardarEncuesta(string rating, string completado, string comentario)
        {
            Encuesta nuevaEncuesta = new Encuesta
            {
                Rating = rating,
                Completado = completado,
                Comentario = comentario
            };

            _context.Encuestas.Add(nuevaEncuesta);
            _context.SaveChanges();

            TempData["MensajeGracias"] = "¡Muchas gracias por su respuesta!";

            return RedirectToAction("Index");
        } 
        public IActionResult PanelEncuestas()
        {
            var encuestas = _context.Encuestas
                .OrderByDescending(e => e.Fecha)
                .ToList();

            return View(encuestas);
        }
    }
}