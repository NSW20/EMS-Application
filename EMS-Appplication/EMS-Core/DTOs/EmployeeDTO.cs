using EMS_Core.Domain.Entities;
using EMS_Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS_Core.DTOs
{
    public class EmployeeDTO
    {
        public int EmployeeId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public int DepartmentId { get; set; }
        public int DesignationId { get; set; }
        public decimal? Salary { get; set; }
        public string? Status { get; set; }
        public string? UserId { get; set; }
    }
    public class EmployeeAddDTO
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public int DepartmentId { get; set; }
        public int DesignationId { get; set; }
        public decimal? Salary { get; set; }
        public string? Status { get; set; }
        public string? UserId { get; set; }
    }
}
