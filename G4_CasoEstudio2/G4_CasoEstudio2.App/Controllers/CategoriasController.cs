using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using G4_CasoEstudio2.App.Models;
using G4_CasoEstudio2.App.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace G4_CasoEstudio2.App.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class CategoriasController : Controller
    {
        private readonly ICategoriaServices _categoria;
        private readonly UserManager<Usuario> _userManager;



        public CategoriasController(ICategoriaServices categoria, UserManager<Usuario> userManager)
        {
            _categoria = categoria;
            _userManager = userManager;
        }

        // GET: Categorias
        [Authorize(Roles = "Administrador, Organizador")]
        public async Task<IActionResult> Index()
        {
            return View(await _categoria.Listar());
        }

        // GET: Categorias/Details/5
        //[Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Details(int? id)
        {
            var categoria = await _categoria.BuscarXid(id);
            return View(categoria);
        }

        // GET: Categorias/Create
        //[Authorize(Roles = "Administrador")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categorias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,Descripcion,Estado")] Categoria categoria)
        {
            // Asignación de valores automáticos
            categoria.FechaRegistro = DateTime.Now;
            categoria.UsuarioRegistro = _userManager.GetUserId(User);

            // Limpieza de ModelState
            ModelState.Remove("FechaRegistro");
            ModelState.Remove("UsuarioRegistro");
            ModelState.Remove("Usuario");

            if (ModelState.IsValid)
            {
                try
                {
                    var resultado = await _categoria.Crear(categoria);
                    if (resultado)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    ModelState.AddModelError(string.Empty, "Error al guardar la categoría");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Error interno: {ex.Message}");
                    Console.WriteLine($"Error al crear categoría: {ex.ToString()}");
                }
            }

            return View(categoria);
        }


        // GET: Categorias/Edit/5
        [Authorize(Roles = "Administrador, Organizador")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _categoria.BuscarXid(id);
            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }

        // POST: Categorias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Descripcion,Estado")] Categoria categoria)
        {
            if (id != categoria.Id)
            {
                return NotFound();
            }

            // Obtener la categoría existente
            var categoriaExistente = await _categoria.BuscarXid(id);
            if (categoriaExistente == null)
            {
                return NotFound();
            }

            // Actualizar solo los campos editables
            categoriaExistente.Nombre = categoria.Nombre;
            categoriaExistente.Descripcion = categoria.Descripcion;
            categoriaExistente.Estado = categoria.Estado;

            // Limpieza COMPLETA del ModelState (igual que en Create)
            ModelState.Remove("FechaRegistro");
            ModelState.Remove("UsuarioRegistro");
            ModelState.Remove("Usuario");

            if (ModelState.IsValid)
            {
                try
                {
                    // Usar el mismo enfoque que en Create
                    _contexto.Entry(categoriaExistente).State = EntityState.Modified;

                    // Especificar que no debe modificar estos campos
                    _contexto.Entry(categoriaExistente).Property(x => x.FechaRegistro).IsModified = false;
                    _contexto.Entry(categoriaExistente).Property(x => x.UsuarioRegistro).IsModified = false;

                    await _contexto.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _categoria.CategoriaExists(categoria.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
            }

            return View(categoria);
        }

        // GET: Categorias/Delete/5
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _categoria.BuscarXid(id);
            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        // POST: Categorias/Delete/5
        [Authorize(Roles = "Administrador")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _categoria.Eliminar(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
