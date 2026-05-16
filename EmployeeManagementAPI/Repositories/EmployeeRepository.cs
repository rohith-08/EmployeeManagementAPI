using EmployeeManagementAPI.Data;
using EmployeeManagementAPI.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementAPI.Repositories
{
    public class EmployeeRepository : IEmployeeRepository

    {
        private readonly AppDbContext _context;
        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(List<Employee> Employees, int TotalCount)> GetAll(
            int page, int pageSize, string? search)
        {
            var query = _context.Employees.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(e =>
                    e.Name.Contains(search) ||
                    e.Email.Contains(search) ||
                    e.Department.Contains(search) ||
                    e.Designation.Contains(search));
            }

            var totalCount = await query.CountAsync();

            var employees = await query
                .OrderBy(e => e.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (employees, totalCount);
        }

        public async Task<Employee?> GetById(int id)
        {
            var param = new SqlParameter("@Id", id);
            var result = await _context.Employees
                .FromSqlRaw("EXEC SP_GetEmployeeByID @Id", param)
                                .AsNoTracking()
                                .ToListAsync();
            return result.FirstOrDefault();

        }
        public async Task<Employee> Add(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task<Employee?> Update(int id, Employee employee)
        {
            var existing = await _context.Employees.FindAsync(id);
            if (existing == null) return null;
            existing.Name = employee.Name;
            existing.Email = employee.Email;
            existing.Department = employee.Department;
            existing.Designation = employee.Designation;
            existing.Salary = employee.Salary;

            await _context.SaveChangesAsync();
            return existing;

        }
        public async Task<bool> Delete(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return false;

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Employee>> GetByDepartment(string department)
        {
            var param = new SqlParameter("@Department", department);
            return await _context.Employees
                .FromSqlRaw("EXEC SP_GetEmployeesByDepartment @Department", param)
                .AsNoTracking()
                .ToListAsync();

        }
    }
}
