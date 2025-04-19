using G4_CasoEstudio2.App.Models;
using Microsoft.EntityFrameworkCore;

namespace G4_CasoEstudio2.App.Services
{
    public class CategoriaServices : ICategoriaServices
    {

        private readonly Contexto _contexto;

        public CategoriaServices(Contexto contexto)
        {
            _contexto = contexto;
        }

        public async Task<Categoria> BuscarXid(int? id)
        {
            var buscarCategorias = await _contexto.Categorias.FindAsync(id);
            return buscarCategorias;
        }

        public async Task<bool> CategoriaExists(int id)
        {
            await _contexto.Categorias.AnyAsync(e => e.Id == id);
            return true;
        }

        public async Task<bool> Crear(Categoria categoria)
        {
            try
            {
                // Validación rápida
                if (string.IsNullOrEmpty(categoria.UsuarioRegistro))
                    return false;

                _contexto.Categorias.Add(categoria);
                return await _contexto.SaveChangesAsync() > 0;
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine($"Error de BD: {dbEx.InnerException?.Message}");
                return false;
            }
        }

        public async Task<bool> Eliminar(int id)
        {
            try
            {
                var categoria = await BuscarXid(id);
                if (categoria != null)
                {
                    _contexto.Categorias.Remove(categoria);
                }
                await _contexto.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<IEnumerable<Categoria>> Listar()
        {
            var lista = await _contexto.Categorias.ToListAsync();
            return lista;
        }

        public async Task<bool> Modificar(Categoria categoria)
        {
            try
            {
                // Marcar solo los campos modificados como cambiados
                _contexto.Attach(categoria);
                _contexto.Entry(categoria).Property(x => x.Nombre).IsModified = true;
                _contexto.Entry(categoria).Property(x => x.Descripcion).IsModified = true;
                _contexto.Entry(categoria).Property(x => x.Estado).IsModified = true;

                return await _contexto.SaveChangesAsync() > 0;
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine($"Error al modificar: {dbEx.InnerException?.Message}");
                return false;
            }
        }
    }
}
