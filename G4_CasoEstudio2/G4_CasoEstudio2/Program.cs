using G4_CasoEstudio2.App.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Leer la cadena de conexión desde appsettings.json
var connectionString = builder.Configuration.GetConnectionString("Server")
    ?? throw new InvalidOperationException("No se encontró la cadena de conexión");

builder.Services.AddDbContext<Contexto>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// ========================
// ENDPOINTS MINIMAL API
// ========================

// Listar eventos
app.MapGet("/api/eventos", async (Contexto db) =>
{
    var eventos = await db.Eventos
        .Include(e => e.Categoria)
        .Where(e => e.Estado == "Activo")
        .Select(e => new
        {
            e.Id,
            e.Titulo,
            e.Descripcion,
            e.Fecha,
            e.Hora,
            e.Ubicacion,
            e.CupoMaximo,
            Categoria = e.Categoria.Nombre
        })
        .ToListAsync();

    return Results.Ok(eventos);
});

// Detalle de evento
app.MapGet("/api/eventos/{id}", async (int id, Contexto db) =>
{
    var evento = await db.Eventos
        .Include(e => e.Categoria)
        .Where(e => e.Id == id && e.Estado == "Activo")
        .Select(e => new
        {
            e.Id,
            e.Titulo,
            e.Descripcion,
            e.Fecha,
            e.Hora,
            e.Ubicacion,
            e.CupoMaximo,
            Categoria = e.Categoria.Nombre
        })
        .FirstOrDefaultAsync();

    return evento is not null ? Results.Ok(evento) : Results.NotFound();
});

// Asistentes por evento
app.MapGet("/api/eventos/{id}/asistentes", async (int id, Contexto db) =>
{
    var asistentes = await db.Asistencias
        .Where(a => a.EventoId == id)
        .Select(a => new
        {
            a.Id,
            a.EventoId,
            a.UsuarioId,
            a.Usuario.NombreUsuario,
            a.Usuario.Correo
        })
        .ToListAsync();

    return Results.Ok(asistentes);
});

app.Run();
