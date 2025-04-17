using G4_CasoEstudio2.App.Models;

namespace G4_CasoEstudio2.App.Services
{
    public interface IAsistenciaServices
    {
        Task<bool> crear(Asistencia asistencia);

        Task<Asistencia> BuscarXid(int? id);

        Task<bool> Modificar(Asistencia asistencia);

        Task<bool> Eliminar(int id);

        Task<IEnumerable<Asistencia>> Listar();

        Task<bool> AsistenciaExists(int id);
    }
}
