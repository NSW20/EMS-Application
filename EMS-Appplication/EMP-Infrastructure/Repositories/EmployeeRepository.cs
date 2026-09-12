using DocumentFormat.OpenXml.Spreadsheet;
using EMP_Infrastructure.SqlOperation;
using EMS_Core.Domain.Entities;
using EMS_Core.Domain.RepositoryContract;
using EMS_Core.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMP_Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EMSDbContext eMSDbContext;
        private readonly ILogger<EmployeeRepository> _logger;
        public EmployeeRepository(EMSDbContext eMSDbContext, ILogger<EmployeeRepository> _logger)
        {
            this.eMSDbContext = eMSDbContext;
            this._logger = _logger;
        }

        public async Task<bool> DeleteEmployee(int empId, CancellationToken token)
        {
            _logger.LogInformation("{class}.{method}.{message}", nameof(EmployeeRepository), nameof(DeleteEmployee), "Remove a employee");
            var employeeToRemove = await eMSDbContext.Employees.FirstOrDefaultAsync(x => x.EmployeeId == empId, token);
            if (employeeToRemove == null)
                return false;
            eMSDbContext.Employees.Remove(employeeToRemove);
            await eMSDbContext.SaveChangesAsync(token);
            return true;
        }

        public async Task<Employee> GetAEmployee(CancellationToken token, string email)
        {
            _logger.LogInformation("{class}.{method}.{message}", nameof(EmployeeRepository), nameof(GetAEmployee), "Get a employee");
            var employee= await eMSDbContext.Employees.FirstOrDefaultAsync(x => x.UserId == email, token);
            return employee;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployees(CancellationToken token)
        {
            _logger.LogInformation("{class}.{method}.{message}", nameof(EmployeeRepository), nameof(GetAllEmployees), "Get all employees");
            var employees = await eMSDbContext.Employees.ToListAsync(token);
            return employees;
        }


        public async Task<Employee> UpdateEmployee(Employee employee, CancellationToken token)
        {
            _logger.LogInformation("{class}.{method}.{message}", nameof(EmployeeRepository), nameof(UpdateEmployee), "Update existing Employee");
            var employeeToBeUpdated = await eMSDbContext.Employees.FirstOrDefaultAsync(x => x.EmployeeId == employee.EmployeeId);
            employeeToBeUpdated.Email = employee.Email;
            employeeToBeUpdated.DesignationId = employee.DesignationId;
            employeeToBeUpdated.DepartmentId = employee.DepartmentId;
            employeeToBeUpdated.FullName = employee.FullName;
            employeeToBeUpdated.DateOfJoining = employee.DateOfJoining;
            employeeToBeUpdated.Status = employee.Status;
            employeeToBeUpdated.Salary = employee.Salary;
            eMSDbContext.Employees.Update(employeeToBeUpdated);
            await eMSDbContext.SaveChangesAsync(token);
            return employeeToBeUpdated;
        }

        public async Task<Employee> AddEmployee(Employee employee, CancellationToken token)
        {
            _logger.LogInformation("{class}.{method}.{message}", nameof(EmployeeRepository), nameof(AddEmployee), "Adding new Employee");
            eMSDbContext.Employees.Add(employee);
            await eMSDbContext.SaveChangesAsync(token);
            return employee;
        }
        public async Task<AppUser> GetUser(string userId)
        {
            _logger.LogInformation("{class}.{method}.{message}", nameof(EmployeeRepository), nameof(GetUser), "FindUserId");
            var result=await eMSDbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
            return result;
        }
        public async Task<IEnumerable<AppUser>> GetAllUsers()
        {
            _logger.LogInformation("{class}.{method}.{message}", nameof(EmployeeRepository), nameof(GetAllUsers), "Get All Users");
            var result = await eMSDbContext.Users.ToListAsync();
            return result;
        }
    }
}
