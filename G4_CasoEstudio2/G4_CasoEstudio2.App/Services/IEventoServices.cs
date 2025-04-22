using G4_CasoEstudio2.App.Models;

namespace G4_CasoEstudio2.App.Services
{
    public interface IEventoServices
    {
        Task<bool> Crear(Evento evento);

        Task<Evento> BuscarXid(int? id);

        Task<bool> Modificar(Evento evento);

        Task<bool> Eliminar(int id);

        Task<IEnumerable<Evento>> Listar();

        Task<bool> EventoExists(int id);

        Task<IEnumerable<Evento>> ListarEventosPorOrganizador(string organizadorId);
        Task<bool> EsOrganizadorEvento(string usuarioId, int eventoId);
    }
}
