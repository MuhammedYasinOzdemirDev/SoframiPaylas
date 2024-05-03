using SoframiPaylas.WebUI.Mappings;
using SoframiPaylas.WebUI.Services;
using SoframiPaylas.WebUI.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient("API", c =>
{
    c.BaseAddress = new Uri("http://localhost:5103/api/"); // API'nizin adresi
});

//Api Services
builder.Services.AddScoped<IEventApiService, EventApiService>();

//Mapping
builder.Services.AddAutoMapper(typeof(ViewModelToDtoProfile));

var app = builder.Build();


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

});

app.Run();
