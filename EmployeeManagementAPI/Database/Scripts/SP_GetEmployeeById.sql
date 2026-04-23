CREATE PROCEDURE SP_GetEmployeeById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Name, Email, Department, Designation, Salary, JoinedDate
    FROM Employees
    WHERE Id = @Id
END