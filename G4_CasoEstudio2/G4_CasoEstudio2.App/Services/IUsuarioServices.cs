using G4_CasoEstudio2.App.Models;

namespace G4_CasoEstudio2.App.Services
{
    public interface IUsuarioServices
    {
        Task<bool> Crear(Usuario usuario);

        Task<Usuario> BuscarXid(int? id);

        Task<bool> Modificar(Usuario usuario);

        Task<bool> Eliminar(int id);

        Task<IEnumerable<Usuario>> Listar();

        Task<bool> UsuarioExists(int id);

       
    }
}
