using System.Reflection;
using Microsoft.OpenApi.Models;
using SoframiPaylas.Application.Interfaces;
using SoframiPaylas.Application.Mappings;
using SoframiPaylas.Application.Services;
using SoframiPaylas.Infrastructure.Data.Service;
using SoframiPaylas.Infrastructure.Interfaces;
using SoframiPaylas.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<FirebaseService>();  // Firebase servisini singleton olarak kaydet


//Repository
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IFoodRepository, FoodRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IParticipantRepository, ParticipantRepository>();
//Service
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IFoodService, FoodService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IParticipantService, ParticipantService>();
//Mapping
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Swager
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Sofranı Paylas API",
        Version = "v1",
        Description = "Yemek Paylaşım Platformu, kullanıcıların yemek tariflerini paylaştıkları ve etkinlik düzenleyebildikleri bir platformdur. Bu API, platform üzerindeki gönderi, katılımcı ve etkinlik yönetimini sağlar. Kullanıcılar gönderiler oluşturabilir, gönderilere katılabilir ve etkinlikleri yönetebilirler.",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Muhammed Yasin Özdemir",
            Email = "cozdemir@gmail.com",
            Url = new Uri("https://www.linkedin.com/in/yasin-%C3%B6zdemir1903/")
        },
        License = new OpenApiLicense
        {
            Name = "MIT License",
            Url = new Uri("https://opensource.org/licenses/MIT")
        }

    });
    c.AddServer(new OpenApiServer { Url = "https://api.example.com", Description = "Üretim Ortamı" });
    c.AddServer(new OpenApiServer { Url = "https://api-staging.example.com", Description = "Staging Ortamı" });
    c.AddServer(new OpenApiServer { Url = "http://localhost:5000", Description = "Geliştirme Ortamı" });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    // Application katmanının XML dosyasını ekleyin
    var applicationXmlFile = "SoframiPaylas.Application.xml";
    var applicationXmlPath = Path.Combine(AppContext.BaseDirectory, applicationXmlFile);
    c.IncludeXmlComments(applicationXmlPath);
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
   {
       c.SwaggerEndpoint("/swagger/v1/swagger.yaml", "Sofranı Paylas API V1");
       c.RoutePrefix = string.Empty;  // Swagger UI'ı ana sayfada açar
   });
}


app.UseAuthorization();

app.UseHttpsRedirection();
app.MapControllers();


app.Run();


