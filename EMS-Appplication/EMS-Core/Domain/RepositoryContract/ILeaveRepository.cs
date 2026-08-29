using EMS_Core.Domain.Entities;

namespace EMS_Core.Domain.RepositoryContract
{
    public interface ILeaveRepository
    {
        Task<IEnumerable<Leave>> GetLeavesAsync(CancellationToken token);
        Task<Leave> GetLeaveByIdAsync(int id, CancellationToken token);
        Task<Leave> AddLeaveAsync(Leave leave, CancellationToken token);
        Task<Leave> UpdateLeaveAsync(Leave leave, int leaveId, CancellationToken token);
        Task<bool> DeleteLeaveAsync(int id, CancellationToken token);
    }
}