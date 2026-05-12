using EmployeeManagementAPI.DTOs;
using EmployeeManagementAPI.Models;
using EmployeeManagementAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace EmployeeManagementAPI.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(
            AppDbContext context,
            ILogger<EmployeeService> logger)
        {
            _context = context;
            _logger = logger;
        }

        private static EmployeeDto MapToDto(Employee e) => new()
        {
            Id = e.Id,
            Name = e.Name,
            Email = e.Email,
            Department = e.Department,
            Designation = e.Designation,
        };

        // ✅ SP: GetAll
        public async Task<List<EmployeeDto>> GetAll()
        {
            _logger.LogInformation("Fetching all employees");

            var employees = await _context.Employees
                .FromSqlRaw("EXEC SP_GetAllEmployees")
                .AsNoTracking()
                .ToListAsync();

            _logger.LogInformation(
                "Returned {Count} employees",
                employees.Count);

            return employees.Select(e => MapToDto(e)).ToList();
        }

        // ✅ SP: GetById
        public async Task<EmployeeDto?> GetById(int id)
        {
            _logger.LogInformation(
                "Fetching employee with Id {EmployeeId}",
                id);

            var param = new SqlParameter("@Id", id);

            var employees = await _context.Employees
                .FromSqlRaw("EXEC SP_GetEmployeeById @Id", param)
                .AsNoTracking()
                .ToListAsync();

            var employee = employees.FirstOrDefault();

            if (employee == null)
            {
                _logger.LogWarning(
                    "Employee with Id {EmployeeId} not found",
                    id);

                return null;
            }

            _logger.LogInformation(
                "Employee with Id {EmployeeId} found",
                id);

            return MapToDto(employee);
        }

        // ✅ SP: GetByDepartment
        public async Task<List<EmployeeDto>> GetByDepartment(string department)
        {
            _logger.LogInformation(
                "Fetching employees from Department {Department}",
                department);

            var param = new SqlParameter("@Department", department);

            var employees = await _context.Employees
                .FromSqlRaw(
                    "EXEC SP_GetEmployeesByDepartment @Department",
                    param)
                .AsNoTracking()
                .ToListAsync();

            _logger.LogInformation(
                "Returned {Count} employees from Department {Department}",
                employees.Count,
                department);

            return employees.Select(e => MapToDto(e)).ToList();
        }

        // ✅ Add
        public async Task<EmployeeDto> Add(CreateEmployeeDto dto)
        {
            _logger.LogInformation(
                "Creating employee {EmployeeName}",
                dto.Name);

            var employee = new Employee
            {
                Name = dto.Name,
                Email = dto.Email,
                Department = dto.Department,
                Designation = dto.Designation,
                Salary = dto.Salary,
                JoinedDate = DateTime.Now
            };

            _context.Employees.Add(employee);

            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Employee created with Id {EmployeeId}",
                employee.Id);

            return MapToDto(employee);
        }

        // ✅ Update
        public async Task<EmployeeDto?> Update(int id, CreateEmployeeDto dto)
        {
            _logger.LogInformation(
                "Updating employee with Id {EmployeeId}",
                id);

            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                _logger.LogWarning(
                    "Update failed. Employee Id {EmployeeId} not found",
                    id);

                return null;
            }

            employee.Name = dto.Name;
            employee.Email = dto.Email;
            employee.Department = dto.Department;
            employee.Designation = dto.Designation;
            employee.Salary = dto.Salary;

            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Employee Id {EmployeeId} updated successfully",
                id);

            return MapToDto(employee);
        }

        // ✅ Delete
        public async Task<bool> Delete(int id)
        {
            _logger.LogInformation(
                "Deleting employee with Id {EmployeeId}",
                id);

            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                _logger.LogWarning(
                    "Delete failed. Employee Id {EmployeeId} not found",
                    id);

                return false;
            }

            _context.Employees.Remove(employee);

            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Employee Id {EmployeeId} deleted successfully",
                id);

            return true;
        }
    }
}