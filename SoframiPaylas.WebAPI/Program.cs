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

//Swager
builder.Services.AddSwaggerGen(c =>
   {
       c.SwaggerDoc("v1", new OpenApiInfo
       {
           Title = "My API",
           Version = "v1",
           Description = "A detailed description of my API.",
           TermsOfService = new Uri("https://example.com/terms"),
           Contact = new OpenApiContact
           {
               Name = "Support",
               Email = "support@example.com",
               Url = new Uri("https://example.com/contact")
           },
           License = new OpenApiLicense
           {
               Name = "Use under LICX",
               Url = new Uri("https://example.com/license")
           }
       });
       // XML Yorumlarını etkinleştirmek için
       var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
       var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
       c.IncludeXmlComments(xmlPath);

   });
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
   {
       c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sofranı Paylas API V1");
       c.RoutePrefix = string.Empty;  // Swagger UI'ı ana sayfada açar
   });
}


app.UseAuthorization();

app.UseHttpsRedirection();
app.MapControllers();


app.Run();


