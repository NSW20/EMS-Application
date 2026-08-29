using EMS_Core.Domain.Entities;
using EMS_Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS_Core.Domain.RepositoryContract
{
    public interface IAttendanceRepository
    {
        Task<IEnumerable<Attendance>> GetAllAttendance(CancellationToken token);
        Task<Attendance> GetAttendanceByIdAsync(int id, CancellationToken token);
        Task<Attendance> ApplyAttendanceAsync(Attendance attendace, CancellationToken token);
        Task<Attendance> UpdateAttendanceAsync(Attendance attendace, int attendaceId, CancellationToken token);
        Task<bool> DeleteAttendanceAsync(int id, CancellationToken token);
    }
}
