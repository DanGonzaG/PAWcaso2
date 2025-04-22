using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using G4_CasoEstudio2.App.Models;
using Microsoft.AspNetCore.Authorization;
using G4_CasoEstudio2.App.Services;
using System.Security.Claims;

namespace G4_CasoEstudio2.App.Controllers
{
    public class AsistenciasController : Controller
    {
        private readonly IAsistenciaServices _asistencia;
        private readonly Contexto _context;

        public AsistenciasController(IAsistenciaServices asistencia, Contexto contexto)
        {
            _asistencia = asistencia;
            _context = contexto;
        }


        // GET: Asistencias

        [Authorize] // Asegura que solo usuarios autenticados puedan acceder
        public async Task<IActionResult> Index()
        {
            IQueryable<Asistencia> query = _context.Asistencias;

            // Filtrado por rol
            if (User.IsInRole("Organizador"))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                query = query.Where(a => a.Evento.UsuarioRegistro == userId);
            }
            else if (!User.IsInRole("Administrador"))
            {
                // Usuario normal solo ve sus propias asistencias
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                query = query.Where(a => a.UsuarioId == userId);
            }

            // Carga relacionada optimizada con Include
            query = query.Include(a => a.Evento);

            if (User.IsInRole("Administrador") || User.IsInRole("Organizador") || !User.IsInRole("Organizador"))
            {
                query = query.Include(a => a.Usuario);
            }

            var asistencias = await query.ToListAsync();
            return View(asistencias);
        }


        // GET: Asistencias/Details/5
        //[Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Details(int? id)
        {
            var asistencia = await _asistencia.BuscarXid(id);
            return View(asistencia);
        }


        // GET: Asistencias/Create
        //[Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create()
        {
            var eventosDisponibles = await _context.Eventos
                .Where(e => e.Estado)
                .ToListAsync();

            ViewBag.EventoId = new SelectList(eventosDisponibles, "Id", "Titulo");
            return View();
        }


        // POST: Asistencias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventoId")] Asistencia asistencia)
        {
            ModelState.Remove("Evento");
            ModelState.Remove("Usuario");
            ModelState.Remove("UsuarioId");

            if (ModelState.IsValid)
            {
                try
                {
                    var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                    if (string.IsNullOrEmpty(userId))
                    {
                        return Forbid(); // o redirigir al login si es necesario
                    }

                    asistencia.UsuarioId = userId;

                    // Validar cupo
                    var evento = await _context.Eventos.FindAsync(asistencia.EventoId);
                    if (evento == null || !evento.Estado)
                    {
                        ModelState.AddModelError("", "El evento no existe o no está disponible.");
                    }
                    else
                    {
                        var registrados = await _context.Asistencias
                            .CountAsync(a => a.EventoId == asistencia.EventoId);

                        if (registrados >= evento.CupoMaximo)
                        {
                            ModelState.AddModelError("", "No hay cupos disponibles para este evento.");
                        }
                        else
                        {
                            // Validar si ya se registró antes este usuario
                            bool yaRegistrado = await _context.Asistencias
                                .AnyAsync(a => a.EventoId == asistencia.EventoId && a.UsuarioId == userId);

                            if (yaRegistrado)
                            {
                                ModelState.AddModelError("", "Ya estás registrado para este evento.");
                            }
                            else
                            {
                                await _asistencia.crear(asistencia);
                                return RedirectToAction(nameof(Index));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            // Recarga el ViewBag por si la vista necesita volver a mostrarse
            var eventosDisponibles = await _context.Eventos
                .Where(e => e.Estado)
                .ToListAsync();

            ViewBag.EventoId = new SelectList(eventosDisponibles, "Id", "Titulo", asistencia.EventoId);

            return View(asistencia);
        }



        // GET: Asistencias/Edit/5
        //[Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asistencia = await _asistencia.BuscarXid(id);
            if (asistencia == null)
            {
                return NotFound();
            }
            return View(asistencia);
        }

        // POST: Asistencias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EventoId,UsuarioId")] Asistencia asistencia)
        {
            if (id != asistencia.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    await _asistencia.Modificar(asistencia);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _asistencia.AsistenciaExists(asistencia.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(asistencia);
        }

        // GET: Asistencias/Delete/5
        //[Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asistencia = await _asistencia.BuscarXid(id);
            if (asistencia == null)
            {
                return NotFound();
            }

            return View(asistencia);
        }

        // POST: Asistencias/Delete/5
        //[Authorize(Roles = "Administrador")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _asistencia.Eliminar(id);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Organizador")]
        public async Task<IActionResult> MisInscritos()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var asistencias = await _asistencia.ListarPorOrganizador(userId);
            return View("Index", asistencias); // Puedes usar la misma vista Index
        }
    }
}
