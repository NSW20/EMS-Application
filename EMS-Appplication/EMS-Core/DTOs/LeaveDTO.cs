using EMS_Core.Domain.Entities;
using EMS_Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS_Core.DTOs
{
    public class LeaveDTO
    {

        public int LeaveId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string? Reason { get; set; }
        public LeaveType LeaveType { get; set; }
        public LeaveStatus Status { get; set; }
        public string? ApprovedBy { get; set; }
    }
    public class LeaveAddDTO
    {
        public int EmployeeId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string? Reason { get; set; }
        public LeaveType LeaveType { get; set; }
        public LeaveStatus Status { get; set; }
        public string? ApprovedBy { get; set; }
    }
}
