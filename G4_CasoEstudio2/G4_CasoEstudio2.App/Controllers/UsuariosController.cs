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
using Microsoft.AspNetCore.Authentication;

namespace G4_CasoEstudio2.App.Controllers
{
    [Authorize]
    public class UsuariosController : Controller
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly ILogger<UsuariosController> _logger;

        public UsuariosController(
            UserManager<Usuario> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<UsuariosController> logger,
            SignInManager<Usuario> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
            _signInManager = signInManager;
        }

        // GET: Usuarios
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Index()
        {
            var usuarios = await _userManager.Users.ToListAsync();
            return View(usuarios);
        }

        // GET: Usuarios/Details/5
        [Authorize]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            ViewBag.Roles = await _userManager.GetRolesAsync(usuario);
            return View(usuario);
        }

        // GET: Usuarios/Create
        //[Authorize(Roles = "Administrador")]
        public IActionResult Create()
        {
            ViewBag.Roles = new SelectList(_roleManager.Roles, "Name", "Name");
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NombreUsuario,Correo,Telefono,Contraseña,Rol")] Usuario usuario, string contraseña, string rol)
        {
            if (ModelState.IsValid)
            {
                usuario.UserName = usuario.Email;

                var result = await _userManager.CreateAsync(usuario, contraseña);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(usuario, rol);
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            ViewBag.Roles = new SelectList(_roleManager.Roles, "Name", "Name");
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(usuario);
            ViewBag.CurrentRole = roles.FirstOrDefault();
            ViewBag.Roles = new SelectList(_roleManager.Roles, "Name", "Name");

            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id,
            [Bind("Id,NombreCompleto,Email,PhoneNumber")] Usuario usuario,
            string rol)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                user.NombreCompleto = usuario.NombreCompleto;
                user.Email = usuario.Email;
                user.UserName = usuario.Email;
                user.PhoneNumber = usuario.PhoneNumber;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    // Actualizar rol
                    var currentRoles = await _userManager.GetRolesAsync(user);
                    await _userManager.RemoveFromRolesAsync(user, currentRoles);
                    await _userManager.AddToRoleAsync(user, rol);

                    return RedirectToAction("Details", new { id = user.Id });
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            ViewBag.Roles = new SelectList(_roleManager.Roles, "Name", "Name");
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            ViewBag.Roles = await _userManager.GetRolesAsync(usuario);
            return View(usuario);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                // Validación básica
                if (string.IsNullOrEmpty(id))
                {
                    return NotFound();
                }

                // Obtener usuario a eliminar
                var userToDelete = await _userManager.FindByIdAsync(id);
                if (userToDelete == null)
                {
                    return NotFound();
                }

                // Verificar si es el usuario actual
                var currentUserId = _userManager.GetUserId(User);
                bool isCurrentUser = currentUserId == id;

                // Eliminar el usuario
                var deleteResult = await _userManager.DeleteAsync(userToDelete);

                if (!deleteResult.Succeeded)
                {
                    // Manejar errores de eliminación
                    foreach (var error in deleteResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View("Delete", userToDelete);
                }

                // Si es el usuario actual, cerrar sesión
                if (isCurrentUser)
                {
                    try
                    {
                        // Cerrar sesión de forma segura
                        await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
                        HttpContext.Session.Clear();

                        // Eliminar cookies de autenticación
                        Response.Cookies.Delete(".AspNetCore.Identity.Application");
                        Response.Cookies.Delete(".AspNetCore.Session");

                        // Redirigir a Home/Index
                        return RedirectToAction("Index", "Home");
                    }
                    catch (Exception signOutEx)
                    {
                        _logger?.LogError(signOutEx, "Error al cerrar sesión después de eliminar usuario");
                        // Aunque falle el cierre de sesión, redirigir a Home
                        return RedirectToAction("Index", "Home");
                    }
                }

                // Redirigir a la lista de usuarios si no es el usuario actual
                return RedirectToAction("Index", "Usuarios");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error crítico al eliminar usuario con ID {UserId}", id);
                // Redirigir a página de error con mensaje específico
                TempData["ErrorMessage"] = "Ocurrió un error al procesar tu solicitud.";
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
