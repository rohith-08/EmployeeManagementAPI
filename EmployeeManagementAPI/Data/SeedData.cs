using EmployeeManagementAPI.Models;

namespace EmployeeManagementAPI.Data
{
    public static class SeedData
    {
        public static void Initialize(AppDbContext context)
        {
            if (context.Employees.Any()) return;

            var employees = new List<Employee>
            {
                new() { Name="Rohith", Email="rohith@ems.com", Department="IT", Designation="Developer", Salary=50000, JoinedDate=DateTime.Now },
                new() { Name="Priya", Email="priya@ems.com", Department="HR", Designation="HR Manager", Salary=45000, JoinedDate=DateTime.Now },
                new() { Name="Suresh", Email="suresh@ems.com", Department="Finance", Designation="Analyst", Salary=40000, JoinedDate=DateTime.Now },
                new() { Name="Anitha", Email="anitha@ems.com", Department="IT", Designation="QA Engineer", Salary=42000, JoinedDate=DateTime.Now },
                new() { Name="Kiran", Email="kiran@ems.com", Department="IT", Designation="Senior Developer", Salary=70000, JoinedDate=DateTime.Now },
                new() { Name="Meena", Email="meena@ems.com", Department="HR", Designation="Recruiter", Salary=38000, JoinedDate=DateTime.Now },
                new() { Name="Vijay", Email="vijay@ems.com", Department="Finance", Designation="Accountant", Salary=36000, JoinedDate=DateTime.Now },
                new() { Name="Deepa", Email="deepa@ems.com", Department="IT", Designation="DevOps Engineer", Salary=65000, JoinedDate=DateTime.Now },
                new() { Name="Ravi", Email="ravi@ems.com", Department="IT", Designation="Tech Lead", Salary=85000, JoinedDate=DateTime.Now },
                new() { Name="Lakshmi", Email="lakshmi@ems.com", Department="HR", Designation="HR Director", Salary=90000, JoinedDate=DateTime.Now }
            };

            context.Employees.AddRange(employees);
            context.SaveChanges();
        }
    }
}
