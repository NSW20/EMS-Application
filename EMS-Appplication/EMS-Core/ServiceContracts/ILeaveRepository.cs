using EMS_Core.DTOs;

namespace EMS_Core.ServiceContracts
{
    public interface ILeaveService
    {
        Task<IEnumerable<LeaveDTO>> GetAllLeavesAsync(CancellationToken token);
        Task<LeaveDTO> GetLeaveAsync(int id, CancellationToken token);
        Task<LeaveDTO> AddLeave(LeaveAddDTO leaveAddDTO, CancellationToken token);
        Task<LeaveDTO> UpdateLeave(LeaveDTO leaveUpdateDTO, int leaveId, CancellationToken token);
        Task<bool> DeleteLeaveAsync(int id, CancellationToken token);
    }
}