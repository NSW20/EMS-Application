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
    public class AttendaceController : ControllerBase
    {
        private readonly IAttendanceService attendaceService;
        private readonly ILogger<AttendaceController> _logger;
        private readonly IMemoryCache memoryCache;
        private readonly SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);
        public AttendaceController(IAttendanceService attendaceService, ILogger<AttendaceController> _logger, IMemoryCache memoryCache)
        {
            this.attendaceService = attendaceService;
            this._logger = _logger;
            this.memoryCache = memoryCache;
        }

        [HttpGet]
        [Authorize]
        [EnableRateLimiting("rateLimiter")]
        public async Task<ActionResult<APIResponseWrapper<IEnumerable<AttendanceDTO>>>> GetAllAttendace(CancellationToken token)
        {
            _logger.LogInformation("{controller}.{Method}.{message}", nameof(AttendaceController), nameof(GetAllAttendace), "Request received to fetch all the attendance");
            if (memoryCache.TryGetValue("cached", out IEnumerable<AttendanceDTO> attendace))
            {
                _logger.LogInformation("{controller}.{Method}.{message}", nameof(AttendaceController), nameof(GetAllAttendace), "Attendance fetched from memory cache");
                return StatusCode(StatusCodes.Status200OK, new APIResponseWrapper<IEnumerable<AttendanceDTO>>
                {
                    Data = attendace,
                    Message = "All the attendance have been fetched successfully.",
                    StatusCode = StatusCodes.Status200OK

                });
            }
            await semaphoreSlim.WaitAsync(token);
            try
            {
                var result = await attendaceService.GetAllAttendance(token);
                var memoryEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(1)).SetSlidingExpiration(TimeSpan.FromMinutes(10));
                memoryCache.Set("cached", result, memoryEntryOptions);
                _logger.LogInformation("{controller}.{Method}.{message}", nameof(AttendaceController), nameof(GetAllAttendace), "Attendance cached in memory");
                return StatusCode(StatusCodes.Status200OK, new APIResponseWrapper<IEnumerable<AttendanceDTO>>
                {
                    Data = result,
                    Message = "All the attendance have been fetched successfully.",
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
        public async Task<ActionResult<APIResponseWrapper<AttendanceDTO>>> FindAttendaceById([FromQuery] int attendaceId, CancellationToken token)
        {
            _logger.LogInformation("{controller}.{Method}.{message}", nameof(AttendaceController), nameof(FindAttendaceById), "Request received to fetch attendance by Id");
            var result = await attendaceService.GetAttendanceByIdAsync(attendaceId, token);
            if (result is null)
            {
                _logger.LogInformation("{controller}.{Method}.{message}", nameof(AttendaceController), nameof(FindAttendaceById), "Attendace details does not exists.Please check the attendace Id");
                return StatusCode(StatusCodes.Status200OK, new APIResponseWrapper<AttendanceDTO>
                {
                    Data = null,
                    Message = "Attendance details does not exists.Please check the attendance Id",
                    StatusCode = StatusCodes.Status200OK

                });
            }
            _logger.LogInformation("{controller}.{Method}.{message}", nameof(AttendaceController), nameof(FindAttendaceById), "Attendace details has been fetched successfully.");
            return StatusCode(StatusCodes.Status200OK, new APIResponseWrapper<AttendanceDTO>
            {
                Data = result,
                Message = "Attendace details has been fetched successfully.",
                StatusCode = StatusCodes.Status200OK

            });
        }
        [HttpPost]
        [Authorize(Roles = "Admin,Employee")]
        [EnableRateLimiting("rateLimiter")]
        public async Task<ActionResult<APIResponseWrapper<AttendanceDTO>>> AddAttendace([FromBody] AttendanceAddDTO attendaceAddDTO, CancellationToken token)
        {
            _logger.LogInformation("{controller}.{Method}.{message}", nameof(AttendaceController), nameof(AddAttendace), "Request received to apply attendace");
            var result = await attendaceService.ApplyAttendanceAsync(attendaceAddDTO, token);
            if (result is null)
            {
                _logger.LogError("{controller}.{Method}.{message}", nameof(AttendaceController), nameof(AddAttendace), "Some Error occured while applying attendace");
                return StatusCode(StatusCodes.Status400BadRequest, new APIResponseWrapper<AttendanceDTO>
                {
                    Data = null,
                    Message = "Some Error occured while applying attendace",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
            memoryCache.Remove("cached");
            _logger.LogInformation("{controller}.{Method}.{message}", nameof(AttendaceController), nameof(AddAttendace), "Attendace has been applied successfully.");
            return StatusCode(StatusCodes.Status200OK, new APIResponseWrapper<AttendanceDTO>
            {
                Data = result,
                Message = "Attendace has been applied successfully.",
                StatusCode = StatusCodes.Status200OK

            });
        }
        [HttpPut]
        [Authorize(Roles = "Admin,Employee")]
        [EnableRateLimiting("rateLimiter")]
        public async Task<ActionResult<APIResponseWrapper<AttendanceDTO>>> UpdateAttendace([FromBody] AttendanceDTO attendaceDTO,[FromQuery] int attendaceId, CancellationToken token)
        {
            _logger.LogInformation("{controller}.{Method}.{message}", nameof(AttendaceController), nameof(UpdateAttendace), "Request received to update a attendace");
            var result = await attendaceService.UpdateAttendanceAsync( attendaceDTO, attendaceId,token);
            if (result is null)
            {
                _logger.LogError("{controller}.{Method}.{message}", nameof(AttendaceController), nameof(UpdateAttendace), "Some Error occured while updating attendace");
                return StatusCode(StatusCodes.Status400BadRequest, new APIResponseWrapper<AttendanceDTO>
                {
                    Data = null,
                    Message = "Some Error occured while updating attendace",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
            memoryCache.Remove("cached");
            _logger.LogInformation("{controller}.{Method}.{message}", nameof(AttendaceController), nameof(UpdateAttendace), "Attendace has been updated successfully.");
            return StatusCode(StatusCodes.Status200OK, new APIResponseWrapper<AttendanceDTO>
            {
                Data = result,
                Message = "Attendace has been updated successfully.",
                StatusCode = StatusCodes.Status200OK

            });
        }
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [EnableRateLimiting("rateLimiter")]
        public async Task<ActionResult<APIResponseWrapper<bool>>> RemoveAttendace([FromQuery] int attendaceId, CancellationToken token)
        {
            _logger.LogInformation("{controller}.{Method}.{message}", nameof(AttendaceController), nameof(RemoveAttendace), "Request received to remove a attendace");
            var result = await attendaceService.DeleteAttendanceAsync(attendaceId, token);
            if (!result)
            {
                _logger.LogError("{controller}.{Method}.{message}", nameof(AttendaceController), nameof(RemoveAttendace), "Some Error occured while removing attendace");
                return StatusCode(StatusCodes.Status400BadRequest, new APIResponseWrapper<bool>
                {
                    Data = false,
                    Message = "Some Error occured while removing attendace",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
            memoryCache.Remove("cached");
            _logger.LogInformation("{controller}.{Method}.{message}", nameof(AttendaceController), nameof(RemoveAttendace), "Attendace has been removed successfully.");
            return StatusCode(StatusCodes.Status200OK, new APIResponseWrapper<bool>
            {
                Data = true,
                Message = "Attendace has been removed successfully.",
                StatusCode = StatusCodes.Status200OK

            });
        }
    }
}

