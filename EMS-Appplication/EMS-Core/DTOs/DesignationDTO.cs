using EMS_Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS_Core.DTOs
{
    public class DesignationDTO
    {
            public int DesignationId { get; set; }
            public string? Title { get; set; }
            public int DepartmentId { get; set; }

    }
    public class DesignationAddDTO
    {
        public string? Title { get; set; }
        public int DepartmentId { get; set; }

    }
}
