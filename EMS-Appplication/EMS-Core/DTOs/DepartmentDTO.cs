using EMS_Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS_Core.DTOs
{
    public class DepartmentDTO
    {
        public int DepartmentId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
    public class DepartmentAddDTO
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
