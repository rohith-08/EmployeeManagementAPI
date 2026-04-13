using EmployeeManagementAPI.DTOs;

namespace EmployeeManagementAPI.Services
{
    public interface IEmployeeService
    {
        List<EmployeeDto> GetAll();
        EmployeeDto? GetById(int id);
        EmployeeDto Add(CreateEmployeeDto dto);
        EmployeeDto? Update(int id,CreateEmployeeDto dto);
        bool Delete(int id);
    }
}