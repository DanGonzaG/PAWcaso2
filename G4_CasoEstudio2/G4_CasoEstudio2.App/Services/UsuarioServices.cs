using G4_CasoEstudio2.App.Models;
using Microsoft.EntityFrameworkCore;

namespace G4_CasoEstudio2.App.Services
{
    public class UsuarioServices : IUsuarioServices
    {

        private readonly Contexto _contexto;

        public UsuarioServices(Contexto contexto)
        {
            _contexto = contexto;
        }

        public async Task<Usuario> BuscarXid(int? id)
        {
            var buscarUsuario = await _contexto.Usuarios.FindAsync(id);
            return buscarUsuario;
        }

        public async Task<bool> Crear(Usuario usuario)
        {
            try
            {
                _contexto.Usuarios.Add(usuario);
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
                var usuario = await BuscarXid(id);
                if (usuario != null)
                {
                    _contexto.Usuarios.Remove(usuario);
                }
                await _contexto.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<IEnumerable<Usuario>> Listar()
        {
            var lista = await _contexto.Usuarios.ToListAsync();
            return lista;
        }

        public async Task<bool> Modificar(Usuario usuario)
        {
            try
            {
                _contexto.Update(usuario);
                await _contexto.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UsuarioExists(string id)
        {
            await _contexto.Usuarios.AnyAsync(e => e.Id == id);
            return true;
        }

        public Task<bool> UsuarioExists(int id)
        {
            throw new NotImplementedException();
        }


    }
}
