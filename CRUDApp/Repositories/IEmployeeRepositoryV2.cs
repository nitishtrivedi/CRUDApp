using CRUDApp.DTOs;
using CRUDApp.Models;

namespace CRUDApp.Repositories
{
    public interface IEmployeeRepositoryV2
    {
        //Introduced in API v2 - ALL BELOW
        Task<IEnumerable<Employee>> AddRangeAsync(IEnumerable<Employee> employees);

        //Added method for LINQ filtering
        //Task<IEnumerable<Employee>> GetByDepartmentASync(string department); //Commented as combined in one method
        Task<IEnumerable<DepartmentSummaryDto>> GetDepartmentSummaryAsync(int minCount = 3);
    }
}
