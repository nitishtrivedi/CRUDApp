﻿using CRUDApp.DTOs;
using CRUDApp.Models;

namespace CRUDApp.Repositories
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllAsync(string department);
        Task<Employee> GetByIdAsync(int id);
        Task<Employee> AddAsync(Employee employee);
        Task<bool> UpdateAsync(Employee employee, int id);
        Task<bool> DeleteAsync(int id);

        
    }
}
