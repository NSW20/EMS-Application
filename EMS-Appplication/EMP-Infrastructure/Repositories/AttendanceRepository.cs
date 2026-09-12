using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Office2010.Excel;
using EMP_Infrastructure.SqlOperation;
using EMS_Core.Domain.Entities;
using EMS_Core.Domain.RepositoryContract;
using EMS_Core.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMP_Infrastructure.Repositories
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly EMSDbContext _context;
        private readonly ILogger<AttendanceRepository> _logger;
        public AttendanceRepository(EMSDbContext _context, ILogger<AttendanceRepository> _logger)
        {
            this._context = _context;
            this._logger = _logger;
        }

        public async Task<Attendance> ApplyAttendanceAsync(Attendance attendance, CancellationToken token)
        {
            _logger.LogInformation("{method}.{class}.{message}", nameof(ApplyAttendanceAsync), nameof(AttendanceRepository), "Request Recieved to apply attendace");
            await _context.Attendances.AddAsync(attendance);
            await _context.SaveChangesAsync(token);
            return attendance;
        }

        public async Task<bool> DeleteAttendanceAsync(int id, CancellationToken token)
        {
            _logger.LogInformation("{method}.{class}.{message}", nameof(DeleteAttendanceAsync), nameof(AttendanceRepository), "Request Recieved to remove attendace");
            var attendanceToBeRemove = await _context.Attendances.FirstOrDefaultAsync(x => x.AttendanceId == id);
            if(attendanceToBeRemove is null)
            {
                return false;
            }
             _context.Attendances.Remove(attendanceToBeRemove);
            await _context.SaveChangesAsync(token);
            return true;
        }

        public async Task<IEnumerable<Attendance>> GetAllAttendance(CancellationToken token)
        {
            _logger.LogInformation("{method}.{class}.{message}", nameof(GetAllAttendance), nameof(AttendanceRepository), "Request Recieved to fetch all attendace records");
            var result= await _context.Attendances.ToListAsync(token);
            return result;
        }

        public async Task<Attendance> GetAttendanceByIdAsync(int id, CancellationToken token)
        {
            _logger.LogInformation("{method}.{class}.{message}", nameof(GetAttendanceByIdAsync), nameof(AttendanceRepository), "Request Recieved to fetch a attendace record");
            var attendance = await _context.Attendances.FirstOrDefaultAsync(x => x.AttendanceId == id,token);
            if (attendance is null)
            {
                return null;
            }
            return attendance;
        }

        public async Task<Attendance> UpdateAttendanceAsync(Attendance attendance, int attendaceId, CancellationToken token)
        {
            _logger.LogInformation("{method}.{class}.{message}", nameof(UpdateAttendanceAsync), nameof(AttendanceRepository), "Request Recieved to update a attendace record");
            var attendanceToBeUpdated = await _context.Attendances.FirstOrDefaultAsync(x => x.AttendanceId == attendaceId);
            if (attendanceToBeUpdated is null)
            {
                return null;
            }
            attendanceToBeUpdated.CheckOut = attendance.CheckOut;
            attendanceToBeUpdated.Status = attendance.Status;
            _context.Attendances.Update(attendanceToBeUpdated);
            await _context.SaveChangesAsync(token);
            return attendanceToBeUpdated;
        }
    }
}
