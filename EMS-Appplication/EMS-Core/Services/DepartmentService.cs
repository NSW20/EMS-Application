using AutoMapper;
using EMS_Core.Domain.Entities;
using EMS_Core.Domain.RepositoryContract;
using EMS_Core.DTOs;
using EMS_Core.ServiceContracts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS_Core.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IMapper _mapper;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ILogger<DepartmentService> _logger;
        public DepartmentService(IMapper _mapper, IDepartmentRepository _departmentRepository, ILogger<DepartmentService> _logger)
        {
            this._departmentRepository = _departmentRepository;
            this._mapper = _mapper;
            this._logger = _logger;
        }
        public async Task<DepartmentDTO> AddDepartment(DepartmentAddDTO departmentAddDTO)
        {
            _logger.LogInformation("{method}.{class}.Request received for adding department", nameof(AddDepartment), nameof(DepartmentService));
            if(departmentAddDTO is null)
            {
                _logger.LogWarning("{method}.{class}.Invalid Inputs", nameof(AddDepartment), nameof(DepartmentService));
                throw new ArgumentNullException(nameof(departmentAddDTO), "Invalid Inputs");
            }
            var deptToBeAdded = _mapper.Map<Department>(departmentAddDTO);
            var result = await _departmentRepository.AddDepartmentAsync(deptToBeAdded);
            if(result is null)
            {
                _logger.LogError("{method}.{class}.Something went wrong while adding department", nameof(AddDepartment), nameof(DepartmentService));
                throw new InvalidOperationException("Something went wrong while adding department");
            }
            var deptToBeReturned = _mapper.Map<DepartmentDTO>(result);
            _logger.LogInformation("{method}.{class}.Department has been added successfully", nameof(AddDepartment), nameof(DepartmentService));
            return deptToBeReturned;

        }

        public async Task<bool> DeleteDepartmentAsync(int departmentId)
        {
            _logger.LogInformation("{method}.{class}.Request received for deleting department", nameof(DeleteDepartmentAsync), nameof(DepartmentService));
            if (departmentId<=0)
            {
                _logger.LogWarning("{method}.{class}.Invalid Inputs", nameof(DeleteDepartmentAsync), nameof(DepartmentService));
                throw new ArgumentException("Invalid Inputs", nameof(departmentId));
            }
            var result = await _departmentRepository.DeleteDepartmentAsync(departmentId);
            if (!result)
            {
                _logger.LogError("{method}.{class}.Something went wrong while deleting department", nameof(DeleteDepartmentAsync), nameof(DepartmentService));
                throw new InvalidOperationException("Something went wrong while deleting department");
            }
            _logger.LogInformation("{method}.{class}.Department has been deleted successfully", nameof(DeleteDepartmentAsync), nameof(DepartmentService));
            return result;
        }

        public async Task<IEnumerable<DepartmentDTO>> GetAllDepartmentAsync()
        {
            _logger.LogInformation("{method}.{class}.Request received for Fetch all department", nameof(GetAllDepartmentAsync), nameof(DepartmentService));
         
            var result = await _departmentRepository.GetAllDepartmentsAsync();
            if (result is null)
            {
                _logger.LogError("{method}.{class}.Something went wrong while fetching department", nameof(GetAllDepartmentAsync), nameof(DepartmentService));
                throw new InvalidOperationException("Something went wrong while fetching department");
            }
            if (!result.Any())
            {
                _logger.LogInformation("{method}.{class}.There are no Department exists", nameof(GetAllDepartmentAsync), nameof(DepartmentService));
                return Enumerable.Empty<DepartmentDTO>();
            }
            _logger.LogInformation("{method}.{class}.Department fetched successfully", nameof(GetAllDepartmentAsync), nameof(DepartmentService));
            var resultToBeReturned = _mapper.Map<IEnumerable<DepartmentDTO>>(result);
            return resultToBeReturned;
        }

        public async Task<DepartmentDTO> GetDepartmentAsync(int id)
        {
            _logger.LogInformation("{method}.{class}.Request received for Fetch an department", nameof(GetDepartmentAsync), nameof(DepartmentService));

            if (id <= 0)
            {
                _logger.LogWarning("{method}.{class}.Invalid Inputs", nameof(GetDepartmentAsync), nameof(DepartmentService));
                throw new ArgumentException("Invalid Inputs", nameof(id));
            }
            var result = await _departmentRepository.GetDepartmentByIdAsync(id);
            if (result is null)
            {
                _logger.LogError("{method}.{class}.Department not found", nameof(GetDepartmentAsync), nameof(DepartmentService));
                throw new KeyNotFoundException($"No department found with id {id}");
            }
            var resultToBeReturned = _mapper.Map<DepartmentDTO>(result);
            return resultToBeReturned;
        }

        public async Task<DepartmentDTO> UpdateDepartment(DepartmentDTO departmentUpdateDTO, int deptId)
        {
            _logger.LogInformation("{method}.{class}.Requested receive to update a department", nameof(UpdateDepartment), nameof(DepartmentService));
            if (deptId <= 0)
            {
                _logger.LogWarning("{method}.{class}.Please provide a correct department id", nameof(UpdateDepartment), nameof(DepartmentService));
                throw new ArgumentException("Please provide the correct input", nameof(deptId));
            }
            if (departmentUpdateDTO is null)
            {
                _logger.LogWarning("{method}.{class}.Please provide the required inputs", nameof(UpdateDepartment), nameof(DepartmentService));
                throw new ArgumentNullException(nameof(departmentUpdateDTO), "Please provide the required inputs");
            }
            var departmentToUpdate = _mapper.Map<Department>(departmentUpdateDTO);
            var result = await _departmentRepository.UpdateDepartmentAsync(departmentToUpdate, deptId);
            if (result is null)
            {
                _logger.LogError("{method}.{class}.Something went wrong while updating department", nameof(UpdateDepartment), nameof(DepartmentService));
                throw new InvalidOperationException("Something went wrong while updating department");
            }
            var resultUpdate = _mapper.Map<DepartmentDTO>(result);
            _logger.LogInformation("{method}.{class}.Departments updated successfully", nameof(UpdateDepartment), nameof(DepartmentService));
            return resultUpdate;

        }
    }
}
