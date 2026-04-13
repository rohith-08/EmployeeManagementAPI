using EmployeeManagementAPI.Services;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult GetAll()
        {
           
            return Ok(_employeeService.GetAll());
        }

        // GET /api/employees/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var e = _employeeService.GetById(id);
           return e == null ? NotFound(new {message = $"Employee {id} not found"}): Ok(e);
        }
        [HttpPost]
        public IActionResult Add([FromBody] CreateEmployeeDto dto)
        {
            if(!ModelState.IsValid)  return BadRequest(ModelState);
            var created = _employeeService.Add(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] CreateEmployeeDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var updated = _employeeService.Update(id, dto);
            return updated == null ? NotFound(new {message = $"Employee {id} not found"}) : Ok(updated);

        }

        [HttpDelete("{id}")]
        public IActionResult  Delete(int id)
        {
            var deleted = _employeeService.Delete(id);
            return deleted ? NoContent() : NotFound(new { message = $"Employee {id} not found" });
        }
    }
}