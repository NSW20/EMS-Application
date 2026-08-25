using System;
using System.Collections.Generic;
using System.Text;

namespace EMS_Core.Domain.Entities
{
    public class Designation
    {
        public int DesignationId { get; set; }
        public string? Title { get; set; }
        public int DepartmentId { get; set; }
        public Department? Department { get; set; }
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
