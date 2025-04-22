using G4_CasoEstudio2.App.Models;
using Microsoft.EntityFrameworkCore;

namespace G4_CasoEstudio2.App.Services
{
    public class EventoServices : IEventoServices
    {

        private readonly Contexto _contexto;

        public EventoServices(Contexto contexto)
        {
            _contexto = contexto;
        }

        public async Task<Evento> BuscarXid(int? id)
        {
            return await _contexto.Eventos
                                  .Include(e => e.Usuario)
                                  .Include(e => e.Categoria)
                                  .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<bool> Crear(Evento evento)
        {
            try
            {
                _contexto.Eventos.Add(evento);
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
                var evento = await BuscarXid(id);
                if (evento != null)
                {
                    _contexto.Eventos.Remove(evento);
                }
                await _contexto.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> EventoExists(int id)
        {
            await _contexto.Eventos.AnyAsync(e => e.Id == id);
            return true;
        }

        public async Task<IEnumerable<Evento>> Listar()
        {
            return await _contexto.Eventos
                                .Include(e => e.Usuario)       // Carga el usuario que registró el evento
                                .Include(e => e.Categoria)     // Carga la categoría del evento
                                .ToListAsync();
        }

        public async Task<bool> Modificar(Evento evento)
        {
            try
            {
                _contexto.Update(evento);
                await _contexto.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<IEnumerable<Evento>> ListarEventosPorOrganizador(string organizadorId)
        {
            return await _contexto.Eventos
                .Include(e => e.Categoria)
                .Include(e => e.Asistencias)
                .Where(e => e.UsuarioRegistro == organizadorId)
                .OrderByDescending(e => e.Fecha)
                .ThenBy(e => e.Hora)
                .ToListAsync();
        }

        public async Task<bool> EsOrganizadorEvento(string usuarioId, int eventoId)
        {
            return await _contexto.Eventos
                .AnyAsync(e => e.Id == eventoId && e.UsuarioRegistro == usuarioId);
        }

        public async Task<List<Evento>> EventosConInscritosPorOrganizador(string organizadorId)
        {
            return await _contexto.Eventos
                .Where(e => e.UsuarioRegistro == organizadorId)
                .Include(e => e.Asistencias)
                    .ThenInclude(a => a.Usuario)
                .ToListAsync();
        }
    }
}
