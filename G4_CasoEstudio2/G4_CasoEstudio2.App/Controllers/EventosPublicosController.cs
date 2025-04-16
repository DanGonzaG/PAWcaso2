using G4_CasoEstudio2.App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace G4_CasoEstudio2.App.Controllers
{
    public class EventosPublicosController : Controller
    {
        private readonly Contexto _context;

        public EventosPublicosController(Contexto context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var eventos = await _context.Eventos
                .Include(e => e.Categoria)
                .Where(e => e.Estado == "Activo")
                .ToListAsync();

            return View(eventos);
        }

        // 🔎 Nuevo método para mostrar detalle de un evento
        public async Task<IActionResult> Detalle(int id)
        {
            var evento = await _context.Eventos
                .Include(e => e.Categoria)
                .FirstOrDefaultAsync(e => e.Id == id && e.Estado == "Activo");

            if (evento == null)
            {
                return NotFound();
            }

            return View(evento);
        }
    }
}
