using KeeperProWpf.Security.Data;
using KeeperProWpf.Security.Models;
using Microsoft.EntityFrameworkCore;

namespace KeeperProWpf.Security.Services
{
    public class SecurityAuthService
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