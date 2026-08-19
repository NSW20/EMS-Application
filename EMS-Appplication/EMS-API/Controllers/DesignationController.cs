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
    }
}
