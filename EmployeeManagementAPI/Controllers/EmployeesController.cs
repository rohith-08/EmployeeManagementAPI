using EmployeeManagementAPI.Services;
using Microsoft.AspNetCore.Mvc;
using EmployeeManagementAPI.DTOs;
namespace EmployeeManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }
        //https://chatgpt.com/c/69d7fb72-8dc0-8321-8643-125fec0d0b89
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var employees = await _employeeService.GetAll();
            return Ok(employees);
        }

        // GET /api/employees/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var employee = await _employeeService.GetById(id);
            if (employee == null)
                return NotFound(new { message = $"Employee {id} not found" });
            return Ok(employee);
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateEmployeeDto dto)
        {
            if(!ModelState.IsValid)  return BadRequest(ModelState);
            var created = await _employeeService.Add(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateEmployeeDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var updated = await _employeeService.Update(id, dto);
            if (updated == null)
                return NotFound(new { message = $"Employee {id} not found" });
            return Ok(updated);

        }

        [HttpDelete("{id}")]
        public async  Task<IActionResult>  Delete(int id)
        {
            var deleted = await _employeeService.Delete(id);
            if (!deleted)
                return NotFound(new { message = $"Employee {id} not found" });
            return NoContent();
        }

        [HttpGet("by-department/{department}")]
        public async Task<IActionResult> GetByDepartment(string department)
        {
            var employees = await _employeeService.GetByDepartment(department);

            if(!employees.Any())
                return NotFound(new {message = $"No employees found in {department} department "});

            return Ok(employees);

        }
    }
}