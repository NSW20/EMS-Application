using EMP_Infrastructure.SqlOperation;
using EMS_Core.Domain.Entities;
using EMS_Core.Domain.RepositoryContract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EMP_Infrastructure.Repositories
{
    public class LeaveRepository : ILeaveRepository
    {
        private readonly EMSDbContext _context;
        private readonly ILogger<LeaveRepository> _logger;

        public LeaveRepository(EMSDbContext context, ILogger<LeaveRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Leave> AddLeaveAsync(Leave leave, CancellationToken token)
        {
            _logger.LogInformation("{method}.{class}.Request received to add leave", nameof(AddLeaveAsync), nameof(LeaveRepository));
            await _context.Leaves.AddAsync(leave, token);
            await _context.SaveChangesAsync(token);
            return leave;
        }

        public async Task<bool> DeleteLeaveAsync(int id, CancellationToken token)
        {
            _logger.LogInformation("{method}.{class}.Request received to delete leave", nameof(DeleteLeaveAsync), nameof(LeaveRepository));
            var entity = await _context.Leaves.FirstOrDefaultAsync(x => x.LeaveId == id, token);
            if (entity != null)
            {
                _context.Leaves.Remove(entity);
                await _context.SaveChangesAsync(token);
            }
            return true;
        }

        public async Task<Leave> GetLeaveByIdAsync(int id, CancellationToken token)
        {
            _logger.LogInformation("{method}.{class}.Request received to fetch leave by id", nameof(GetLeaveByIdAsync), nameof(LeaveRepository));
            return await _context.Leaves.FirstOrDefaultAsync(x => x.LeaveId == id, token);
        }

        public async Task<IEnumerable<Leave>> GetLeavesAsync(CancellationToken token)
        {
            _logger.LogInformation("{method}.{class}.Request received to fetch all leaves", nameof(GetLeavesAsync), nameof(LeaveRepository));
            return await _context.Leaves.ToListAsync(token);
        }

        public async Task<Leave> UpdateLeaveAsync(Leave leave, int leaveId, CancellationToken token)
        {
            _logger.LogInformation("{method}.{class}.Request received to update leave", nameof(UpdateLeaveAsync), nameof(LeaveRepository));
            var existing = await _context.Leaves.FirstOrDefaultAsync(x => x.LeaveId == leaveId, token);
            if (existing == null) return null!;
            existing.FromDate = leave.FromDate;
            existing.ToDate = leave.ToDate;
            existing.Reason = leave.Reason;
            existing.LeaveType = leave.LeaveType;
            existing.Status = leave.Status;
            existing.ApprovedBy = leave.ApprovedBy;
            _context.Leaves.Update(existing);
            await _context.SaveChangesAsync(token);
            return existing;
        }
    }
}