using EMS_Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS_Core.ServiceContracts
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentDTO>> GetAllDepartmentAsync();
        Task<DepartmentDTO> GetDepartmentAsync(int id);
        Task<DepartmentDTO> AddDepartment(DepartmentAddDTO departmentAddDTO);

        Task<DepartmentDTO> UpdateDepartment(DepartmentDTO departmentUpdateDTO,int deptId);
        Task<bool> DeleteDepartmentAsync(int departmentId);
    }
}
