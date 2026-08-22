using EMS_Core.DTOs;
using EMS_Core.ReponseWrapper;
using EMS_Core.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Caching.Memory;
using Serilog.Core;

namespace EMS_API.Controllers
{
    [Route("api/V{version:apiVersion}/[controller]/[action]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class DesignationController : ControllerBase
    {
        private readonly IDesignationService _designationService;
        private readonly ILogger<DesignationController> _logger;
        private readonly IMemoryCache memoryCache;
        private static readonly SemaphoreSlim _cacheLock = new(1, 1);
        public DesignationController(IDesignationService designationService, ILogger<DesignationController> logger, IMemoryCache memoryCache)
        {
            _designationService = designationService;
            _logger = logger;
            this.memoryCache = memoryCache;
        }

        [HttpGet]
        [EnableRateLimiting("rateLimiter")]
        public async Task<APIResponseWrapper<IEnumerable<DesignationDTO>>> GetAllDesignations(CancellationToken token)
        {
            _logger.LogInformation("{Method}.{controller}.{message}",
               nameof(GetAllDesignations), nameof(DesignationController), "Request received for fetching all the designations");
            if (memoryCache.TryGetValue("cacheDesignations", out IEnumerable<DesignationDTO> cached))
            {
                return APIResponseWrapper<IEnumerable<DesignationDTO>>.Ok(200, cached, "Designations fetched successfully");
            }
            await _cacheLock.WaitAsync(token);
            try
            {
                if (!memoryCache.TryGetValue("cacheDesignations", out IEnumerable<DesignationDTO> listOfDesignations))
                {
                    var memoryOptions = new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromHours(1))
                        .SetSlidingExpiration(TimeSpan.FromMinutes(1));

                    listOfDesignations = await _designationService.GetAllDesignationAsync(token);
                    memoryCache.Set("cacheDesignations", listOfDesignations, memoryOptions);
                }
                return APIResponseWrapper<IEnumerable<DesignationDTO>>.Ok(200, listOfDesignations, "Designations fetched successfully");
            }
            finally
            {
                _cacheLock.Release();
            }
        }
        [HttpGet]
        [EnableRateLimiting("rateLimiter")]
        public async Task<APIResponseWrapper<DesignationDTO>> GetADesignation([FromQuery] int designationId, CancellationToken token)
        {
            _logger.LogInformation("{Method}.{controller}.{message}",
               nameof(GetADesignation), nameof(DesignationController), "Request received for fetching the designation");
            if (memoryCache.TryGetValue("cacheDesignations", out DesignationDTO cached))
            {
                return APIResponseWrapper<DesignationDTO>.Ok(200, cached, "Designations fetched successfully");
            }
                if (!memoryCache.TryGetValue("cacheDesignations", out DesignationDTO designation))
                {
                    var memoryOptions = new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromHours(1))
                        .SetSlidingExpiration(TimeSpan.FromMinutes(1));

                designation = await _designationService.GetDesignationAsync(designationId, token);
                    memoryCache.Set("cacheDesignations", designation, memoryOptions);
                }
                return APIResponseWrapper<DesignationDTO>.Ok(200, designation, "Designations fetched successfully");
        }

        [HttpPost]
        [EnableRateLimiting("rateLimiter")]
        public async Task<APIResponseWrapper<DesignationDTO>> AddDesignation([FromBody] DesignationAddDTO designationAddDTO, CancellationToken token)
        {
            memoryCache.Remove("cacheDesignations");
            _logger.LogInformation("{Method}.{controller}.{message}",
                         nameof(AddDesignation), nameof(DesignationController), "Request received for Add a designations");
            var result = await _designationService.AddDesignation(designationAddDTO, token);
            if (result == null)
            {
                _logger.LogInformation("{Method}.{controller}.{message}",
                       nameof(AddDesignation), nameof(DesignationController), "Something went wrong while adding designation");
                return new APIResponseWrapper<DesignationDTO>()
                {
                    Message = "Something went wrong while adding designation",
                    Data = result,
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }
            else
            {
                _logger.LogInformation("{Method}.{controller}.{message}",
                       nameof(AddDesignation), nameof(DesignationController), "Designation has been added successfully");
                return new APIResponseWrapper<DesignationDTO>()
                {
                    Message = "Designation has been added successfully",
                    Data = result,
                    StatusCode = StatusCodes.Status200OK
                };
            }
        }
        [HttpPut]
        [EnableRateLimiting("rateLimiter")]
        public async Task<APIResponseWrapper<DesignationDTO>> UpdateDesignation([FromBody] DesignationDTO designationUpdateDTO, [FromQuery] int designationId, CancellationToken token)
        {
            memoryCache.Remove("cacheDesignations");
            _logger.LogInformation("{Method}.{controller}.{message}",
                         nameof(UpdateDesignation), nameof(DesignationController), "Request received for update a designations");
            var result = await _designationService.UpdateDesignation(designationUpdateDTO, designationId, token);
            if (result == null)
            {
                _logger.LogInformation("{Method}.{controller}.{message}",
                       nameof(AddDesignation), nameof(DesignationController), "Something went wrong while updating designation");
                return new APIResponseWrapper<DesignationDTO>()
                {
                    Message = "Something went wrong while updating designation",
                    Data = result,
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }
            else
            {
                _logger.LogInformation("{Method}.{controller}.{message}",
                       nameof(AddDesignation), nameof(DesignationController), "Designation has been updated successfully");
                return new APIResponseWrapper<DesignationDTO>()
                {
                    Message = "Designation has been updated successfully",
                    Data = result,
                    StatusCode = StatusCodes.Status200OK
                };
            }
        }
        [HttpDelete]
        public async Task<APIResponseWrapper<string>> DeleteDesignation([FromQuery] int designationId, CancellationToken token)
        {
            memoryCache.Remove("cacheDesignations");
            _logger.LogInformation("{Method}.{controller}.{message}",
                         nameof(UpdateDesignation), nameof(DesignationController), "Request received for deleting a designations");
            var result = await _designationService.DeleteDesignationAsync(designationId, token);
            if (!result)
            {
                _logger.LogInformation("{Method}.{controller}.{message}",
                       nameof(AddDesignation), nameof(DesignationController), "Something went wrong while deleting designation");
                return new APIResponseWrapper<string>()
                {
                    Message = "Something went wrong while deleting designation",
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data=null
                };
            }
            else
            {
                _logger.LogInformation("{Method}.{controller}.{message}",
                       nameof(AddDesignation), nameof(DesignationController), "Designation has been deleted successfully");
                return new APIResponseWrapper<string>()
                {
                    Message = "Designation has been deleted successfully",
                    Data = null,
                    StatusCode = StatusCodes.Status200OK
                };
            }
        }

    }
}
