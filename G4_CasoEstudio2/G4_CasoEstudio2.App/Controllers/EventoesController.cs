using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using G4_CasoEstudio2.App.Models;
using System.Security.Claims;

namespace G4_CasoEstudio2.App.Controllers
{
    public class EventoesController : Controller
    {
        private readonly Contexto _context;

        public EventoesController(Contexto context)
        {
            _context = context;
        }

        // GET: Eventoes
        //[Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Index()
        {
            var contexto = _context.Eventos.Include(e => e.Categoria).Include(e => e.Usuario);
            return View(await contexto.ToListAsync());
        }

        // GET: Eventoes/Details/5
        //[Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evento = await _context.Eventos
                .Include(e => e.Categoria)
                .Include(e => e.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (evento == null)
            {
                return NotFound();
            }

            return View(evento);
        }

        // GET: Eventoes/Create
        //[Authorize(Roles = "Administrador")]
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Descripcion");
            ViewData["UsuarioRegistro"] = new SelectList(_context.Usuarios, "Id", "Contraseña");
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
                _context.Add(evento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Descripcion", evento.CategoriaId);
            ViewData["UsuarioRegistro"] = new SelectList(_context.Usuarios, "Id", "Contraseña", evento.UsuarioRegistro);
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

            var evento = await _context.Eventos.FindAsync(id);
            if (evento == null)
            {
                return NotFound();
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Descripcion", evento.CategoriaId);
            ViewData["UsuarioRegistro"] = new SelectList(_context.Usuarios, "Id", "Contraseña", evento.UsuarioRegistro);
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
                    _context.Update(evento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventoExists(evento.Id))
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
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Descripcion", evento.CategoriaId);
            ViewData["UsuarioRegistro"] = new SelectList(_context.Usuarios, "Id", "Contraseña", evento.UsuarioRegistro);
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

            var evento = await _context.Eventos
                .Include(e => e.Categoria)
                .Include(e => e.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
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
            var evento = await _context.Eventos.FindAsync(id);
            if (evento != null)
            {
                _context.Eventos.Remove(evento);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventoExists(int id)
        {
            return _context.Eventos.Any(e => e.Id == id);
        }

        /*Lista de eventos filtrados por usuario*/
        //[Authorize(Roles = "Organizador")]
        public async Task<IActionResult> EventosXOrganizador()
        {            
            var contexto = _context.Eventos.Include(e => e.Categoria)
                .Include(e => e.Usuario)
                .Where(co => co.Usuario.Correo == User.Identity.Name);
            return View(await contexto.ToListAsync());
        }
    }
}
