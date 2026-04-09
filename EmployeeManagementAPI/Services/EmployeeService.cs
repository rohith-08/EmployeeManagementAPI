using EmployeeManagementAPI.DTOs;
using EmployeeManagementAPI.Models;

namespace EmployeeManagementAPI.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly List<Employee> _employees = new()
        {
            new Employee
            {
                Id = 1,
                Name = "Rohith",
                Email = "rohith@ems.com",
                Department = "IT",
                Designation = "Developer",
                Salary = 50000,
                JoinedDate = DateTime.Now
            },
            new Employee
            {
                Id = 2,
                Name = "Priya",
                Email = "priya@ems.com",
                Department = "HR",
                Designation = "HR Manager",
                Salary = 45000,
                JoinedDate = DateTime.Now
            }
        };

        public List<EmployeeDto> GetAll()
        {
            return _employees.Select(e => new EmployeeDto
            {
                Id = e.Id,
                Name = e.Name,
                Email = e.Email,
                Department = e.Department,
                Designation = e.Designation
            }).ToList();
        }

        public EmployeeDto? GetById(int id)
        {
            var employee = _employees.FirstOrDefault(e => e.Id == id);
            if (employee == null) return null;

            return new EmployeeDto
            {
                Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                Department = employee.Department,
                Designation = employee.Designation
            };
        }
    }
}