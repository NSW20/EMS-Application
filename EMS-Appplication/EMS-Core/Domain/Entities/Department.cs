using System;
using System.Collections.Generic;
using System.Text;

namespace EMS_Core.Domain.Entities
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public ICollection<Designation> Designations { get; set; } = new List<Designation>();
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
