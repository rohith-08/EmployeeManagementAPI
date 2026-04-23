using EmployeeManagementAPI.DTOs;
using EmployeeManagementAPI.Models;
using EmployeeManagementAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace EmployeeManagementAPI.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly AppDbContext _context;

        public EmployeeService(AppDbContext context)
        {
            _context = context;
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
            var employees = await _context.Employees
         .FromSqlRaw("EXEC SP_GetAllEmployees")
         .AsNoTracking()
         .ToListAsync();

            return employees.Select(e => MapToDto(e)).ToList();
        }

        // ✅ SP: GetById
        public async Task<EmployeeDto?> GetById(int id)
        {
            var param = new SqlParameter("@Id", id);

            var employees = await _context.Employees
                .FromSqlRaw("EXEC SP_GetEmployeeById @Id", param)
                .AsNoTracking()
                .ToListAsync();

            var employee = employees.FirstOrDefault();
            return employee == null ? null : MapToDto(employee);
        }

        // ✅ SP: GetByDepartment (Day 5)
        public async Task<List<EmployeeDto>> GetByDepartment(string department)
        {
            var param = new SqlParameter("@Department", department);

            var employees = await _context.Employees
                .FromSqlRaw("EXEC SP_GetEmployeesByDepartment @Department", param)
                .AsNoTracking()
                .ToListAsync();   // ✅ fetch first

            // ✅ then map in memory
            return employees.Select(e => MapToDto(e)).ToList();

        }

        // ✅ Add
        public async Task<EmployeeDto> Add(CreateEmployeeDto dto)
        {
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
            return MapToDto(employee);
        }

        // ✅ Update
        public async Task<EmployeeDto?> Update(int id, CreateEmployeeDto dto)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return null;

            employee.Name = dto.Name;
            employee.Email = dto.Email;
            employee.Department = dto.Department;
            employee.Designation = dto.Designation;
            employee.Salary = dto.Salary;

            await _context.SaveChangesAsync();
            return MapToDto(employee);
        }

        // ✅ Delete
        public async Task<bool> Delete(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return false;

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}