using CRUDApp.Models;
using CRUDApp.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CRUDApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        public EmployeesController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            var employees = await _employeeRepository.GetAllAsync();
            return Ok(employees);
        }
        [HttpGet("{id}")]

        public async Task<IActionResult> GetEmployeeById(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            return Ok(employee);
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromBody] Employee employee)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var newEmployee = await _employeeRepository.AddAsync(employee);
            return CreatedAtAction(nameof(GetEmployeeById), new {id = employee.Id}, newEmployee);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditEmployee([FromBody] Employee employee, int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (id != employee.Id) return BadRequest(new { Message = $"Employee with ID: {id} not found" });
            var success = await _employeeRepository.UpdateAsync(employee, id);
            if (!success) return NotFound(new {Message = $"Employee with ID: {id} was not found or could not be edited"});
            return Ok(new {Message = $"Employee with ID: {id} was updated successfully"});
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var success = await _employeeRepository.DeleteAsync(id);
            if (!success) return BadRequest(new { Message = $"Employee with ID: {id} could not be deleted or was not found" });
            return Ok(new {Message = $"Employee with ID: {id} deleted successfully"});
        }
    }
}
