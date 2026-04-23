CREATE PROCEDURE SP_GetAllEmployees
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Name, Email, Department, Designation, Salary, JoinedDate
    FROM Employees
    ORDER BY Name ASC
END