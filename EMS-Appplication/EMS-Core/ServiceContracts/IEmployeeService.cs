using EMS_Core.Domain.Entities;
using EMS_Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS_Core.ServiceContracts
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeDTO>> GetAllEmployees(CancellationToken token);
        Task<EmployeeDTO> GetEmployeeByEmail(CancellationToken token, string email);
        Task<EmployeeDTO> AddEmployee(EmployeeAddDTO employee, CancellationToken token);
        Task<EmployeeDTO> UpdateEmployee(EmployeeDTO employee, CancellationToken token);
        Task<bool> DeleteEmployee(int empId, CancellationToken token);
        Task<AppUser> GetUser(string userId);
        Task<IEnumerable<AppUser>> GetAllUsers();
    }
}
