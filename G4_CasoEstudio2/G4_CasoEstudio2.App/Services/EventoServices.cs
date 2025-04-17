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
            var buscarEventos = await _contexto.Eventos.FindAsync(id);
            return buscarEventos;
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
            var lista = await _contexto.Eventos.ToListAsync();
            return lista;
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
    }
}
