using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementAPI.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [MaxLength(150)]
        public string Email { get; set; } = string.Empty;
        [MaxLength(100)]
        public string Department { get; set; } = string.Empty;
        [MaxLength(100)]
        public string Designation { get; set; } = string.Empty;
        [Column(TypeName = "decimal(18,2)")]
        public decimal Salary { get; set; }
        public DateTime JoinedDate { get; set; }
    }
}