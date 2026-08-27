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
    public class DepartmentController : ControllerBase
    {
        private readonly IMemoryCache memoryCache;
        private readonly ILogger<DepartmentController> _logger;
        private readonly IDepartmentService departmentService;
        private readonly SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);
        public DepartmentController(IMemoryCache memoryCache, ILogger<DepartmentController> _logger, IDepartmentService departmentService)
        {
            this.memoryCache = memoryCache;
            this._logger = _logger;
            this.departmentService = departmentService;
        }

        [HttpGet]
        [EnableRateLimiting("rateLimiter")]
        [Authorize(Roles ="Admin")]
        public async Task<APIResponseWrapper<IEnumerable<DepartmentDTO>>> GetAllDepartment(CancellationToken token)
        {
            _logger.LogInformation("{controller}.{method}.{message}", nameof(DepartmentController), nameof(GetAllDepartment), "Request received to fetch all the departments");
            if(memoryCache.TryGetValue("cached",out IEnumerable<DepartmentDTO> listOfDepartment))
            {
                return new APIResponseWrapper<IEnumerable<DepartmentDTO>>()
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Department has been fetched successfully.",
                    Data = listOfDepartment
                };
            }
            await semaphoreSlim.WaitAsync(token);
            try
            {
                if(!memoryCache.TryGetValue("cached",out IEnumerable<DepartmentDTO> department))
                {
                    var memoryOption = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(1)).SetSlidingExpiration(TimeSpan.FromMinutes(10));
                    department = await departmentService.GetAllDepartmentAsync(token);
                    memoryCache.Set("cached", department, memoryOption);
                }

                return new APIResponseWrapper<IEnumerable<DepartmentDTO>>()
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Department has been fetched successfully.",
                    Data = department
                };
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }
        [HttpGet]
        [EnableRateLimiting("rateLimiter")]
        [Authorize(Roles = "Admin")]
        public async Task<APIResponseWrapper<DepartmentDTO>> GetADepartment([FromQuery] int deptId,CancellationToken token)
        {
            _logger.LogInformation("{controller}.{method}.{message}", nameof(DepartmentController), nameof(GetADepartment), "Request received to fetch a department");
            if (memoryCache.TryGetValue("cached", out DepartmentDTO department))
            {
                return new APIResponseWrapper<DepartmentDTO>()
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Department has been fetched successfully.",
                    Data = department
                };
            }
            await semaphoreSlim.WaitAsync(token);
            try
            {
                if (!memoryCache.TryGetValue("cached", out DepartmentDTO departmentFetched))
                {
                    var memoryOption = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(1)).SetSlidingExpiration(TimeSpan.FromMinutes(10));
                    departmentFetched = await departmentService.GetDepartmentAsync(deptId,token);
                    memoryCache.Set("cached", departmentFetched, memoryOption);
                }

                return new APIResponseWrapper<DepartmentDTO>()
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Department has been fetched successfully.",
                    Data = departmentFetched
                };
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }
        [HttpPost]
        [EnableRateLimiting("rateLimiter")]
        [Authorize(Roles = "Admin")]
        public async Task<APIResponseWrapper<DepartmentDTO>> AddNewDepartment([FromBody] DepartmentAddDTO departmentAdd,CancellationToken token)
        {
            _logger.LogInformation("{controller}.{method}.{message}", nameof(DepartmentController), nameof(AddNewDepartment), "Request received to add new departments");
            memoryCache.Remove("cached");
            var result = await departmentService.AddDepartment(departmentAdd, token);
            if (result == null)
            {
                _logger.LogError("{controller}.{method}.{message}", nameof(DepartmentController), nameof(AddNewDepartment), "Something went wrong while adding department");

                return new APIResponseWrapper<DepartmentDTO>()
                {
                    Data = null,
                    Message = "Something went wrong while adding department",
                    StatusCode = StatusCodes.Status400BadRequest,

                };
            }
            _logger.LogInformation("{controller}.{method}.{message}", nameof(DepartmentController), nameof(AddNewDepartment), "Department has been added successfully");

            return new APIResponseWrapper<DepartmentDTO>()
            {
                Data = result,
                Message = "Department has been added successfully",
                StatusCode = StatusCodes.Status200OK,

            };

        }
        [HttpPut]
        [EnableRateLimiting("rateLimiter")]
        [Authorize(Roles = "Admin")]
        public async Task<APIResponseWrapper<DepartmentDTO>> UpdateDepartment([FromBody] DepartmentDTO departmentUpdate,[FromQuery]int deptId, CancellationToken token)
        {
            _logger.LogInformation("{controller}.{method}.{message}", nameof(DepartmentController), nameof(UpdateDepartment), "Request received to update a department");
            memoryCache.Remove("cached");
            var result = await departmentService.UpdateDepartment(departmentUpdate, deptId, token);
            if (result == null)
            {
                _logger.LogError("{controller}.{method}.{message}", nameof(DepartmentController), nameof(UpdateDepartment), "Something went wrong while updating department");

                return new APIResponseWrapper<DepartmentDTO>()
                {
                    Data = null,
                    Message = "Something went wrong while updating department",
                    StatusCode = StatusCodes.Status400BadRequest,

                };
            }
            _logger.LogInformation("{controller}.{method}.{message}", nameof(DepartmentController), nameof(UpdateDepartment), "Department has been updated successfully");

            return new APIResponseWrapper<DepartmentDTO>()
            {
                Data = result,
                Message = "Department has been updated successfully",
                StatusCode = StatusCodes.Status200OK,

            };

        }
        [HttpDelete]
        [EnableRateLimiting("rateLimiter")]
        [Authorize(Roles = "Admin")]
        public async Task<APIResponseWrapper<string>> DeleteDepartment([FromQuery] int deptId,CancellationToken token)
        {
            _logger.LogInformation("{controller}.{method}.{message}", nameof(DepartmentController), nameof(DeleteDepartment), "Request received to remove a department");
            memoryCache.Remove("cached");
            var result = await departmentService.DeleteDepartmentAsync(deptId, token);
            if (!result)
            {
                _logger.LogError("{controller}.{method}.{message}", nameof(DepartmentController), nameof(DeleteDepartment), "Something went wrong while deleting department");

                return new APIResponseWrapper<string>()
                {
                    Data = null,
                    Message = "Something went wrong while deleting department",
                    StatusCode = StatusCodes.Status400BadRequest,

                };
            }
            _logger.LogInformation("{controller}.{method}.{message}", nameof(DepartmentController), nameof(DeleteDepartment), "Department has been removed successfully");

            return new APIResponseWrapper<string>()
            {
                Data = null,
                Message = "Department has been removed successfully",
                StatusCode = StatusCodes.Status200OK,

            };

        }
    }
}
