
using G4_CasoEstudio2.App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace G4_CasoEstudio2.App.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class DashboardController : Controller
    {
        private readonly Contexto _context;

        public DashboardController(Contexto context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var totalUsuarios = await _context.Usuarios.CountAsync();
            var totalEventos = await _context.Eventos.CountAsync();
            var totalAsistencias = await _context.Asistencias.CountAsync();

            var topEventos = await _context.Asistencias
                .GroupBy(a => a.Evento)
                .Select(g => new
                {
                    Evento = g.Key,
                    Total = g.Count()
                })
                .OrderByDescending(g => g.Total)
                .Take(5)
                .ToListAsync();

            ViewBag.TotalUsuarios = totalUsuarios;
            ViewBag.TotalEventos = totalEventos;
            ViewBag.TotalAsistencias = totalAsistencias;
            ViewBag.TopEventos = topEventos;

            return View();
        }
    }
}
