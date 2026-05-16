using EmployeeManagementAPI.DTOs;
using EmployeeManagementAPI.Models;
using EmployeeManagementAPI.Repositories;
using Microsoft.Extensions.Logging;

namespace EmployeeManagementAPI.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repository;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(IEmployeeRepository repository,
            ILogger<EmployeeService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<PagedResultDto<EmployeeDto>> GetAll(int page, int pageSize, string? search)
        {
            _logger.LogInformation("Fetching employees — Page: {Page} PageSize: {PageSize} Search: {Search}", page, pageSize, search);
            var (employees, totalCount) = await _repository.GetAll(page, pageSize, search);
            return new PagedResultDto<EmployeeDto>
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
                Data = employees.Select(MapToDto).ToList()
            };
        }

        public async Task<EmployeeDto?> GetById(int id)
        {
            _logger.LogInformation("Fetching employee Id {EmployeeId}", id);
            var employee = await _repository.GetById(id);
            if (employee == null)
            {
                _logger.LogWarning("Employee Id {EmployeeId} not found", id);
                return null;
            }
            return MapToDto(employee);
        }

        public async Task<EmployeeDto> Add(CreateEmployeeDto dto)
        {
            _logger.LogInformation("Creating employee: {Name}", dto.Name);
            var employee = new Employee
            {
                Name = dto.Name,
                Email = dto.Email,
                Department = dto.Department,
                Designation = dto.Designation,
                Salary = dto.Salary,
                JoinedDate = DateTime.Now
            };
            var created = await _repository.Add(employee);
            _logger.LogInformation("Employee created Id {EmployeeId}", created.Id);
            return MapToDto(created);
        }

        public async Task<EmployeeDto?> Update(int id, CreateEmployeeDto dto)
        {
            _logger.LogInformation("Updating employee Id {EmployeeId}", id);
            var employee = new Employee
            {
                Name = dto.Name,
                Email = dto.Email,
                Department = dto.Department,
                Designation = dto.Designation,
                Salary = dto.Salary
            };
            var updated = await _repository.Update(id, employee);
            if (updated == null)
            {
                _logger.LogWarning("Update failed — Id {EmployeeId} not found", id);
                return null;
            }
            _logger.LogInformation("Employee Id {EmployeeId} updated", id);
            return MapToDto(updated);
        }

        public async Task<bool> Delete(int id)
        {
            _logger.LogInformation("Deleting employee Id {EmployeeId}", id);
            var result = await _repository.Delete(id);
            if (!result)
                _logger.LogWarning("Delete failed — Id {EmployeeId} not found", id);
            else
                _logger.LogInformation("Employee Id {EmployeeId} deleted", id);
            return result;
        }

        public async Task<List<EmployeeDto>> GetByDepartment(string department)
        {
            _logger.LogInformation("Fetching department: {Department}", department);
            var employees = await _repository.GetByDepartment(department);
            return employees.Select(MapToDto).ToList();
        }

        private static EmployeeDto MapToDto(Employee e) => new()
        {
            Id = e.Id,
            Name = e.Name,
            Email = e.Email,
            Department = e.Department,
            Designation = e.Designation
        };
    }
}