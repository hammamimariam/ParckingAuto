using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ParckingAuto.Data;
using ParckingAuto.Services;
using ParckingAuto.Repositories;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// JWT settings
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);

// Connexion MySQL
builder.Services.AddDbContext<ParcAutoContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), new MySqlServerVersion(new Version(8, 0, 32))));

// Authentification JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
    options.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            System.Console.WriteLine("JWT AUTHENTICATION FAILED: " + context.Exception.ToString());
            return System.Threading.Tasks.Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            System.Console.WriteLine("JWT TOKEN VALIDATED SUCCESSFULLY.");
            return System.Threading.Tasks.Task.CompletedTask;
        }
    };
});

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// MVC + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Services et Repositories
        builder.Services.AddHttpContextAccessor(); // Required for AuditService to get IP/UserAgent
        builder.Services.AddScoped<UtilisateurRepository>();
        builder.Services.AddScoped<UtilisateurService>();
        builder.Services.AddScoped<JwtService>();
        builder.Services.AddScoped<AlerteRepository>();
        builder.Services.AddScoped<AlerteService>();
        builder.Services.AddScoped<CarburantRepository>();
        builder.Services.AddScoped<CarburantService>();
        builder.Services.AddScoped<ChauffeurRepository>();
        builder.Services.AddScoped<ChauffeurService>();
        builder.Services.AddScoped<MaintenanceRepository>();
        builder.Services.AddScoped<MaintenanceService>();
        builder.Services.AddScoped<MouvementRepository>();
        builder.Services.AddScoped<MouvementService>();
        builder.Services.AddScoped<VehiculeRepository>();
        builder.Services.AddScoped<VehiculeService>();
        builder.Services.AddScoped<ParametresService>();
        builder.Services.AddScoped<AuditRepository>();
        builder.Services.AddScoped<AuditService>();
        builder.Services.AddScoped<IOcrService, OcrService>();

        builder.Services.AddScoped(sp => new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7275/api/")
        });

// Autoriser CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        policy =>
        {
            policy.WithOrigins("http://localhost:5199", "https://localhost:7042", "http://localhost:5228")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

var app = builder.Build();

// Swagger en dev
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ParcAutoContext>();
    context.Database.Migrate();
    DbInitializer.Seed(context);
}

app.UseCors("AllowLocalhost");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
