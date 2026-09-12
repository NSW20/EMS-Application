using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using EMS_Core.Domain.Entities;
using EMS_Core.Domain.RepositoryContract;
using EMS_Core.DTOs;
using EMS_Core.ServiceContracts;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS_Core.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ILogger<EmployeeService> _logger;
        private readonly IMapper _mapper;
        private readonly IEmployeeRepository employeeRepository;
        public EmployeeService(ILogger<EmployeeService> _logger, IMapper _mapper, IEmployeeRepository employeeRepository)
        {
            this._logger = _logger;
            this._mapper = _mapper;
            this.employeeRepository = employeeRepository;
        }
        public async Task<EmployeeDTO> AddEmployee(EmployeeAddDTO employee, CancellationToken token)
        {
            _logger.LogInformation("{class}.{method}.{message}", nameof(EmployeeService), nameof(AddEmployee), "Add new Employee");
            if(employee is null)
            {
                _logger.LogError("{class}.{method}.{message}", nameof(EmployeeService), nameof(AddEmployee), "Please provide mandatory inputs");
                throw new ArgumentNullException(nameof(employee),"Please provide mandatory inputs");
            }
            var employeEntity = _mapper.Map<Employee>(employee);
            var result = await employeeRepository.AddEmployee(employeEntity, token);
            if(result is null)
            {
                _logger.LogError("{class}.{method}.{message}", nameof(EmployeeService), nameof(AddEmployee), "Unable to add employee. Please try again later.");
                throw new InvalidOperationException("Unable to add employee. Please try again later.");
            }
            var employeeDTO = _mapper.Map<EmployeeDTO>(result);
            return employeeDTO;

        }

        public async Task<bool> DeleteEmployee(int empId, CancellationToken token)
        {
            _logger.LogInformation("{class}.{method}.{message}", nameof(EmployeeService), nameof(DeleteEmployee), "Remove an Employee");
            if (empId <= 0)
            {
                _logger.LogError("{class}.{method}.{message}", nameof(EmployeeService), nameof(DeleteEmployee), "Please enter a valid employee Id");
                throw new ArgumentException("Please enter a valid employee Id", nameof(empId));
            }
            var empToBeRemoved = await employeeRepository.DeleteEmployee(empId, token);
            if (empToBeRemoved)
            {
                _logger.LogInformation("{class}.{method}.{message}", nameof(EmployeeService), nameof(DeleteEmployee), "Employee has been removed successfully");
                return true;
            }
            _logger.LogError("{class}.{method}.{message}", nameof(EmployeeService), nameof(DeleteEmployee),"Unable to remove employee. Please try again later.");
            return false;
        }

        public async Task<EmployeeDTO> GetEmployeeByEmail(CancellationToken token, string email)
        {

            _logger.LogInformation("{class}.{method}.{message}", nameof(EmployeeService), nameof(GetEmployeeByEmail), "Find a Employee by employee id");
            if (string.IsNullOrEmpty(email))
            {
                _logger.LogError("{class}.{method}.{message}", nameof(EmployeeService), nameof(GetEmployeeByEmail), "Please enter a valid employee Id");
                throw new ArgumentException("Please enter a valid employee Id", nameof(email));
            }
            var fetchedEmployee = await employeeRepository.GetAEmployee (token, email);
            if (fetchedEmployee is null)
            {
                _logger.LogInformation("{class}.{method}.{message}", nameof(EmployeeService), nameof(GetEmployeeByEmail), "Employee doesn't exists");
                return null;
            }
            else
            {
                _logger.LogInformation("{class}.{method}.{message}", nameof(EmployeeService), nameof(GetEmployeeByEmail), "Employee has been fetched successfully");
                var result = _mapper.Map<EmployeeDTO>(fetchedEmployee);
                return result;
            }
        }

        public async Task<IEnumerable<EmployeeDTO>> GetAllEmployees(CancellationToken token)
        {
            _logger.LogInformation("{class}.{method}.{message}", nameof(EmployeeService), nameof(GetAllEmployees), "Fetch all the employees");
            var result = await employeeRepository.GetAllEmployees(token);
            if (!result.Any())
            {
                _logger.LogInformation("{class}.{method}.{message}", nameof(EmployeeService), nameof(GetAllEmployees), "No records found");
                return new List<EmployeeDTO>();
            }
            else
            {
                _logger.LogInformation("{class}.{method}.{message}", nameof(EmployeeService), nameof(GetAllEmployees), "All the employees has been fetched successfully");
                var resultInDto = _mapper.Map<IEnumerable<EmployeeDTO>>(result);
                return resultInDto;
            }

        }

        public async Task<EmployeeDTO> UpdateEmployee(EmployeeDTO employee, CancellationToken token)
        {
            _logger.LogInformation("{class}.{method}.{message}", nameof(EmployeeService), nameof(UpdateEmployee), "Update a Employee");
            if (employee is null)
            {
                _logger.LogError("{class}.{method}.{message}", nameof(EmployeeService), nameof(UpdateEmployee), "Please provide mandatory inputs");
                throw new ArgumentNullException(nameof(employee), "Please provide mandatory inputs");
            }
            var employeEntity = _mapper.Map<Employee>(employee);
            var result = await employeeRepository.UpdateEmployee(employeEntity, token);
            if (result is null)
            {
                _logger.LogError("{class}.{method}.{message}", nameof(EmployeeService), nameof(UpdateEmployee), "Unable to update employee. Please try again later.");
                throw new InvalidOperationException("Unable to update employee. Please try again later.");
            }
            var employeeDTO = _mapper.Map<EmployeeDTO>(result);
            return employeeDTO;
        }
        public async Task<AppUser> GetUser(string userId)
        {
            _logger.LogInformation("{class}.{method}.{message}", nameof(EmployeeService), nameof(GetUser), "Find user details");
            if (userId is null)
            {
                _logger.LogError("{class}.{method}.{message}", nameof(EmployeeService), nameof(GetUser), "Please enter a valid user Id");
                throw new ArgumentException("Please enter a valid employee Id", nameof(userId));
            }
            var result = await employeeRepository.GetUser(userId);
            if (result==null)
            {
                _logger.LogInformation("{class}.{method}.{message}", nameof(EmployeeService), nameof(GetUser), "No records found");
                return new AppUser();
            }
            else
            {
                _logger.LogInformation("{class}.{method}.{message}", nameof(EmployeeService), nameof(GetUser), "User details has been fetched successfully");
                return result;
            }
        }
        public async Task<IEnumerable<AppUser>> GetAllUsers()
        {
            _logger.LogInformation("{class}.{method}.{message}", nameof(EmployeeService), nameof(GetAllUsers), "Get All Users Details");
          
            var result = await employeeRepository.GetAllUsers();
            if (!result.Any())
            {
                _logger.LogInformation("{class}.{method}.{message}", nameof(EmployeeService), nameof(GetAllUsers), "No records found");
                return Enumerable.Empty<AppUser>();
            }
            else
            {
                _logger.LogInformation("{class}.{method}.{message}", nameof(EmployeeService), nameof(GetUser), "User details has been fetched successfully");
                return result;
            }
        }
    }
}
