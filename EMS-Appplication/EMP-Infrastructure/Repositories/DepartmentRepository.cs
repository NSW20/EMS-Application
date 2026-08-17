using EMP_Infrastructure.SqlOperation;
using EMS_Core.Domain.Entities;
using EMS_Core.Domain.RepositoryContract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMP_Infrastructure.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly EMSDbContext _context;
        private readonly ILogger<DepartmentRepository> _logger;
        public DepartmentRepository(EMSDbContext _context, ILogger<DepartmentRepository> _logger)
        {
            this._context = _context;
            this._logger = _logger;
        }
        public async Task<Department> AddDepartmentAsync(Department department)
        {
            _logger.LogInformation("{method}.{class}.Requested Recived to Add a department", nameof(AddDepartmentAsync), nameof(DepartmentRepository));
            await _context.Departments.AddAsync(department);
            await _context.SaveChangesAsync();
            return department;
        }

        public async Task<bool> DeleteDepartmentAsync(int id)
        {
            _logger.LogInformation("{method}.{class}.Requested Recived to delete a department", nameof(DeleteDepartmentAsync), nameof(DepartmentRepository));
            var departmentToDelete= await _context.Departments.FirstOrDefaultAsync(x => x.DepartmentId == id);
            if(departmentToDelete != null)
            {
                 _context.Departments.Remove(departmentToDelete);
                 await _context.SaveChangesAsync();
            }
            return true;
        }

        public async Task<IEnumerable<Department>> GetAllDepartmentsAsync()
        {
            _logger.LogInformation("{method}.{class}.Requested Recived to fetch all the department", nameof(GetAllDepartmentsAsync), nameof(DepartmentRepository));
            return await _context.Departments.ToListAsync();
        }

        public async Task<Department> GetDepartmentByIdAsync(int id)
        {
            _logger.LogInformation("{method}.{class}.Requested Recived to fetch a department",nameof(GetDepartmentByIdAsync),nameof(DepartmentRepository));
            return await _context.Departments.FirstOrDefaultAsync(x=>x.DepartmentId== id);
        }

        public async Task<Department> UpdateDepartmentAsync(Department department, int deptId)
        {
            _logger.LogInformation("{method}.{class}.Requested Recived to update a department", nameof(UpdateDepartmentAsync), nameof(DepartmentRepository));
             var departmentToUpdate = await _context.Departments.FirstOrDefaultAsync(x => x.DepartmentId == deptId);
            departmentToUpdate.Description=department.Description;
            departmentToUpdate.Name=department.Name;
             _context.Departments.Update(departmentToUpdate);
            await _context.SaveChangesAsync();
            return departmentToUpdate;
        }
    }
}
