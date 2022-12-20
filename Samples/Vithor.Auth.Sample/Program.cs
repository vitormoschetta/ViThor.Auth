using Vithor.Auth.Sample.Requests;
using Vithor.Auth.Sample.Services;
using ViThor.Auth.Extensions;
using ViThor.Auth.Models;
using ViThor.Auth.Requests;
using ViThor.Auth.Sample.Models;
using ViThor.Auth.Sample.Services;
using ViThor.Auth.Services.Mapper;
using ViThor.Auth.Services.User;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Serviço para gerenciar usuários
builder.Services.AddSingleton<IUserService<User>, UserService>();

// Serviço para gerenciar mapeamento de requisições para usuários customizados. Ou usar o MapperService padrão, conforme acima.
builder.Services.AddScoped<IMapperService<User, CreateUserRequest>, MyMapperService>();

// Add ViThor services and settings
builder.BuildViThorSettings();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();