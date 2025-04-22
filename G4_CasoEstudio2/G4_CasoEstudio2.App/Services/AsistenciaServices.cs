using G4_CasoEstudio2.App.Models;
using Microsoft.EntityFrameworkCore;

namespace G4_CasoEstudio2.App.Services
{
    public class AsistenciaServices : IAsistenciaServices
    {
        private readonly Contexto _contexto;

        public AsistenciaServices(Contexto contexto)
        {
            _contexto = contexto;
        }

        public async Task<Asistencia> BuscarXid(int? id)
        {
            var buscarAsistencia = await _contexto.Asistencias.FindAsync(id);
            return buscarAsistencia;
        }

        public async Task<bool> crear(Asistencia asistencia)
        {
            // Obtener evento actual
            var eventoActual = await _contexto.Eventos.FindAsync(asistencia.EventoId);
            if (eventoActual == null) return false;

            // Calcular rango del evento actual
            DateTime inicioActual = eventoActual.Fecha.Date + eventoActual.Hora;
            DateTime finActual = inicioActual.AddHours(eventoActual.Duration);

            // Buscar asistencias anteriores del usuario
            var asistenciasUsuario = await _contexto.Asistencias
                .Where(a => a.UsuarioId == asistencia.UsuarioId)
                .Include(a => a.Evento)
                .ToListAsync();

            // Verificar superposición
            foreach (var a in asistenciasUsuario)
            {
                DateTime inicio = a.Evento.Fecha.Date + a.Evento.Hora;
                DateTime fin = inicio.AddHours(a.Evento.Duration);

                bool hayConflicto = inicioActual < fin && inicio < finActual;
                if (hayConflicto)
                    throw new Exception("Ya estás inscrito en un evento que se superpone con este.");
            }

            // Verificar cupo disponible
            int inscritos = await _contexto.Asistencias
                .CountAsync(a => a.EventoId == asistencia.EventoId);

            if (inscritos >= eventoActual.CupoMaximo)
                throw new Exception("El evento ya alcanzó el cupo máximo.");

            _contexto.Asistencias.Add(asistencia);
            await _contexto.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Eliminar(int id)
        {
            try
            {
                var asistencia = await BuscarXid(id);
                if (asistencia != null)
                {
                    _contexto.Asistencias.Remove(asistencia);
                }
                await _contexto.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> AsistenciaExists(int id)
        {
            await _contexto.Asistencias.AnyAsync(e => e.Id == id);
            return true;
        }

        public async Task<IEnumerable<Asistencia>> Listar()
        {
            var lista = await _contexto.Asistencias.ToListAsync();
            return lista;
        }

        public async Task<bool> Modificar(Asistencia asistencia)
        {
            try
            {
                _contexto.Update(asistencia);
                await _contexto.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<Asistencia>> ListarPorOrganizador(string organizadorId)
        {
            return await _contexto.Asistencias
                .Include(a => a.Evento)
                .Include(a => a.Usuario)
                .Where(a => a.Evento.UsuarioRegistro == organizadorId)
                .ToListAsync();
        }
    }
}
