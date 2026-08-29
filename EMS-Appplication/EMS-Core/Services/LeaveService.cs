using AutoMapper;
using EMS_Core.Domain.Entities;
using EMS_Core.Domain.RepositoryContract;
using EMS_Core.DTOs;
using EMS_Core.ServiceContracts;
using Microsoft.Extensions.Logging;

namespace EMS_Core.Services
{
    public class LeaveService : ILeaveService
    {
        private readonly IMapper _mapper;
        private readonly ILeaveRepository _leaveRepository;
        private readonly ILogger<LeaveService> _logger;

        public LeaveService(IMapper mapper, ILeaveRepository leaveRepository, ILogger<LeaveService> logger)
        {
            _mapper = mapper;
            _leaveRepository = leaveRepository;
            _logger = logger;
        }

        public async Task<LeaveDTO> AddLeave(LeaveAddDTO leaveAddDTO, CancellationToken token)
        {
            _logger.LogInformation("{method}.{class}.Request received to add leave", nameof(AddLeave), nameof(LeaveService));
            if (leaveAddDTO is null)
            {
                _logger.LogWarning("{method}.{class}.Invalid input", nameof(AddLeave), nameof(LeaveService));
                throw new ArgumentNullException(nameof(leaveAddDTO));
            }

            var entity = _mapper.Map<Leave>(leaveAddDTO);
            var result = await _leaveRepository.AddLeaveAsync(entity, token);
            if (result is null)
            {
                _logger.LogError("{method}.{class}.Failed to add leave", nameof(AddLeave), nameof(LeaveService));
                throw new InvalidOperationException("Something went wrong while adding leave");
            }
            return _mapper.Map<LeaveDTO>(result);
        }

        public async Task<bool> DeleteLeaveAsync(int id, CancellationToken token)
        {
            _logger.LogInformation("{method}.{class}.Request received to delete leave", nameof(DeleteLeaveAsync), nameof(LeaveService));
            if (id <= 0)
            {
                _logger.LogWarning("{method}.{class}.Invalid leave id", nameof(DeleteLeaveAsync), nameof(LeaveService));
                throw new ArgumentException("Invalid input", nameof(id));
            }
            return await _leaveRepository.DeleteLeaveAsync(id, token);
        }

        public async Task<IEnumerable<LeaveDTO>> GetAllLeavesAsync(CancellationToken token)
        {
            _logger.LogInformation("{method}.{class}.Request received to fetch all leaves", nameof(GetAllLeavesAsync), nameof(LeaveService));
            var result = await _leaveRepository.GetLeavesAsync(token);
            if (!result.Any()) return Enumerable.Empty<LeaveDTO>();
            return _mapper.Map<IEnumerable<LeaveDTO>>(result);
        }

        public async Task<LeaveDTO> GetLeaveAsync(int id, CancellationToken token)
        {
            _logger.LogInformation("{method}.{class}.Request received to fetch leave", nameof(GetLeaveAsync), nameof(LeaveService));
            if (id <= 0)
            {
                _logger.LogWarning("{method}.{class}.Invalid leave id", nameof(GetLeaveAsync), nameof(LeaveService));
                throw new ArgumentException("Invalid input", nameof(id));
            }

            var result = await _leaveRepository.GetLeaveByIdAsync(id, token);
            if (result is null)
            {
                _logger.LogError("{method}.{class}.Leave not found", nameof(GetLeaveAsync), nameof(LeaveService));
                throw new KeyNotFoundException($"No leave found with id {id}");
            }
            return _mapper.Map<LeaveDTO>(result);
        }

        public async Task<LeaveDTO> UpdateLeave(LeaveDTO leaveUpdateDTO, int leaveId, CancellationToken token)
        {
            _logger.LogInformation("{method}.{class}.Request received to update leave", nameof(UpdateLeave), nameof(LeaveService));
            if (leaveId <= 0)
            {
                _logger.LogWarning("{method}.{class}.Invalid leave id", nameof(UpdateLeave), nameof(LeaveService));
                throw new ArgumentException("Invalid input", nameof(leaveId));
            }
            if (leaveUpdateDTO is null)
            {
                _logger.LogWarning("{method}.{class}.Invalid input", nameof(UpdateLeave), nameof(LeaveService));
                throw new ArgumentNullException(nameof(leaveUpdateDTO));
            }

            var entity = _mapper.Map<Leave>(leaveUpdateDTO);
            var result = await _leaveRepository.UpdateLeaveAsync(entity, leaveId, token);
            if (result is null)
            {
                _logger.LogError("{method}.{class}.Failed to update leave", nameof(UpdateLeave), nameof(LeaveService));
                throw new InvalidOperationException("Something went wrong while updating leave");
            }
            return _mapper.Map<LeaveDTO>(result);
        }
    }
}