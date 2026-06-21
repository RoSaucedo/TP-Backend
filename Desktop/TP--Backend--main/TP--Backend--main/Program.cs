using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// acá esta la configuración a la BD 
var connectionString = builder.Configuration.GetConnectionString("ConexionDefault");
builder.Services.AddDbContext<GestorDespacho.Models.AplicationDbContext>(options =>
    options.UseSqlServer(connectionString));


// Add services to the container.
builder.Services.AddDistributedMemoryCache(); // prepara un lugar en la memoria para guardar los datos de la sesion
builder.Services.AddSession(); // habilita las sesiones de la aplicacion
builder.Services.AddControllersWithViews();

var app = builder.Build();
var supportedCultures = new[] { "es-AR" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();
app.UseSession();  // le avisa a la app que use las sesiones en cada pedido que llega

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
