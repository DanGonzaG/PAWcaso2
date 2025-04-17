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
                _contexto.Categorias.Add(categoria);
                await _contexto.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
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
                _contexto.Update(categoria);
                await _contexto.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
