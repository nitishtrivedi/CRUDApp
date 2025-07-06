using CRUDApp.Data;
using CRUDApp.DTOs;
using CRUDApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUDApp.Repositories
{
    public class EmployeeRepository : IEmployeeRepository, IEmployeeRepositoryV2
    {
        private readonly AppDBContext _dbContext;
        public EmployeeRepository(AppDBContext dBContext)
        {
            _dbContext = dBContext;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync(string department)
        {
            department = department.ToLower();
            IEnumerable<Employee> employees = string.IsNullOrWhiteSpace(department) 
                ? await _dbContext.Employees.AsNoTracking().ToListAsync()
                : await _dbContext.Employees.Where(e => e.Department.Equals(department, StringComparison.CurrentCultureIgnoreCase)).AsNoTracking().ToListAsync();
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

        public async Task<IEnumerable<DepartmentSummaryDto>> GetDepartmentSummaryAsync(int minCount = 3)
        {
            //Get all records first
            var employees = await _dbContext.Employees.AsNoTracking().ToListAsync();

            return employees
                .GroupBy(e => e.Department)
                .Where(g => g.Count() >= minCount)
                .Select(g => new DepartmentSummaryDto
                {
                    Department = g.Key,
                    Count = g.Count(),
                    Employees = g.Where(e => !string.IsNullOrWhiteSpace(e.Email))
                                .OrderBy(e => e.LastName)
                                .ThenBy(e => e.FirstName)
                                .ToList()
                }).ToList();
        }


        //public async Task<IEnumerable<Employee>> GetByDepartmentASync(string department)
        //{
        //    var employees = await _dbContext.Employees.Where(e => e.Department == department).ToListAsync();
        //    return employees;
        //}
    }
}
