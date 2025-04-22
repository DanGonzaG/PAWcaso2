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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;

namespace G4_CasoEstudio2.App.Controllers
{
    public class EventoesController : Controller
    {
        private readonly IEventoServices _evento;
        private readonly ICategoriaServices _categoria;
        private readonly UserManager<Usuario> _userManager;
        private readonly Contexto _contexto;

        public EventoesController(IEventoServices evento, ICategoriaServices categoria, UserManager<Usuario> userManager, Contexto contexto)
        {
            _evento = evento;
            _categoria = categoria;
            _userManager = userManager;
            _contexto = contexto;
        }

        // GET: Eventoes
        [Authorize]
        public async Task<IActionResult> Index()
        {

            var usuarioActual = await _userManager.GetUserAsync(User);

            if (User.IsInRole("Administrador"))
            {
                // Administrador ve todos los eventos
                return View(await _evento.Listar());
            }
            else if (User.IsInRole("Organizador"))
            {
                // Organizador ve solo sus eventos
                var eventosDelOrganizador = await _contexto.Eventos
                    .Where(e => e.UsuarioRegistro == usuarioActual.Id)
                    .ToListAsync();

                return View(eventosDelOrganizador);
            }

            // Otros roles (si los hay) pueden ver solo eventos activos
            var eventosPublicos = await _contexto.Eventos
                .Where(e => e.Estado)
                .ToListAsync();

            return View(eventosPublicos);

        }

        // GET: Eventoes/Details/5
        //[Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evento = await _contexto.Eventos
                .Include(e => e.Usuario)
                .Include(e => e.Asistencias)
                    .ThenInclude(a => a.Usuario)
                .Include(e => e.Categoria)
                .FirstOrDefaultAsync(e => e.Id == id);

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
            var categorias = _categoria.Listar().Result;
            ViewBag.CategoriaId = new SelectList(categorias, "Id", "Nombre");
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
            evento.FechaRegistro = DateTime.Now;
            evento.UsuarioRegistro = _userManager.GetUserId(User);
            ModelState.Remove("Categoria");
            ModelState.Remove("UsuarioRegistro");
            ModelState.Remove("Usuario");
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
            var categorias = _categoria.Listar().Result;
            ViewBag.CategoriaId = new SelectList(categorias, "Id", "Nombre");
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Descripcion,Fecha,Hora,Duration,Ubicacion,CupoMaximo,Estado,CategoriaId")] Evento evento)
        {
            ModelState.Remove("Usuario");
            ModelState.Remove("Categoria");
            ModelState.Remove("UsuarioRegistro");
            if (id != evento.Id)
            {
                return NotFound();
            }
            
            if (ModelState.IsValid)
            {
                try
                {

                    // Obtener el evento actual de la base de datos
                    var eventoExistente = await _evento.BuscarXid(id);

                    if (eventoExistente == null)
                    {
                        return NotFound();
                    }

                    // Actualizar solo los campos permitidos
                    eventoExistente.Titulo = evento.Titulo;
                    eventoExistente.Descripcion = evento.Descripcion;
                    eventoExistente.Fecha = evento.Fecha;
                    eventoExistente.Hora = evento.Hora;
                    eventoExistente.Duration = evento.Duration;
                    eventoExistente.Ubicacion = evento.Ubicacion;
                    eventoExistente.CupoMaximo = evento.CupoMaximo;
                    eventoExistente.Estado = evento.Estado;
                    eventoExistente.CategoriaId = evento.CategoriaId;

                    // Guardar el usuario que está modificando
                    eventoExistente.UsuarioRegistro = _userManager.GetUserId(User);
                    eventoExistente.FechaRegistro = DateTime.Now;

                    await _evento.Modificar(eventoExistente);
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
            // Si el modelo no es válido, recargar las categorías para el dropdown
            ViewBag.CategoriaId = new SelectList(await _categoria.Listar(), "Id", "Nombre", evento.CategoriaId);
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
