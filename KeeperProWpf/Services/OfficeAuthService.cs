using KeeperProWpf.Data;
using KeeperProWpf.Models;
using Microsoft.EntityFrameworkCore;

namespace KeeperProWpf.Services
{
    public class OfficeAuthService
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