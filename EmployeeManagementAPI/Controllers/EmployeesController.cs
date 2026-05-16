using EmployeeManagementAPI.DTOs;
using EmployeeManagementAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace EmployeeManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _service;

        public EmployeesController(IEmployeeService service)
        {
            _service = service;
        }
        [HttpGet]
        [Authorize(Roles = "Admin,HR,Viewer")]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null)
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var result = await _service.GetAll(page, pageSize, search);
            return Ok(result);
        }


        [HttpGet("{id}")]
        [Authorize(Roles ="Admin,HR,Viewer")]
        public async Task<IActionResult> GetById(int id)
        {
            var employee = await _service.GetById(id);
            if (employee == null)
                return NotFound(new { message = $"Employee {id} not found" });

            return Ok(employee);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> Add(CreateEmployeeDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _service.Add(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> Update(int id, CreateEmployeeDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _service.Update(id, dto);
            if (updated == null)
                return NotFound(new { message = $"Employee {id} not found" });

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.Delete(id);
            if (!deleted)
                return NotFound(new { message = $"Employee {id} not found" });

            return NoContent();
        }

        [HttpGet("by-department/{department}")]
        [Authorize(Roles = "Admin,HR,Viewer")]

        public async Task<IActionResult> GetByDepartment(string department)
        {
            var result = await _service.GetByDepartment(department);
            return Ok(result);
        }
    }
}