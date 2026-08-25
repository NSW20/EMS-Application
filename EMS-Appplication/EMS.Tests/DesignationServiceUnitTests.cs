using AutoMapper;
using EMS_Core.Domain.Entities;
using EMS_Core.Domain.RepositoryContract;
using EMS_Core.DTOs;
using EMS_Core.Helpers;
using EMS_Core.ServiceContracts;
using EMS_Core.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS.Tests
{
    public class DesignationServiceUnitTests
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<DesignationService>> _mockLogger;
        private readonly Mock<IDesignationRepository> _mockRepo;
        private readonly DesignationService _service;

        public DesignationServiceUnitTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<DesignationService>>();
            _mockRepo = new Mock<IDesignationRepository>();

            _service = new DesignationService(
                _mockMapper.Object,
                _mockLogger.Object,
                _mockRepo.Object
            );
        }

        [Fact]
        public async Task AddDesignation_Valid()
        {
            // Arrange
            var dto = new DesignationAddDTO { Title = "Manager", DepartmentId = 1 };
            var entity = new Designation { DesignationId = 1, Title = "Manager", DepartmentId = 1 };
            var resultDto = new DesignationDTO { DesignationId = 1, Title = "Manager", DepartmentId = 1 };

            _mockMapper.Setup(m => m.Map<Designation>(dto)).Returns(entity);
            _mockRepo.Setup(r => r.AddDesignationAsync(entity, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(entity);
            _mockMapper.Setup(m => m.Map<DesignationDTO>(entity)).Returns(resultDto);

            // Act
            var result = await _service.AddDesignation(dto, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Manager", result.Title);
            Assert.Equal(1, result.DesignationId);
        }
        [Fact]
        public async Task AddDesignation_InputInvalid()
        {
           await Assert.ThrowsAsync<ArgumentNullException>(() =>
                _service.AddDesignation(null, CancellationToken.None));
        }
        [Fact]
        public async Task AddDesignation_RepoReturnsNull()
        {
            var dto = new DesignationAddDTO { Title = "Manager", DepartmentId = 1 };
            var entity = new Designation { DesignationId = 1, Title = "Manager", DepartmentId = 1 };

            _mockMapper.Setup(x => x.Map<Designation>(dto)).Returns(entity);
             _mockRepo.Setup(x => x.AddDesignationAsync(entity,It.IsAny<CancellationToken>())).ReturnsAsync((Designation)null);
            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.AddDesignation(dto, CancellationToken.None));
        }
        [Fact]
        public async Task GetAllDesignation_valid()
        {
            var entity=new Designation { DesignationId = 1, Title = "Manager", DepartmentId = 1 };
            var dto = new DesignationDTO { Title = "Manager", DepartmentId = 1 };
            var entityList = new List<Designation>() { entity };
            var dtoList = new List<DesignationDTO>() { dto };

            _mockMapper.Setup(x => x.Map<IEnumerable<Designation>>(dtoList)).Returns(entityList);
            _mockRepo.Setup(x => x.GetDesignationAsync(It.IsAny<CancellationToken>())).ReturnsAsync(entityList);
            _mockMapper.Setup(x => x.Map<IEnumerable<DesignationDTO>>(entityList)).Returns(dtoList);
            var result = await _service.GetAllDesignationAsync(CancellationToken.None);
            result.Should().HaveCount(1);
            result.Should().NotBeNull();
            
        }
        [Fact]
        public async Task GetAllDesignation_Invalid()
        {
            _mockRepo.Setup(x => x.GetDesignationAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new List<Designation>());
            var result = await _service.GetAllDesignationAsync(CancellationToken.None);
            result.Should().NotBeNull();
            result.Should().HaveCount(0);

        }
        [Fact]
        public async Task UpdateDesignation_Valid()
        {
            int designationId = 1;
            var entity = new Designation { DesignationId = 1, Title = "Manager", DepartmentId = 1 };
            var dto = new DesignationDTO { Title = "Manager", DepartmentId = 1 };

            _mockMapper.Setup(x => x.Map<Designation>(dto)).Returns(entity);
            _mockRepo.Setup(x => x.UpdateDesignationAsync(entity, designationId, It.IsAny<CancellationToken>())).ReturnsAsync(entity);
            _mockMapper.Setup(x => x.Map<DesignationDTO>(entity)).Returns(dto);

            var result = await _service.UpdateDesignation(dto, designationId, CancellationToken.None);
            result.Should().NotBeNull();
            result.DepartmentId.Should().Be(1);
            result.Title.Should().Be("Manager");
        }
        [Fact]
        public async Task UpdateDesignation_Invalid()
        {
            int designationId = 1;
            var entity = new Designation { DesignationId = 1, Title = "Manager", DepartmentId = 1 };
            var dto = new DesignationDTO { Title = "Manager", DepartmentId = 1 };

            _mockMapper.Setup(x => x.Map<Designation>(dto)).Returns(entity);
            _mockRepo.Setup(x => x.UpdateDesignationAsync(entity, designationId, It.IsAny<CancellationToken>())).ReturnsAsync((Designation)null);

            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.UpdateDesignation(dto, designationId, CancellationToken.None));
        }
        [Fact]
        public async Task UpdateDesignation_InvalidDesignationId()
        {
            int designationId = 0;
            var entity = new Designation { DesignationId = 1, Title = "Manager", DepartmentId = 1 };
            var dto = new DesignationDTO { Title = "Manager", DepartmentId = 1 };

            await Assert.ThrowsAsync<ArgumentException>(() => _service.UpdateDesignation(dto, designationId, CancellationToken.None));
        }
        [Fact]
        public async Task UpdateDesignation_InvalidDesignationNull()
        {
            int designationId = 1;
            var entity = new Designation { DesignationId = 1, Title = "Manager", DepartmentId = 1 };
            var dto = new DesignationDTO { Title = "Manager", DepartmentId = 1 };

            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.UpdateDesignation(null, designationId, CancellationToken.None));
        }
        [Fact]
        public async Task DeleteDesignation_True()
        {
            int designationId = 1;
            _mockRepo.Setup(x => x.DeleteDesignationAsync(designationId, It.IsAny<CancellationToken>())).ReturnsAsync(true);
            var result = await _service.DeleteDesignationAsync(designationId, CancellationToken.None);
            result.Should().BeTrue();
        }
        [Fact]
        public async Task DeleteDesignation_False()
        {
            int designationId = 1;
            _mockRepo.Setup(x => x.DeleteDesignationAsync(designationId, It.IsAny<CancellationToken>())).ReturnsAsync(false);
            var result = await _service.DeleteDesignationAsync(designationId, CancellationToken.None);
            result.Should().BeFalse();
        }
    }
}
