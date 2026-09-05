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
        Task<Employee> GetAEmployee(CancellationToken token,int empId);
        Task<Employee> AddEmployee(Employee employee, CancellationToken token);
        Task<Employee> UpdateEmployee(Employee employee, CancellationToken token);
        Task<bool> DeleteEmployee(int empId, CancellationToken token);
        Task<AppUser> GetUser(string userId);
    }
}
