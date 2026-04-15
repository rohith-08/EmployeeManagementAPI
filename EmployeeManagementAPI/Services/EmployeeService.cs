using EmployeeManagementAPI.DTOs;
using EmployeeManagementAPI.Models;
using EmployeeManagementAPI.Data;
using Microsoft.EntityFrameworkCore;
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
        public async Task<List<EmployeeDto>> GetAll()
        {

            return await _context.Employees
                .AsNoTracking()
                .Select(e => MapToDto(e))
                .ToListAsync();


        }
        public async Task<EmployeeDto?> GetById(int id)
        {
            var employee = await _context.Employees
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);

            return employee == null ? null : MapToDto(employee);

        }
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
        public async Task<EmployeeDto?> Update(int id,CreateEmployeeDto dto)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return null;
            employee.Name=dto.Name;
            employee.Email = dto.Email;
            employee.Department = dto.Department;
            employee.Designation = dto.Designation;
            employee.Salary = dto.Salary;



            await _context.SaveChangesAsync();
            return MapToDto(employee);
        }

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