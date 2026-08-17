using EMS_Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS_Core.Domain.RepositoryContract
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<Department>> GetAllDepartmentsAsync();
        Task<Department> GetDepartmentByIdAsync(int id);
        Task<Department> AddDepartmentAsync(Department department);
        Task<Department> UpdateDepartmentAsync(Department department,int deptId);
        Task<bool> DeleteDepartmentAsync(int id);
    }
}
