using CRUDApp.Data;
using CRUDApp.Middleware;
using CRUDApp.Repositories;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//API Versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
});


builder.Services.AddDbContext<AppDBContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("CRUDdb")));
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeRepositoryV2, EmployeeRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();


var app = builder.Build();


app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCustomRequestLogging();

app.MapControllers();

app.Run();
