using EMS_Core.Domain.Entities;
using EMS_Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS_Core.Domain.RepositoryContract
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<Department>> GetAllDepartmentsAsync(CancellationToken token);
        Task<(IEnumerable<Department> department,int pagenumber)> GetAllDepartmentsWithPaginationAsync(CancellationToken token,string? searchText,string sortOrder="ASC", int pageSize=10, int pageNumber=1,string sortColumns="Name");
        Task<Department> GetDepartmentByIdAsync(int id, CancellationToken token);
        Task<Department> AddDepartmentAsync(Department department, CancellationToken token);
        Task<Department> UpdateDepartmentAsync(Department department,int deptId, CancellationToken token);
        Task<bool> DeleteDepartmentAsync(int id, CancellationToken token);
    }
}
