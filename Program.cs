using PriorityApp.Api.Stores;
using PriorityApp.PriorityApp.Api.Services;
using PriorityApp.PriorityApp.Api.Stores;
using Microsoft.AspNetCore.Builder;


var builder = WebApplication.CreateBuilder(args);

// Реєструємо “in‑memory” сховища як **singleton** (збереження до завершення процесу)
builder.Services.AddSingleton<IPreferencesStore, MemoryPreferencesStore>();
builder.Services.AddSingleton<ITaskStore, MemoryTaskStore>();

// Сервіс підрахунку
builder.Services.AddScoped<IScoringService, ScoringService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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