using CRUDApp.Models;
using CRUDApp.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CRUDApp.Controllers
{
    [ApiController]
    [ApiVersion("1.0")] //Added versioning
    [ApiVersion("2.0")] //Added versioning
    [Route("api/v{version:apiVersion}/[controller]")] //Updated route to include versioning
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEmployeeRepositoryV2 _employeeRepositoryV2;
        public EmployeesController(IEmployeeRepository employeeRepository, IEmployeeRepositoryV2 employeeRepositoryV2)
        {
            _employeeRepository = employeeRepository;
            _employeeRepositoryV2 = employeeRepositoryV2;
        }

        [HttpGet]
        [MapToApiVersion("1.0"), MapToApiVersion("2.0")] //Compatible to both versions
        public async Task<IActionResult> GetEmployees([FromQuery] string? department)
        {
            var employees = await _employeeRepository.GetAllAsync(department);
            return Ok(employees);
        }
        [HttpGet("{id:int}")]
        [MapToApiVersion("1.0"), MapToApiVersion("2.0")] //Compatible to both versions
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            return Ok(employee);
        }

        [HttpPost]
        [MapToApiVersion("1.0")] //Compatible only with v1
        public async Task<IActionResult> AddEmployee([FromBody] Employee employee)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var newEmployee = await _employeeRepository.AddAsync(employee);
            return CreatedAtAction(nameof(GetEmployeeById), new {id = employee.Id}, newEmployee);
        }

        [HttpPost("bulk")] // Path is api/v2/employees/bulk
        [MapToApiVersion("2.0")] //Compatible only with v2
        public async Task<IActionResult> AddEmployeesBulk([FromBody] IEnumerable<Employee> employees)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var added = await _employeeRepositoryV2.AddRangeAsync(employees);
            return StatusCode(StatusCodes.Status201Created, added);
        }

        [HttpPut("{id:int}")]
        [MapToApiVersion("1.0"), MapToApiVersion("2.0")] //Compatible to both versions
        public async Task<IActionResult> EditEmployee([FromBody] Employee employee, int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (id != employee.Id) return BadRequest(new { Message = $"Employee with ID: {id} not found" });
            var success = await _employeeRepository.UpdateAsync(employee, id);
            if (!success) return NotFound(new {Message = $"Employee with ID: {id} was not found or could not be edited"});
            return Ok(new {Message = $"Employee with ID: {id} was updated successfully"});
        }

        [HttpDelete("{id}")]
        [MapToApiVersion("1.0"), MapToApiVersion("2.0")] //Compatible to both versions
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var success = await _employeeRepository.DeleteAsync(id);
            if (!success) return BadRequest(new { Message = $"Employee with ID: {id} could not be deleted or was not found" });
            return Ok(new {Message = $"Employee with ID: {id} deleted successfully"});
        }


        //API V2 METHODS:
        //[HttpGet]
        //[MapToApiVersion("1.0") ,MapToApiVersion("2.0")]
        //public async Task<IActionResult> GetEmployeesByDepartment([FromQuery] string? department)
        //{
        //    IEnumerable<Employee> result;
        //    if (string.IsNullOrWhiteSpace(department))
        //    {
        //        result = await _employeeRepository.GetAllAsync();
        //    }
        //    else
        //    {
        //        result = await _employeeRepository.GetByDepartmentASync(department);
        //    }
        //    return Ok(result);
        //}

        [HttpGet("summary")]
        [MapToApiVersion("2.0")]
        public async Task<IActionResult> GetDepartmentSummaryAsync([FromQuery] int minCount = 3)
        {
            var summary = await _employeeRepositoryV2.GetDepartmentSummaryAsync(minCount);
            return Ok(summary);
        }
    }
}
