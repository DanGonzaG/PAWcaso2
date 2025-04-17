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

namespace G4_CasoEstudio2.App.Controllers
{
    public class AsistenciasController : Controller
    {
        private readonly IAsistenciaServices _asistencia;

        public AsistenciasController(IAsistenciaServices asistencia)
        {
            _asistencia = asistencia;
        }


        // GET: Asistencias
        //[Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Index()
        {
            return View(await _asistencia.Listar());
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
        public IActionResult Create()
        {
            return View();
        }


        // POST: Asistencias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EventoId,UsuarioId")] Asistencia asistencia)
        {
            if (ModelState.IsValid)
            {

                await _asistencia.crear(asistencia);
                return RedirectToAction(nameof(Index));
            }
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
    }
}
