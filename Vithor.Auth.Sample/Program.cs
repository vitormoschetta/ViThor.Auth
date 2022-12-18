using ViThor.Auth.Extensions;
using ViThor.Auth.Sample.Services;
using ViThor.Auth.Services.User;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add your own services
builder.Services.AddSingleton<IUserService<ViThor.Auth.Sample.Models.User>, UserService>();

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