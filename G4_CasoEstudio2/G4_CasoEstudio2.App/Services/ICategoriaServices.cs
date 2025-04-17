using G4_CasoEstudio2.App.Models;

namespace G4_CasoEstudio2.App.Services
{
    public interface ICategoriaServices
    {
        Task<bool> Crear(Categoria categoria);

        Task<Categoria> BuscarXid(int? id);

        Task<bool> Modificar(Categoria categoria);

        Task<bool> Eliminar(int id);

        Task<IEnumerable<Categoria>> Listar();

        Task<bool> CategoriaExists(int id);
    }
}
