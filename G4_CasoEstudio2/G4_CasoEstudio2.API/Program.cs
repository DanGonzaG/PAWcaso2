using G4_CasoEstudio2.App.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configuración de cadena de conexión compartida
var connectionString = builder.Configuration.GetConnectionString("Server")
    ?? throw new InvalidOperationException("Connection string 'Server' not found.");

builder.Services.AddDbContext<Contexto>(options =>
    options.UseSqlServer(connectionString));

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware de Swagger
app.UseSwagger();
app.UseSwaggerUI();

// Endpoint: Obtener todos los eventos disponibles
app.MapGet("/api/events", async (Contexto db) =>
{
    var eventos = await db.Eventos
        .Where(e => e.Estado == true)
        .Select(e => new
        {
            e.Id,
            e.Titulo,
            e.Descripcion,
            e.Fecha,
            e.Hora,
            e.Ubicacion,
            e.CupoMaximo
        })
        .ToListAsync();

    return Results.Ok(eventos);
});

// Endpoint: Obtener evento por ID
app.MapGet("/api/events/{id}", async (int id, Contexto db) =>
{
    var evento = await db.Eventos
        .Where(e => e.Id == id && e.Estado == true)
        .Select(e => new
        {
            e.Id,
            e.Titulo,
            e.Descripcion,
            e.Fecha,
            e.Hora,
            e.Ubicacion,
            e.CupoMaximo
        })
        .FirstOrDefaultAsync();

    return evento is not null ? Results.Ok(evento) : Results.NotFound();
});

app.Run();
