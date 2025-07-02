using CRUDApp.Data;
using CRUDApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUDApp.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDBContext _dbContext;
        public EmployeeRepository(AppDBContext dBContext)
        {
            _dbContext = dBContext;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            var employees = await _dbContext.Employees.AsNoTracking().ToListAsync();
            return employees;
        }

        public async Task<Employee> GetByIdAsync(int id)
        {
            var employee = await _dbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);
            return employee ?? null;
        }

        public async Task<Employee> AddAsync(Employee employee)
        {
            _dbContext.Employees.Add(employee);
            await _dbContext.SaveChangesAsync();
            return employee;
        }

        public async Task<bool> UpdateAsync(Employee employee, int id)
        {
            if (id != employee.Id) return false;
            var existingEmployee = await _dbContext.Employees.FindAsync(id);
            if (existingEmployee == null) return false;

            //Manually update fields
            existingEmployee.FirstName = employee.FirstName;
            existingEmployee.LastName = employee.LastName;
            existingEmployee.Email = employee.Email;
            existingEmployee.Department = employee.Department;
            existingEmployee.ContactNumber = employee.ContactNumber;


            _dbContext.Entry(existingEmployee).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var employee = await _dbContext.Employees.FindAsync(id);
            if (employee == null) return false;
            _dbContext.Remove(employee);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Employee>> AddRangeAsync(IEnumerable<Employee> employees)
        {
            _dbContext.Employees.AddRange(employees);
            await _dbContext.SaveChangesAsync();
            return employees;
        }
    }
}
