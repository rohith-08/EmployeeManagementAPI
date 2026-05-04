using EmployeeManagementAPI.Models;

namespace EmployeeManagementAPI.Repositories
{
    public interface IEmployeeRepository
    {
        Task<List<Employee>> GetAll();
        Task<Employee?> GetById(int id);
        Task<Employee> Add(Employee employee);
        Task<Employee?> Update(int id, Employee employee);
        Task<bool> Delete(int id);
        Task<List<Employee>> GetByDepartment(string department);

    }
}
