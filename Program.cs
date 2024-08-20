using Microsoft.EntityFrameworkCore;
using System;
using WebApplication1;
using WebApplication1.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApiContext>
    (opt => opt.UseInMemoryDatabase(databaseName: "TodosDb"));

builder.Services.AddScoped<ApiContext>();

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

LoadSeedData(app);

app.Run();

static void LoadSeedData(WebApplication app)
{
    var newTodos = new List<Todo>()
    {
        new() { Name = "Todo 1",Status = 1 },
        new() { Name = "Todo 2",Status = 1 },
    };

    using var scope = app.Services.CreateScope();
    ApiContext context = scope.ServiceProvider.GetRequiredService<ApiContext>();

    if (context is not null)
    {
        context.AddRange(newTodos);
        context.SaveChanges();
    }
}
