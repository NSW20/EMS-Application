using EMS_Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS_Core.ServiceContracts
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentDTO>> GetAllDepartmentAsync(CancellationToken token);
        Task<DepartmentDTO> GetDepartmentAsync(int id, CancellationToken token);
        Task<DepartmentDTO> AddDepartment(DepartmentAddDTO departmentAddDTO, CancellationToken token);

        Task<DepartmentDTO> UpdateDepartment(DepartmentDTO departmentUpdateDTO,int deptId, CancellationToken token);
        Task<bool> DeleteDepartmentAsync(int departmentId, CancellationToken token);
 
    }
}
