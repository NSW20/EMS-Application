using AutoMapper;
using Castle.Core.Logging;
using EMP_Infrastructure.Repositories;
using EMS_Core.Domain.Entities;
using EMS_Core.Domain.RepositoryContract;
using EMS_Core.DTOs;
using EMS_Core.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS.Tests
{
    public class DepartmentServiceTests
    {
        private DepartmentService departmentService;
        private readonly Mock<ILogger<DepartmentService>> _logger;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IDepartmentRepository> _departmentRepository;
        public DepartmentServiceTests()
        {
            _logger = new Mock<ILogger<DepartmentService>>();
            _mapper = new Mock<IMapper>();
            _departmentRepository = new Mock<IDepartmentRepository>();
            departmentService = new DepartmentService(_mapper.Object, _departmentRepository.Object, _logger.Object);
        }

        [Fact]
        public async Task AddDepartment_Valid()
        {
            var dto = new DepartmentAddDTO() { Description = "Human Resource", Name = "HR" };
            var entity = new Department() { DepartmentId = 1, Description = "Human Resource", Name = "HR" };
            var result = new DepartmentDTO() { DepartmentId = 1, Description = "Human Resource", Name = "HR" };

            _mapper.Setup(x => x.Map<Department>(dto)).Returns(entity);
            _departmentRepository.Setup(x => x.AddDepartmentAsync(entity, It.IsAny<CancellationToken>())).ReturnsAsync(entity);
            _mapper.Setup(x => x.Map<DepartmentDTO>(entity)).Returns(result);

            var resultReturn = await departmentService.AddDepartment(dto, CancellationToken.None);
            resultReturn.Should().NotBeNull();
            resultReturn.DepartmentId.Should().Be(1);
            resultReturn.Name.Should().Be("HR");

        }
        [Fact]
        public async Task AddDepartment_InValid()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => departmentService.AddDepartment(null,CancellationToken.None));

        }
        [Fact]
        public async Task AddDepartment_RepoInValid()
        {
            var dto = new DepartmentAddDTO() { Description = "Human Resource", Name = "HR" };
            var entity = new Department() { DepartmentId = 1, Description = "Human Resource", Name = "HR" };
            var result = new DepartmentDTO() { DepartmentId = 1, Description = "Human Resource", Name = "HR" };

            _mapper.Setup(x => x.Map<Department>(dto)).Returns(entity);
            _departmentRepository.Setup(x => x.AddDepartmentAsync(entity, It.IsAny<CancellationToken>())).ReturnsAsync((Department)null);
           await Assert.ThrowsAsync<InvalidOperationException>(() => departmentService.AddDepartment(dto, CancellationToken.None));

        }
        [Fact]
        public async Task GetAllDepartment_ShouldReturnListOfDepartments()
        {
            // Arrange
            var entity = new Department { DepartmentId = 1, Name = "HR", Description = "Human Resource" };
            var dto = new DepartmentDTO { DepartmentId = 1, Name = "HR", Description = "Human Resource" };
            var entityList = new List<Department> { entity };
            var dtoList = new List<DepartmentDTO> { dto };
            _departmentRepository.Setup(x => x.GetAllDepartmentsAsync(It.IsAny<CancellationToken>()))
                                 .ReturnsAsync( entityList );
            _mapper.Setup(x => x.Map<IEnumerable<DepartmentDTO>>(entityList)).Returns(dtoList);

            // Act
            var result = await departmentService.GetAllDepartmentAsync(CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.First().Name.Should().Be("HR");
        }
        [Fact]
        public async Task GetAllDepartment_HaveCountZero()
        {

             _departmentRepository.Setup(x => x.GetAllDepartmentsAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new List<Department>());
            var result = await departmentService.GetAllDepartmentAsync(CancellationToken.None);
            result.Should().NotBeNull();
            result.Should().HaveCount(0);

        }
        [Fact]
        public async Task UpdateDepartment_Valid()
        {
            var deptId = 1;
            var dto = new DepartmentDTO() { DepartmentId = 1, Name = "HR", Description = "Human Resource" };
            var entity=new Department() { DepartmentId = 1, Name = "HR", Description = "Human Resource" };

            _mapper.Setup(x => x.Map<Department>(dto)).Returns(entity);
            _departmentRepository.Setup(x => x.UpdateDepartmentAsync(entity, deptId, It.IsAny<CancellationToken>())).ReturnsAsync(entity);
            _mapper.Setup(x => x.Map<DepartmentDTO>(entity)).Returns(dto);

            var result = await departmentService.UpdateDepartment(dto, deptId, CancellationToken.None);
            result.Should().NotBeNull();
            result.DepartmentId.Should().Be(1);
            result.Name.Should().Be("HR");
        }
        [Fact]
        public async Task UpdateDepartment_InValid()
        {
            var deptId = 1;
            var dto = new DepartmentDTO() { DepartmentId = 1, Name = "HR", Description = "Human Resource" };
            var entity = new Department() { DepartmentId = 1, Name = "HR", Description = "Human Resource" };

            _mapper.Setup(x => x.Map<Department>(dto)).Returns(entity);
            _departmentRepository.Setup(x => x.UpdateDepartmentAsync(entity, deptId, It.IsAny<CancellationToken>())).ReturnsAsync((Department)null);
            await Assert.ThrowsAsync<InvalidOperationException>(() => departmentService.UpdateDepartment(dto, deptId, CancellationToken.None));

        }

        [Fact]
        public async Task DeleteDepartment_Valid()
        {
            int deptId = 1;
            _departmentRepository.Setup(x => x.DeleteDepartmentAsync(deptId, It.IsAny<CancellationToken>())).ReturnsAsync(true);
            var result = await departmentService.DeleteDepartmentAsync(deptId, CancellationToken.None);
            result.Should().BeTrue();
        }
        [Fact]
        public async Task DeleteDepartment_InValid()
        {
            int deptId = 1;
            _departmentRepository.Setup(x => x.DeleteDepartmentAsync(deptId, It.IsAny<CancellationToken>())).ReturnsAsync(false);
            var result = await departmentService.DeleteDepartmentAsync(deptId, CancellationToken.None);
            result.Should().BeFalse();
        }
    }
}
