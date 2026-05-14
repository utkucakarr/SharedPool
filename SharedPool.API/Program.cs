using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SharedPool.API.Exceptions;
using SharedPool.Application.Behaviors;
using SharedPool.Application.DTOs.SharedPool.Application.DTOs;
using SharedPool.Domain.Interfaces;
using SharedPool.Infrastructure.Contexts;
using SharedPool.Infrastructure.Repositories;

DotNetEnv.Env.Load(Path.Combine(Directory.GetCurrentDirectory(), "..", ".env"));

var builder = WebApplication.CreateBuilder(args);

// 3. Adım: Bağlantı dizesini önce sistem değişkenlerinden (Docker veya .env), 
// yoksa appsettings.json'dan alacak şekilde kurguluyoruz.
var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
                      ?? builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 1. Veritabanı Yapılandırması (PostgreSQL)
builder.Services.AddDbContext<SharedPoolDbContext>(options =>
    options.UseNpgsql(connectionString));

// 2. Dependency Injection (IoC) Kayıtları
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// 3. MediatR ve FluentValidation Kayıtları
// Application katmanındaki herhangi bir sınıfı vererek o assembly'yi taramasını sağlıyoruz.
var applicationAssembly = typeof(UserDto).Assembly;

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(applicationAssembly);
    // Pipeline Behavior'ımızı araya ekliyoruz
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

// Assembly içindeki tüm Validator'ları otomatik bul ve kaydet
builder.Services.AddValidatorsFromAssembly(applicationAssembly);

// 4. Global Exception Handler (.NET 8 standardı)
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// 5. Standart API Servisleri
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Pipeline (Middleware) Ayarları
app.UseExceptionHandler(); // Hataları yakalaması için en üste koyuyoruz

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
