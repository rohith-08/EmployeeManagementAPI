CREATE PROCEDURE SP_GetEmployeesByDepartment
    @Department NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Name, Email, Department, Designation, Salary, JoinedDate
    FROM Employees
    WHERE Department = @Department
    ORDER BY Name ASC
END