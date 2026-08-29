using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using EMS_Core.Domain.Entities;
using EMS_Core.Domain.RepositoryContract;
using EMS_Core.DTOs;
using EMS_Core.ServiceContracts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS_Core.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IMapper _mapper;
        private readonly IAttendanceRepository _attendaceRepository;
        private readonly ILogger<AttendanceService> _logger;
        public AttendanceService(IMapper _mapper, IAttendanceRepository _attendaceRepository, ILogger<AttendanceService> _logger)
        {
            this._attendaceRepository = _attendaceRepository;
            this._mapper = _mapper;
            this._logger = _logger;
        }
        public async Task<AttendanceDTO> ApplyAttendanceAsync(AttendanceAddDTO attendance, CancellationToken token)
        {
            _logger.LogInformation("{method}.{class}.{message}", nameof(ApplyAttendanceAsync), nameof(AttendanceService), "Request Recieved to apply attendace");
            if(attendance is null)
            {
                _logger.LogError("{method}.{class}.{message}", nameof(ApplyAttendanceAsync), nameof(AttendanceService), "Please provide valid input");
                throw new ArgumentException("Please provide valid input", nameof(attendance));
            }
            var entity = _mapper.Map<Attendance>(attendance);
            var result = await _attendaceRepository.ApplyAttendanceAsync(entity, token);
            if(result is null)
            {
                _logger.LogError("{method}.{class}.{message}", nameof(ApplyAttendanceAsync), nameof(AttendanceService), "Something went wrong while applying attendace");
                throw new InvalidOperationException("Something went wrong while applying attendace");
            }
            _logger.LogInformation("{method}.{class}.{message}", nameof(ApplyAttendanceAsync), nameof(AttendanceService), "Attendace has been applied successfully");
            return _mapper.Map<AttendanceDTO>(result);
        }

        public async Task<bool> DeleteAttendanceAsync(int id, CancellationToken token)
        {
            _logger.LogInformation("{method}.{class}.{message}", nameof(DeleteAttendanceAsync), nameof(AttendanceService), "Request Recieved to remove attendace");
            if (id <= 0)
            {
                _logger.LogError("{method}.{class}.{message}", nameof(DeleteAttendanceAsync), nameof(AttendanceService), "Please enter valid attendace id");
                throw new ArgumentNullException(nameof(id), "Please enter valid attendace id");
            }
            var result = await _attendaceRepository.DeleteAttendanceAsync(id, token);
            if(result)
            {
                _logger.LogInformation("{method}.{class}.{message}", nameof(DeleteAttendanceAsync), nameof(AttendanceService), "Attendace has been removed succussfully");
                return true;
            }
            _logger.LogError("{method}.{class}.{message}", nameof(DeleteAttendanceAsync), nameof(AttendanceService), "Unable to remove attendace. Please try again later.");
            return false;

        }

        public async Task<IEnumerable<AttendanceDTO>> GetAllAttendance(CancellationToken token)
        {
            _logger.LogInformation("{method}.{class}.{message}", nameof(GetAllAttendance), nameof(AttendanceService), "Request Recieved to fetch all the attendace");
            var result = await _attendaceRepository.GetAllAttendance(token);
            if (!result.Any())
            {
                _logger.LogInformation("{method}.{class}.{message}", nameof(GetAllAttendance), nameof(AttendanceService), "No records found.");
                return Enumerable.Empty<AttendanceDTO>();
            }
            _logger.LogInformation("{method}.{class}.{message}", nameof(GetAllAttendance), nameof(AttendanceService), "Attendace have been fetched successfully.");
            return _mapper.Map<IEnumerable<AttendanceDTO>>(result);

        }

        public async Task<AttendanceDTO> GetAttendanceByIdAsync(int id, CancellationToken token)
        {
            _logger.LogInformation("{method}.{class}.{message}", nameof(GetAttendanceByIdAsync), nameof(AttendanceService), "Request Recieved to fetch a attendace");
            if (id <= 0)
            {
                _logger.LogError("{method}.{class}.{message}", nameof(GetAttendanceByIdAsync), nameof(AttendanceService), "Please enter valid attendace id");
                throw new ArgumentNullException(nameof(id), "Please enter valid attendace id");
            }
            var result = await _attendaceRepository.GetAttendanceByIdAsync(id, token);
            if(result is null)
            {
                _logger.LogError("{method}.{class}.{message}", nameof(GetAttendanceByIdAsync), nameof(AttendanceService), "Attendace does not exists.");
                return null;
            }
            _logger.LogInformation("{method}.{class}.{message}", nameof(GetAttendanceByIdAsync), nameof(AttendanceService), "Attendace record has been fetched successfully");
            return _mapper.Map<AttendanceDTO>(result);
        }

        public async Task<AttendanceDTO> UpdateAttendanceAsync(AttendanceDTO attendance, int id, CancellationToken token)
        {

            _logger.LogInformation("{method}.{class}.{message}", nameof(UpdateAttendanceAsync), nameof(AttendanceService), "Request Recieved to update attendace");
            if (attendance is null)
            {
                _logger.LogError("{method}.{class}.{message}", nameof(UpdateAttendanceAsync), nameof(AttendanceService), "Please provide valid input");
                throw new ArgumentException("Please provide valid input", nameof(attendance));
            }
            if (id <= 0)
            {
                _logger.LogError("{method}.{class}.{message}", nameof(UpdateAttendanceAsync), nameof(AttendanceService), "Please enter valid attendace id");
                throw new ArgumentNullException(nameof(id), "Please enter valid attendace id");
            }
            var entity = _mapper.Map<Attendance>(attendance);
            var result = await _attendaceRepository.UpdateAttendanceAsync(entity, id, token);
            if(result is null)
            {
                _logger.LogError("{method}.{class}.{message}", nameof(UpdateAttendanceAsync), nameof(AttendanceService), "Unable to update attendance. Please try again later.");
                throw new InvalidOperationException("Unable to update attendance. Please try again later.");
            }
            _logger.LogInformation("{method}.{class}.{message}", nameof(UpdateAttendanceAsync), nameof(AttendanceService), "Attendace has been updated successfully.");
            return _mapper.Map<AttendanceDTO>(result);
        }
    }
}
