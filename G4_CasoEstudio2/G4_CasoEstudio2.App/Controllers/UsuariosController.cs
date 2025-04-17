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
    public class UsuariosController : Controller
    {
        private readonly IUsuarioServices _usuario;

        public UsuariosController(IUsuarioServices usuario)
        {
            _usuario = usuario;
        }

        // GET: Usuarios
        //[Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Index()
        {
            return View(await _usuario.Listar());
        }

        // GET: Usuarios/Details/5
        //[Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Details(int? id)
        {
            var usuario = await _usuario.BuscarXid(id);
            return View(usuario);
        }

        // GET: Usuarios/Create
        //[Authorize(Roles = "Administrador")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NombreUsuario,Correo,Telefono,Contraseña,Rol")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                await _usuario.Crear(usuario);
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        //[Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _usuario.BuscarXid(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NombreUsuario,Correo,Telefono,Contraseña,Rol")] Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    await _usuario.Modificar(usuario);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _usuario.UsuarioExists(usuario.Id))
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
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        //[Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _usuario.BuscarXid(id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        //[Authorize(Roles = "Administrador")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _usuario.Eliminar(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
