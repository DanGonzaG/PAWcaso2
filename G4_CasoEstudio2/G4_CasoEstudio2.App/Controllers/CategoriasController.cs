using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using G4_CasoEstudio2.App.Models;
using G4_CasoEstudio2.App.Services;

namespace G4_CasoEstudio2.App.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly ICategoriaServices _categoria;

        public CategoriasController(ICategoriaServices categoria)
        {
            _categoria = categoria;
        }

        // GET: Categorias
        //[Authorize(Roles = "Administrador")]
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
        public async Task<IActionResult> Create([Bind("Id,Nombre,Descripcion,Estado,FechaRegistro,UsuarioRegistro")] Categoria categoria)
        {
            if (ModelState.IsValid)
            {

                await _categoria.Crear(categoria);
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        // GET: Categorias/Edit/5
        //[Authorize(Roles = "Administrador")]
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Descripcion,Estado,FechaRegistro,UsuarioRegistro")] Categoria categoria)
        {
            if (id != categoria.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    await _categoria.Modificar(categoria);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _categoria.CategoriaExists(categoria.Id))
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
            return View(categoria);
        }

        // GET: Categorias/Delete/5
        //[Authorize(Roles = "Administrador")]
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
        //[Authorize(Roles = "Administrador")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _categoria.Eliminar(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
