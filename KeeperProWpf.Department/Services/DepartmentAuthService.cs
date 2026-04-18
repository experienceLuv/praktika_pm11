using KeeperProWpf.Department.Data;
using KeeperProWpf.Department.Models;
using Microsoft.EntityFrameworkCore;

namespace KeeperProWpf.Department.Services
{
    public class DepartmentAuthService
    {
        public async Task<Employee?> LoginByCodeAsync(string employeeCode)
        {
            using var db = new AppDbContext();

            return await db.Employees
                .Include(x => x.Department)
                .FirstOrDefaultAsync(x =>
                    x.EmployeeCode == employeeCode &&
                    x.IsActive);
        }
    }
}