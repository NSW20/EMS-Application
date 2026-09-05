using AutoMapper;
using EMS_Core.Domain.Entities;
using EMS_Core.Domain.RepositoryContract;
using EMS_Core.DTOs;
using EMS_Core.Enums;
using EMS_Core.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS.Tests
{
    public class LeaveServiceTests
    {
        private LeaveService _leaveService;
        private Mock<ILogger<LeaveService>> _logger;
        private Mock<IMapper> _mapper;
        private Mock<ILeaveRepository> _leaveRepo;
        public LeaveServiceTests()
        {
            _logger = new Mock<ILogger<LeaveService>>();
            _mapper = new Mock<IMapper>();
            _leaveRepo = new Mock<ILeaveRepository>();
            _leaveService = new LeaveService(_mapper.Object, _leaveRepo.Object, _logger.Object);
        }

        [Fact]
        public async Task GetAllLeaves_Valid()
        {
            var dto= new LeaveDTO()
            {
                LeaveId=1,
                EmployeeId = 1,
                ApprovedBy = "Niranjan",
                FromDate = DateTime.UtcNow,
                ToDate = DateTime.UtcNow.AddDays(1),
                Reason = "Medical Emergency",
                LeaveType = LeaveType.Earned,
                Status = LeaveStatus.Pending
            };
            var entity = new Leave()
            {
                LeaveId = 1,
                EmployeeId = 1,
                ApprovedBy = "Niranjan",
                FromDate = DateTime.UtcNow,
                ToDate = DateTime.UtcNow.AddDays(1),
                Reason = "Medical Emergency",
                LeaveType = LeaveType.Earned,
                Status = LeaveStatus.Pending
            };
            var dtoList = new List<LeaveDTO> { dto };
            var entityList = new List<Leave> { entity };

            _mapper.Setup(x => x.Map<IEnumerable<Leave>>(dtoList)).Returns(entityList);
             _leaveRepo.Setup(x => x.GetLeavesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(entityList);
            _mapper.Setup(x => x.Map<IEnumerable<LeaveDTO>>(entityList)).Returns(dtoList);
            var result = await _leaveService.GetAllLeavesAsync(CancellationToken.None);
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
        }

        [Fact]
        public async Task GetAllLeaves_ZeroLeaves()
        {
            var dto = new LeaveDTO()
            {
                LeaveId = 1,
                EmployeeId = 1,
                ApprovedBy = "Niranjan",
                FromDate = DateTime.UtcNow,
                ToDate = DateTime.UtcNow.AddDays(1),
                Reason = "Medical Emergency",
                LeaveType = LeaveType.Earned,
                Status = LeaveStatus.Pending
            };
            var entity = new Leave()
            {
                LeaveId = 1,
                EmployeeId = 1,
                ApprovedBy = "Niranjan",
                FromDate = DateTime.UtcNow,
                ToDate = DateTime.UtcNow.AddDays(1),
                Reason = "Medical Emergency",
                LeaveType = LeaveType.Earned,
                Status = LeaveStatus.Pending
            };
            var dtoList = new List<LeaveDTO> { dto };
            var entityList = new List<Leave> { entity };

            _mapper.Setup(x => x.Map<IEnumerable<Leave>>(dtoList)).Returns(entityList);
            _leaveRepo.Setup(x => x.GetLeavesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(Enumerable.Empty<Leave>());
            _mapper.Setup(x => x.Map<IEnumerable<LeaveDTO>>(entityList)).Returns(dtoList);
            var result = await _leaveService.GetAllLeavesAsync(CancellationToken.None);
            result.Should().HaveCount(0);
        }
    }
}
