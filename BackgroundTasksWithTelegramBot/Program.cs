using BackgroundTasksWithTelegramBot.BackgroundTasks;
using BackgroundTasksWithTelegramBot.Data;
using BackgroundTasksWithTelegramBot.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreDB")));

builder.Services.AddHostedService<ValyutaService>();
builder.Services.AddHostedService<BotService>();
//builder.Services.AddHostedService<TestService>();
builder.Services.AddTransient<IBotUserService, BotUserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
