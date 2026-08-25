using EMS_Core.Domain.Entities;
using EMS_Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS_Core.DTOs
{
    public class AttendanceDTO
    {
        public int AttendanceId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan? CheckIn { get; set; }
        public TimeSpan? CheckOut { get; set; }
        public string? Status { get; set; }
    }
    public class AttendanceAddDTO
    {
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan? CheckIn { get; set; }
        public TimeSpan? CheckOut { get; set; }
        public string? Status { get; set; }
    }
}
