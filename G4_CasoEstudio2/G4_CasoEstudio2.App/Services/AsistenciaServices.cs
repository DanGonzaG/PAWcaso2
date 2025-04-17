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
            try
            {
                _contexto.Asistencias.Add(asistencia);
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

        
    }
}
