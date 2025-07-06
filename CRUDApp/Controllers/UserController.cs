using CRUDApp.Models;
using CRUDApp.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CRUDApp.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UserController(IUserRepository userRepository) : ControllerBase
    {
        private readonly IUserRepository _userRepository = userRepository;

        [HttpGet]
        [MapToApiVersion("1.0")]
        [MapToApiVersion("2.0")]
        public async Task<IActionResult> GetAllAsync(bool? isAdmin)
        {
            var users = await _userRepository.GetAllAsync(isAdmin);
            return Ok(users);
        }

        [HttpGet("{id:int}")]
        [MapToApiVersion("1.0")]
        [MapToApiVersion("2.0")]
        public async Task<IActionResult> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return Ok(user);
        }

        [HttpPost]
        [MapToApiVersion("1.0")]
        [MapToApiVersion("2.0")]
        public async Task<IActionResult> AddUserAsync([FromBody] User user)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var newEmployee = await _userRepository.AddAsync(user);
            return CreatedAtAction(nameof(GetUserByIdAsync), new {id =  newEmployee.Id}, newEmployee);
        }

        [HttpPut("{id:int}")]
        [MapToApiVersion("1.0")]
        [MapToApiVersion("2.0")]
        public async Task<IActionResult> EditUserAsync([FromBody] User user, int id)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);
            if (id != user.Id) return BadRequest(new {message = $"User with ID: {id} could not be found"});
            var success = await _userRepository.UpdateAsync(user, id);
            if (!success) return NotFound(new { message = $"User with ID: {id} could not be updated" });
            return Ok(new { message = $"User with ID: {id} updated successfully"});
        }

        [HttpDelete("{id:int}")]
        [MapToApiVersion("1.0")]
        [MapToApiVersion("2.0")]
        public async Task<IActionResult> DeleteUserAsync(int id)
        {
            var success = await _userRepository.DeleteAsync(id);
            if (!success) return BadRequest(new { message = $"User with ID: {id} could not be deleted" });
            return Ok(new { message = $"User with ID: {id} deleted successfully" });
        }

    }
}
