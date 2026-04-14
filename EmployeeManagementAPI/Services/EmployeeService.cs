using EmployeeManagementAPI.DTOs;
using EmployeeManagementAPI.Models;

namespace EmployeeManagementAPI.Services
{
    public class EmployeeService : IEmployeeService
    {
        private static List<Employee> _employees = new()
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
        private int _nextId = 3;
        private static EmployeeDto MapToDto(Employee e) => new()
        {
            Id = e.Id,
            Name= e.Name,
            Email = e.Email,
            Department = e.Department,
            Designation= e.Designation,

        };
        public List<EmployeeDto> GetAll() => _employees.Select(MapToDto).ToList();

        public EmployeeDto? GetById(int id)
        {
            var e = _employees.FirstOrDefault(e => e.Id == id);
            return e == null ? null : MapToDto(e);
          //  return MapToDto(e);


        }
        public EmployeeDto Add(CreateEmployeeDto dto)
        {
            var e = new Employee
            {
                Id = _nextId++,
                Name = dto.Name,
                Email = dto.Email,
                Department = dto.Department,
                Designation = dto.Designation,
                Salary= dto.Salary,
                JoinedDate = DateTime.Now
            };
            _employees.Add(e);
            return MapToDto(e);
        }
        public EmployeeDto? Update(int id,CreateEmployeeDto dto)
        {
            var e = _employees.FirstOrDefault(e => e.Id == id);
            if (e == null) return null;
            e.Name = dto.Name;
            e.Email = dto.Email;
            e.Department = dto.Department;
            e.Designation = dto.Designation;
            e.Salary = dto.Salary;
            return MapToDto(e);

        }
        public bool Delete(int id)
        {
            var e = _employees.FirstOrDefault(e => e.Id == id);
            if (e == null) return false;
            _employees.Remove(e);
            return true;
        }
    }
}