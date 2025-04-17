using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using G4_CasoEstudio2.App.Models;
using System.Security.Claims;
using G4_CasoEstudio2.App.Services;

namespace G4_CasoEstudio2.App.Controllers
{
    public class EventoesController : Controller
    {
        private readonly IEventoServices _evento;

        public EventoesController(IEventoServices evento)
        {
            _evento = evento;
        }

        // GET: Eventoes
        //[Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Index()
        {
            return View(await _evento.Listar());
        }

        // GET: Eventoes/Details/5
        //[Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Details(int? id)
        {
            var evento = await _evento.BuscarXid(id);
            return View(evento);
        }

        // GET: Eventoes/Create
        //[Authorize(Roles = "Administrador")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Eventoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Titulo,Descripcion,Fecha,Hora,Duration,Ubicacion,CupoMaximo,Estado,FechaRegistro,UsuarioRegistro,CategoriaId")] Evento evento)
        {
            if (ModelState.IsValid)
            {
                await _evento.Crear(evento);
                return RedirectToAction(nameof(Index));
            }
            return View(evento);
        }

        // GET: Eventoes/Edit/5
        //[Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evento = await _evento.BuscarXid(id);
            if (evento == null)
            {
                return NotFound();
            }
            return View(evento);
        }

        // POST: Eventoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Descripcion,Fecha,Hora,Duration,Ubicacion,CupoMaximo,Estado,FechaRegistro,UsuarioRegistro,CategoriaId")] Evento evento)
        {
            if (id != evento.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    await _evento.Modificar(evento);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _evento.EventoExists(evento.Id))
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
            return View(evento);
        }

        // GET: Eventoes/Delete/5
        //[Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evento = await _evento.BuscarXid(id);
            if (evento == null)
            {
                return NotFound();
            }

            return View(evento);
        }

        // POST: Eventoes/Delete/5
        //[Authorize(Roles = "Administrador")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _evento.Eliminar(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
