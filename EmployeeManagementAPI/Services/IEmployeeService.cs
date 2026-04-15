using EmployeeManagementAPI.DTOs;

namespace EmployeeManagementAPI.Services
{
    public interface IEmployeeService
    {
        Task<List<EmployeeDto>> GetAll();
       Task<EmployeeDto?> GetById(int id);
       Task<EmployeeDto> Add(CreateEmployeeDto dto);
        Task<EmployeeDto?> Update(int id,CreateEmployeeDto dto);
        Task<bool> Delete(int id);
    }
}