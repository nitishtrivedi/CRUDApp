using CRUDApp.Data;
using CRUDApp.Middleware;
using CRUDApp.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDBContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("CRUDdb")));
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();


var app = builder.Build();


app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCustomRequestLogging();

app.MapControllers();

app.Run();
