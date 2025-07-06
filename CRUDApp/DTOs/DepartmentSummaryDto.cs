using CRUDApp.Models;

namespace CRUDApp.DTOs
{
    public class DepartmentSummaryDto
    {
        public string Department { get; set; } = default!;
        public int Count { get; set; }
        public List<Employee> Employees { get; set; } = [];
    }
}
