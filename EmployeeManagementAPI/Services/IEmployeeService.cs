using EmployeeManagementAPI.DTOs;

namespace EmployeeManagementAPI.Services
{
    public interface IEmployeeService
    {
        Task<PagedResultDto<EmployeeDto>> GetAll(int page, int pageSize, string? search);
        Task<EmployeeDto?> GetById(int id);
        Task<EmployeeDto> Add(CreateEmployeeDto dto);
        Task<EmployeeDto?> Update(int id, CreateEmployeeDto dto);
        Task<bool> Delete(int id);
        Task<List<EmployeeDto>> GetByDepartment(string department);
    }
}