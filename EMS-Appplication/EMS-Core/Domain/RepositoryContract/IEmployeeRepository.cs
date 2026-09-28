using EMS_Core.Domain.Entities;
using EMS_Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS_Core.Domain.RepositoryContract
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllEmployees(CancellationToken token);
        Task<(IEnumerable<Employee>,int)> GetAllEmployeesWithPagination(CancellationToken token, string? searchText,int pageNumber=1,int TotalItems=10,string sortColumn= "FullName",string sortOrder="ASC");
        Task<Employee> GetAEmployee(CancellationToken token,string email);
        Task<Employee> AddEmployee(Employee employee, CancellationToken token);
        Task<Employee> UpdateEmployee(Employee employee, CancellationToken token);
        Task<bool> DeleteEmployee(int empId, CancellationToken token);
        Task<AppUser> GetUser(string userId);
        Task<AppUser> GetUserByUserId(string userId);
        Task<IEnumerable<AppUser>> GetAllUsers();
    }
}
