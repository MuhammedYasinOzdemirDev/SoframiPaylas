using System.Reflection;
using BlogApp.Models;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using SoframiPaylas.Application.ExternalServices;
using SoframiPaylas.Application.ExternalServices.Interfaces;
using SoframiPaylas.Application.Interfaces;
using SoframiPaylas.Application.Mappings;
using SoframiPaylas.Application.Services;
using SoframiPaylas.Infrastructure.Data.Config;
using SoframiPaylas.Infrastructure.Data.Service;
using SoframiPaylas.Infrastructure.Interfaces;
using SoframiPaylas.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// FirebaseConfig sınıfını singleton olarak kaydet
builder.Services.AddSingleton<FirebaseService>(serviceProvider =>
{
    // Yapılandırma nesnesini veya başka bağımlılıkları çözümle
    // Örneğin, eğer FirebaseConfig yerine IConfiguration gerekiyorsa:
    var config = serviceProvider.GetRequiredService<IConfiguration>();
    var firebaseConfigSection = config.GetSection("FIREBASE_CONFIG");
    if (firebaseConfigSection == null)
    {
        throw new InvalidOperationException("FIREBASE_CONFIG section is missing in the configuration.");
    }
    // Yapılandırmayı Dictionary olarak çevir
    var firebaseConfigDict = firebaseConfigSection.Get<Dictionary<string, string>>();

    // Dictionary'i model nesnesine dönüştür
    var firebaseConfig = new FireBaseConfig
    {
        Type = firebaseConfigDict.GetValueOrDefault("type"),
        ProjectId = firebaseConfigDict.GetValueOrDefault("project_id"),
        PrivateKeyId = firebaseConfigDict.GetValueOrDefault("private_key_id"),
        PrivateKey = firebaseConfigDict.GetValueOrDefault("private_key"),
        ClientEmail = firebaseConfigDict.GetValueOrDefault("client_email"),
        ClientId = firebaseConfigDict.GetValueOrDefault("client_id"),
        AuthUri = firebaseConfigDict.GetValueOrDefault("auth_uri"),
        TokenUri = firebaseConfigDict.GetValueOrDefault("token_uri"),
        AuthProviderX509CertUrl = firebaseConfigDict.GetValueOrDefault("auth_provider_x509_cert_url"),
        ClientX509CertUrl = firebaseConfigDict.GetValueOrDefault("client_x509_cert_url"),
        UniverseDomain = firebaseConfigDict.GetValueOrDefault("universe_domain")
    };
    if (firebaseConfig == null)
        throw new InvalidOperationException("Firebase configuration must be provided.");

    return new FirebaseService(firebaseConfig);
});
builder.Services.AddSingleton<FirebaseAppProvider>(serviceProvider =>
{
    // Yapılandırma nesnesini veya başka bağımlılıkları çözümle
    // Örneğin, eğer FirebaseConfig yerine IConfiguration gerekiyorsa:
    var config = serviceProvider.GetRequiredService<IConfiguration>();
    var firebaseConfigSection = config.GetSection("FIREBASE_CONFIG");
    if (firebaseConfigSection == null)
    {
        throw new InvalidOperationException("FIREBASE_CONFIG section is missing in the configuration.");
    }
    // Yapılandırmayı Dictionary olarak çevir
    var firebaseConfigDict = firebaseConfigSection.Get<Dictionary<string, string>>();

    // Dictionary'i model nesnesine dönüştür
    var firebaseConfig = new FireBaseConfig
    {
        Type = firebaseConfigDict.GetValueOrDefault("type"),
        ProjectId = firebaseConfigDict.GetValueOrDefault("project_id"),
        PrivateKeyId = firebaseConfigDict.GetValueOrDefault("private_key_id"),
        PrivateKey = firebaseConfigDict.GetValueOrDefault("private_key"),
        ClientEmail = firebaseConfigDict.GetValueOrDefault("client_email"),
        ClientId = firebaseConfigDict.GetValueOrDefault("client_id"),
        AuthUri = firebaseConfigDict.GetValueOrDefault("auth_uri"),
        TokenUri = firebaseConfigDict.GetValueOrDefault("token_uri"),
        AuthProviderX509CertUrl = firebaseConfigDict.GetValueOrDefault("auth_provider_x509_cert_url"),
        ClientX509CertUrl = firebaseConfigDict.GetValueOrDefault("client_x509_cert_url"),
        UniverseDomain = firebaseConfigDict.GetValueOrDefault("universe_domain")
    };
    if (firebaseConfig == null)
        throw new InvalidOperationException("Firebase configuration must be provided.");

    return new FirebaseAppProvider(firebaseConfig);
});
builder.Services.AddSingleton<FirebaseAuthService>();

builder.Services.AddSingleton<FirebaseMessagingService>();


//Repository
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IFoodRepository, FoodRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IParticipantRepository, ParticipantRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IAnnouncementRepository, AnnouncementRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
//Service
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IFoodService, FoodService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IParticipantService, ParticipantService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IAnnouncementService, AnnouncementService>();
builder.Services.AddScoped<IMessageService, MessageService>();
//Mapping
builder.Services.AddAutoMapper(typeof(MappingProfile));

//External Services
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddSingleton<IEmailSender, EmailService>(serviceProvider =>
{
    var emailSettings = serviceProvider.GetRequiredService<IOptions<EmailSettings>>().Value;
    return new EmailService(emailSettings.SmtpHost, emailSettings.SmtpPort, emailSettings.FromAddress, emailSettings.SmtpUsername, emailSettings.SmtpPassword);
});
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
    c.AddServer(new OpenApiServer { Url = "http://localhost:5103", Description = "Geliştirme Ortamı" });
    c.AddServer(new OpenApiServer { Url = "https://soframipaylaswebapi.azurewebsites.net", Description = "Azure Cloud Ortamı" });
    c.AddServer(new OpenApiServer { Url = "https://xn--sofranpaylas-64b.azurewebsites.net", Description = "Azure Cloud Ortamı 2 " });




    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    // Application katmanının XML dosyasını ekleyin
    var applicationXmlFile = "SoframiPaylas.Application.xml";
    var applicationXmlPath = Path.Combine(AppContext.BaseDirectory, applicationXmlFile);
    c.IncludeXmlComments(applicationXmlPath);
});

builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowSpecificOrigin",
            builder => builder.WithOrigins("http://localhost:5103", "https://soframipaylaswebapi.azurewebsites.net")
                              .AllowAnyMethod()
                              .AllowAnyHeader());
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

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowSpecificOrigin");
app.UseAuthorization();
app.MapControllers();


app.Run();


