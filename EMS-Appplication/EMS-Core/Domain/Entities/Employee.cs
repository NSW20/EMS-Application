using EMS_Core.Enums;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace EMS_Core.Domain.Entities
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public int DepartmentId { get; set; }
        public Department? Department { get; set; }
        public int DesignationId { get; set; }
        public Designation? Designation { get; set; }
        public decimal? Salary { get; set; }
        public EmployeeStatus? Status { get; set; }
        public string? UserId { get; set; }
        public AppUser? User { get; set; }
        public ICollection<Attendance>? Attendances { get; set; } = new List<Attendance>();
        public ICollection<Leave>? Leaves { get; set; } = new List<Leave>();
    }
}
