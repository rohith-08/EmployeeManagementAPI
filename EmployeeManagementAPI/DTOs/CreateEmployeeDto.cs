using System.ComponentModel.DataAnnotations;
namespace EmployeeManagementAPI.DTOs
{
    public class CreateEmployeeDto
    {
        [Required][StringLength(100,MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;
        [Required][EmailAddress]
       public string Email { get; set; }=string.Empty;
        [Required]
        public string Department { get; set; }=string.Empty;
        [Required]
        public string Designation { get; set; } = string.Empty;
        [Range(10000, 1000000)]
        public decimal Salary { get; set; }
    }
}
