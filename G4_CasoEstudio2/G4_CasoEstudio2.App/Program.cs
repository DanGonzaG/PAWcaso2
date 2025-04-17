using G4_CasoEstudio2.App.Data;
using G4_CasoEstudio2.App.Models;
using G4_CasoEstudio2.App.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//  Conexión a la base de datos usando "DefaultConnection" desde appsettings.json
var connectionString = builder.Configuration.GetConnectionString("Server")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
Console.WriteLine($"Cadena de conexión utilizada: {connectionString}");

//  Inyección de dependencias de Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDbContext<Contexto>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//  Configuración de Identity
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
    options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>() //se agrega para habilitar el servicio de roles
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

// Servicios
builder.Services.AddScoped<IAsistenciaServices, AsistenciaServices>();
builder.Services.AddScoped<ICategoriaServices, CategoriaServices>();
builder.Services.AddScoped<IEventoServices, EventoServices>();
builder.Services.AddScoped<IUsuarioServices, UsuarioServices>();

var app = builder.Build();

//Verifica se lo roles existen y si no los crea todo esto sucede en el incio de la aplicacion
using (var scope = app.Services.CreateScope())
{
    var roles = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    if (!await roles.RoleExistsAsync("Administrador") || !await roles.RoleExistsAsync("Organizador") || !await roles.RoleExistsAsync("Usuario"))
    {
        await roles.CreateAsync(new IdentityRole("Administrador"));
        await roles.CreateAsync(new IdentityRole("Organizador"));
        await roles.CreateAsync(new IdentityRole("Usuario"));
    }
}


//  Configuración del pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

//  Configuración de rutas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

//  MINIMAL API con DTO
app.MapGet("/api/eventos", async (Contexto db) =>
{
    var eventos = await db.Eventos
        .Include(e => e.Categoria)
        .Where(e => e.Estado == true)
        .Select(e => new EventoDTO
        {
            Id = e.Id,
            Titulo = e.Titulo,
            Descripcion = e.Descripcion,
            Fecha = e.Fecha,
            Hora = e.Hora,
            Ubicacion = e.Ubicacion,
            CupoMaximo = e.CupoMaximo,
            Categoria = e.Categoria.Nombre
        })
        .ToListAsync();

    return Results.Ok(eventos);
});

app.MapGet("/api/eventos/{id}", async (int id, Contexto db) =>
{
    var evento = await db.Eventos
        .Include(e => e.Categoria)
        .Where(e => e.Id == id && e.Estado == true)
        .Select(e => new EventoDTO
        {
            Id = e.Id,
            Titulo = e.Titulo,
            Descripcion = e.Descripcion,
            Fecha = e.Fecha,
            Hora = e.Hora,
            Ubicacion = e.Ubicacion,
            CupoMaximo = e.CupoMaximo,
            Categoria = e.Categoria.Nombre
        })
        .FirstOrDefaultAsync();

    return evento is not null ? Results.Ok(evento) : Results.NotFound();
});

// API: Lista de asistentes por evento
app.MapGet("/api/eventos/{id}/asistentes", async (int id, Contexto db) =>
{
    var asistentes = await db.Asistencias
        .Where(a => a.EventoId == id)
        .Select(a => new AsistenciaDTO
        {
            Id = a.Id,
            EventoId = a.EventoId,
            UsuarioId = a.UsuarioId,
            NombreUsuario = a.Usuario.NombreUsuario,
            Correo = a.Usuario.Correo
        })
        .ToListAsync();

    return Results.Ok(asistentes);
});

app.Run();
