using SoframiPaylas.WebUI.ExternalService.Handler;
using SoframiPaylas.WebUI.Mappings;
using SoframiPaylas.WebUI.Services;
using SoframiPaylas.WebUI.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
        }).AddSessionStateTempDataProvider();

builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session timeout süresini belirleyin
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
//Api Services
builder.Services.AddScoped<IPostApiService, PostApiService>();
builder.Services.AddScoped<IAuthService, AuthApiService>();
builder.Services.AddScoped<IUserApiService, UserApiService>();
//Mapping
builder.Services.AddAutoMapper(typeof(ViewModelToDtoProfile));

//Handler
builder.Services.AddTransient<RetryHandler>();
builder.Services.AddHttpClient("API", c =>
{
    c.BaseAddress = new Uri("http://localhost:5103/api/"); // API'nizin adresi
}).AddHttpMessageHandler<RetryHandler>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "CustomScheme";
    options.DefaultChallengeScheme = "CustomScheme";
})
.AddScheme<AuthenticationSchemeOptions, CustomJwtAuthenticationHandler>("CustomScheme", options => { });
builder.Services.AddAuthorization();
var app = builder.Build();


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

});

app.Run();
