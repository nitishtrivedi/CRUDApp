using CRUDApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUDApp.Data
{
    public class AppDBContext(DbContextOptions<AppDBContext> options) : DbContext(options)
    {
        public DbSet<Employee> Employees { get; set; }
    }
}
