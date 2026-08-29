using EMS_Core.Domain.Entities;
using EMS_Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS_Core.ServiceContracts
{
    public interface IAttendanceService
    {
        Task<IEnumerable<AttendanceDTO>> GetAllAttendance(CancellationToken token);
        Task<AttendanceDTO> GetAttendanceByIdAsync(int id, CancellationToken token);
        Task<AttendanceDTO> ApplyAttendanceAsync(AttendanceAddDTO attendace, CancellationToken token);
        Task<AttendanceDTO> UpdateAttendanceAsync(AttendanceDTO attendace, int attendaceId, CancellationToken token);
        Task<bool> DeleteAttendanceAsync(int id, CancellationToken token);
    }
}
