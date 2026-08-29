using EMS_Core.Domain.Entities;
using EMS_Core.DTOs;
using EMS_Core.ReponseWrapper;
using EMS_Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Caching.Memory;

namespace EMS_API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class LeaveController : ControllerBase
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<LeaveController> _logger;
        private readonly ILeaveService _leaveService;
        private static readonly SemaphoreSlim _cacheLock = new(1, 1);

        public LeaveController(IMemoryCache memoryCache, ILogger<LeaveController> logger, ILeaveService leaveService)
        {
            _memoryCache = memoryCache;
            _logger = logger;
            _leaveService = leaveService;
        }

        [HttpGet]
        [EnableRateLimiting("rateLimiter")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<APIResponseWrapper<IEnumerable<LeaveDTO>>>> GetAllLeaves(CancellationToken token)
        {
            _logger.LogInformation("{controller}.{method}.Request received to fetch all leaves", nameof(LeaveController), nameof(GetAllLeaves));
            if (_memoryCache.TryGetValue("cacheLeaves", out IEnumerable<LeaveDTO> cached))
            {
                _logger.LogInformation("{controller}.{Method}.{message}", nameof(LeaveController), nameof(GetAllLeaves), "Leaves fetched from memory cache");
                return StatusCode(StatusCodes.Status200OK, new APIResponseWrapper<IEnumerable<LeaveDTO>>
                {
                    Data = cached,
                    Message = "Leaves fetched successfully",
                    StatusCode = StatusCodes.Status200OK

                });     
            }

            await _cacheLock.WaitAsync(token);
            try
            {
                if (!_memoryCache.TryGetValue("cacheLeaves", out IEnumerable<LeaveDTO> leaves))
                {
                    var memoryOptions = new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromHours(1))
                        .SetSlidingExpiration(TimeSpan.FromMinutes(10));

                    leaves = await _leaveService.GetAllLeavesAsync(token);
                    _memoryCache.Set("cacheLeaves", leaves, memoryOptions);
                    _logger.LogInformation("{controller}.{Method}.{message}", nameof(LeaveController), nameof(GetAllLeaves), "Leaves cached in memory");

                }
                return StatusCode(StatusCodes.Status200OK, new APIResponseWrapper<IEnumerable<LeaveDTO>>
                {
                    Data = leaves,
                    Message = "Leaves fetched successfully",
                    StatusCode = StatusCodes.Status200OK

                });
            }
            finally
            {
                _cacheLock.Release();
            }
        }

        [HttpGet]
        [EnableRateLimiting("rateLimiter")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<ActionResult<APIResponseWrapper<LeaveDTO>>> GetALeave([FromQuery] int leaveId, CancellationToken token)
        {
            _logger.LogInformation("{controller}.{method}.Request received to fetch leave", nameof(LeaveController), nameof(GetALeave));
            if (_memoryCache.TryGetValue($"cacheLeave_{leaveId}", out LeaveDTO cached))
            {
                _logger.LogInformation("{controller}.{Method}.{message}", nameof(LeaveController), nameof(GetALeave), "Leave is fetched from memory.");
                return StatusCode(StatusCodes.Status200OK, new APIResponseWrapper<LeaveDTO>
                {
                    Data = cached,
                    Message = "Leave fetched successfully",
                    StatusCode = StatusCodes.Status200OK

                });
            }

            await _cacheLock.WaitAsync(token);
            try
            {
                if (!_memoryCache.TryGetValue($"cacheLeave_{leaveId}", out LeaveDTO leave))
                {
                    var memoryOptions = new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromHours(1))
                        .SetSlidingExpiration(TimeSpan.FromMinutes(10));

                    leave = await _leaveService.GetLeaveAsync(leaveId, token);
                    _memoryCache.Set($"cacheLeave_{leaveId}", leave, memoryOptions);
                    _logger.LogInformation("{controller}.{Method}.{message}", nameof(LeaveController), nameof(GetALeave), "Leave is stored in the server memory.");

                }
                return StatusCode(StatusCodes.Status200OK, new APIResponseWrapper<LeaveDTO>
                {
                    Data = leave,
                    Message = "Leave fetched successfully",
                    StatusCode = StatusCodes.Status200OK

                });
            }
            finally
            {
                _cacheLock.Release();
            }
        }

        [HttpPost]
        [EnableRateLimiting("rateLimiter")]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<APIResponseWrapper<LeaveDTO>>> AddNewLeave([FromBody] LeaveAddDTO leaveAdd, CancellationToken token)
        {
            _memoryCache.Remove("cacheLeaves");
            _logger.LogInformation("{controller}.{method}.Request received to add leave", nameof(LeaveController), nameof(AddNewLeave));
            var result = await _leaveService.AddLeave(leaveAdd, token);
            if (result == null)
            {
                _logger.LogError("{controller}.{Method}.{message}", nameof(LeaveController), nameof(AddNewLeave), "Some Error occured while adding leave");
                return StatusCode(StatusCodes.Status400BadRequest, new APIResponseWrapper<LeaveDTO>
                {
                    Data = null,
                    Message = "Something went wrong while adding leave",
                    StatusCode = StatusCodes.Status400BadRequest

                });

            }
            _logger.LogInformation("{controller}.{Method}.{message}", nameof(LeaveController), nameof(AddNewLeave), "Leave has been added successfully.");
            return StatusCode(StatusCodes.Status200OK, new APIResponseWrapper<LeaveDTO>
            {
                Data = result,
                Message = "Leave has been added successfully",
                StatusCode = StatusCodes.Status200OK

            });
          
        }

        [HttpPut]
        [EnableRateLimiting("rateLimiter")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<APIResponseWrapper<LeaveDTO>>> UpdateLeave([FromBody] LeaveDTO leaveUpdate, [FromQuery] int leaveId, CancellationToken token)
        {
            _memoryCache.Remove("cacheLeaves");
            _logger.LogInformation("{controller}.{method}.Request received to update leave", nameof(LeaveController), nameof(UpdateLeave));
            var result = await _leaveService.UpdateLeave(leaveUpdate, leaveId, token);
            if (result is null)
            {
                _logger.LogError("{controller}.{Method}.{message}", nameof(LeaveController), nameof(UpdateLeave), "Some Error occured while updating leave");
                return StatusCode(StatusCodes.Status400BadRequest, new APIResponseWrapper<LeaveDTO>
                {
                    Data = null,
                    Message = "Some Error occured while updating leave",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
            _logger.LogInformation("{controller}.{Method}.{message}", nameof(LeaveController), nameof(UpdateLeave), "Leave has been updated successfully.");
            return StatusCode(StatusCodes.Status200OK, new APIResponseWrapper<LeaveDTO>
            {
                Data = result,
                Message = "Leave has been updated successfully.",
                StatusCode = StatusCodes.Status200OK

            });
        }

        [HttpDelete]
        [EnableRateLimiting("rateLimiter")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<APIResponseWrapper<bool>>> DeleteLeave([FromQuery] int leaveId, CancellationToken token)
        {
            _memoryCache.Remove("cacheLeaves");
            _logger.LogInformation("{controller}.{method}.Request received to delete leave", nameof(LeaveController), nameof(DeleteLeave));
            var result = await _leaveService.DeleteLeaveAsync(leaveId, token);
            if (!result)
            {
                _logger.LogError("{controller}.{Method}.{message}", nameof(LeaveController), nameof(DeleteLeave), "Something went wrong while removing leave");

                return StatusCode(StatusCodes.Status400BadRequest, new APIResponseWrapper<bool>
                {
                    Data = false,
                    Message = "Something went wrong while removing leave",
                    StatusCode = StatusCodes.Status400BadRequest

                });
            }
            _logger.LogInformation("{controller}.{Method}.{message}", nameof(LeaveController), nameof(DeleteLeave), "Leave has been removed successfully");
            return StatusCode(StatusCodes.Status200OK, new APIResponseWrapper<bool>
            {
                Data = true,
                Message = "Leave has been removed successfully",
                StatusCode = StatusCodes.Status200OK

            });
        }
    }
}