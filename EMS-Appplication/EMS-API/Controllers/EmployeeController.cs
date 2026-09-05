using EMS_Core.Domain.Entities;
using EMS_Core.DTOs;
using EMS_Core.ReponseWrapper;
using EMS_Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Caching.Memory;

namespace EMS_API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService employeeService;
        private readonly ILogger<EmployeeController> _logger;
        private readonly IMemoryCache memoryCache;
        private readonly SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);
        public EmployeeController(IEmployeeService employeeService, ILogger<EmployeeController> _logger, IMemoryCache memoryCache)
        {
            this.employeeService = employeeService;
            this._logger = _logger;
            this.memoryCache = memoryCache;
        }

        [HttpGet]
        [Authorize(Roles ="Admin")]
        [EnableRateLimiting("rateLimiter")]
        public async Task<ActionResult<APIResponseWrapper<IEnumerable<EmployeeDTO>>>> GetAllEmployee(CancellationToken token)
        {
            _logger.LogInformation("{controller}.{Method}.{message}", nameof(EmployeeController), nameof(GetAllEmployee), "Request received to fetch all the employees");
            if(memoryCache.TryGetValue("cached",out IEnumerable<EmployeeDTO> employees))
            {
                _logger.LogInformation("{controller}.{Method}.{message}", nameof(EmployeeController), nameof(GetAllEmployee), "Employees fetched from memory cache");
                return StatusCode(StatusCodes.Status200OK, new APIResponseWrapper<IEnumerable<EmployeeDTO>>
                {
                    Data = employees,
                    Message = "All the employees have been fetched successfully.",
                    StatusCode = StatusCodes.Status200OK

                });
            }
            await semaphoreSlim.WaitAsync(token);
            try
            {
                var result = await employeeService.GetAllEmployees(token);
                var memoryEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(1)).SetSlidingExpiration(TimeSpan.FromMinutes(10));
                memoryCache.Set("cached", result, memoryEntryOptions);
                _logger.LogInformation("{controller}.{Method}.{message}", nameof(EmployeeController), nameof(GetAllEmployee), "Employees cached in memory");
                return StatusCode(StatusCodes.Status200OK, new APIResponseWrapper<IEnumerable<EmployeeDTO>>
                {
                    Data = result,
                    Message = "All the employees have been fetched successfully.",
                    StatusCode = StatusCodes.Status200OK

                });
            }
            finally
            {
                semaphoreSlim.Release();
            }
            
        }
        [HttpGet]
        [Authorize(Roles = "Admin,Employee")]
        [EnableRateLimiting("rateLimiter")]
        public async Task<ActionResult<APIResponseWrapper<EmployeeDTO>>> FindEmployeeById([FromQuery] int empId,CancellationToken token)
        {
            _logger.LogInformation("{controller}.{Method}.{message}", nameof(EmployeeController), nameof(FindEmployeeById), "Request received to fetch employee by Id");
            var result = await employeeService.GetEmployeeById(token, empId);
            if(result is null)
            {
                _logger.LogInformation("{controller}.{Method}.{message}", nameof(EmployeeController), nameof(FindEmployeeById), "Employee does not exists.Please check the employee Id");
                return StatusCode(StatusCodes.Status200OK, new APIResponseWrapper<EmployeeDTO>
                {
                    Data = null,
                    Message = "Employee does not exists.Please check the employee Id",
                    StatusCode = StatusCodes.Status200OK

                });
            }
            _logger.LogInformation("{controller}.{Method}.{message}", nameof(EmployeeController), nameof(FindEmployeeById), "Employee details has been fetched successfully.");
            return StatusCode(StatusCodes.Status200OK, new APIResponseWrapper<EmployeeDTO>
            {
                Data = result,
                Message = "Employee details has been fetched successfully.",
                StatusCode = StatusCodes.Status200OK

            });
        }
        [HttpPost]
        [Authorize(Roles = "Admin,Employee")]
        [EnableRateLimiting("rateLimiter")]
        public async Task<ActionResult<APIResponseWrapper<EmployeeDTO>>> AddEmployee([FromBody]EmployeeAddDTO employeeAddDTO,CancellationToken token)
        {
            memoryCache.Remove("cached");
            _logger.LogInformation("{controller}.{Method}.{message}", nameof(EmployeeController), nameof(AddEmployee), "Request received to add a employee");
            var result = await employeeService.AddEmployee(employeeAddDTO, token);
            if(result is null)
            {
                _logger.LogError("{controller}.{Method}.{message}", nameof(EmployeeController), nameof(AddEmployee), "Some Error occured while adding employee");
                return StatusCode(StatusCodes.Status400BadRequest, new APIResponseWrapper<EmployeeDTO>
                {
                    Data = null,
                    Message = "Some Error occured while adding employee",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
            _logger.LogInformation("{controller}.{Method}.{message}", nameof(EmployeeController), nameof(AddEmployee), "Employee has been added successfully.");
            return StatusCode(StatusCodes.Status200OK, new APIResponseWrapper<EmployeeDTO>
            {
                Data = result,
                Message = "Employee has been added successfully.",
                StatusCode = StatusCodes.Status200OK

            });
        }
        [HttpPut]
        [Authorize(Roles = "Admin,Employee")]
        [EnableRateLimiting("rateLimiter")]
        public async Task<ActionResult<APIResponseWrapper<EmployeeDTO>>> UpdateEmployee([FromBody] EmployeeDTO employeeDTO, CancellationToken token)
        {
            memoryCache.Remove("cached");
            _logger.LogInformation("{controller}.{Method}.{message}", nameof(EmployeeController), nameof(UpdateEmployee), "Request received to update a employee");
            var result = await employeeService.UpdateEmployee(employeeDTO, token);
            if (result is null)
            {
                _logger.LogError("{controller}.{Method}.{message}", nameof(EmployeeController), nameof(UpdateEmployee), "Some Error occured while updating employee");
                return StatusCode(StatusCodes.Status400BadRequest, new APIResponseWrapper<EmployeeDTO>
                {
                    Data = null,
                    Message = "Some Error occured while updating employee",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
            _logger.LogInformation("{controller}.{Method}.{message}", nameof(EmployeeController), nameof(UpdateEmployee), "Employee has been updated successfully.");
            return StatusCode(StatusCodes.Status200OK, new APIResponseWrapper<EmployeeDTO>
            {
                Data = result,
                Message = "Employee has been updated successfully.",
                StatusCode = StatusCodes.Status200OK

            });
        }
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [EnableRateLimiting("rateLimiter")]
        public async Task<ActionResult<APIResponseWrapper<bool>>> RemoveEmployee([FromQuery]int empId, CancellationToken token)
        {
          _logger.LogInformation("{controller}.{Method}.{message}", nameof(EmployeeController), nameof(RemoveEmployee), "Request received to remove a employee");
            var result = await employeeService.DeleteEmployee(empId, token);
            if (!result)
            {
                _logger.LogError("{controller}.{Method}.{message}", nameof(EmployeeController), nameof(RemoveEmployee), "Some Error occured while removing employee");
                return StatusCode(StatusCodes.Status400BadRequest, new APIResponseWrapper<bool>
                {
                    Data = false,
                    Message = "Some Error occured while removing employee",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
            memoryCache.Remove("cached");
            _logger.LogInformation("{controller}.{Method}.{message}", nameof(EmployeeController), nameof(RemoveEmployee), "Employee has been removed successfully.");
            return StatusCode(StatusCodes.Status200OK, new APIResponseWrapper<bool>
            {
                Data = true,
                Message = "Employee has been removed successfully.",
                StatusCode = StatusCodes.Status200OK

            });
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [EnableRateLimiting("rateLimiter")]
        public async Task<ActionResult<APIResponseWrapper<AppUser>>> GetUserDetails([FromQuery] string email)
        {
            _logger.LogInformation("{controller}.{Method}.{message}", nameof(EmployeeController), nameof(GetUserDetails), "Request received to fetch user details");
            var result = await employeeService.GetUser(email);
            if (result == null)
            {
                _logger.LogError("{controller}.{Method}.{message}", nameof(EmployeeController), nameof(GetUserDetails), "Some Error occured while fetching user details");
                return StatusCode(StatusCodes.Status400BadRequest, new APIResponseWrapper<AppUser>()
                {
                    Data = null,
                    Message = "Some Error occured while fetching user details",
                    StatusCode = 400
                });
            }
            _logger.LogInformation("{controller}.{Method}.{message}", nameof(EmployeeController), nameof(GetUserDetails), "User details has been fetched successfully.");

            return StatusCode(StatusCodes.Status200OK, new APIResponseWrapper<AppUser>()
            {
                Data = result,
                Message = "User details has been fetched successfully.",
                StatusCode = 200
            });
        }

    }
}
