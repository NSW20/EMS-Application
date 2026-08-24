using EMS_Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS_Core.Domain.RepositoryContract
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<Department>> GetAllDepartmentsAsync(CancellationToken token);
        Task<Department> GetDepartmentByIdAsync(int id, CancellationToken token);
        Task<Department> AddDepartmentAsync(Department department, CancellationToken token);
        Task<Department> UpdateDepartmentAsync(Department department,int deptId, CancellationToken token);
        Task<bool> DeleteDepartmentAsync(int id, CancellationToken token);
    }
}
