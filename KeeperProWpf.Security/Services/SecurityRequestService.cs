using KeeperProWpf.Security.Data;
using KeeperProWpf.Security.Models;
using KeeperProWpf.Security.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Media;

namespace KeeperProWpf.Security.Services
{
    public class SecurityRequestService
    {
        public async Task<List<Department>> GetDepartmentsAsync()
        {
            using var db = new AppDbContext();
            return await db.Departments.OrderBy(x => x.DepartmentName).ToListAsync();
        }

        public async Task<List<SecurityRequestItemViewModel>> GetApprovedApplicationsAsync(
            DateTime? visitDate,
            string? typeName,
            int? departmentId,
            string? searchText)
        {
            using var db = new AppDbContext();

            int approvedStatusId = await db.ApplicationStatuses
                .Where(x => x.StatusName == "Одобрена")
                .Select(x => x.StatusId)
                .FirstAsync();

            var applications = await db.Applications
                .Include(a => a.ApplicationType)
                .Include(a => a.Department)
                .Include(a => a.Employee)
                .Include(a => a.Status)
                .Where(a => a.StatusId == approvedStatusId)
                .ToListAsync();

            if (visitDate.HasValue)
                applications = applications
                    .Where(a => a.DateStart.Date <= visitDate.Value.Date && a.DateEnd.Date >= visitDate.Value.Date)
                    .ToList();

            if (!string.IsNullOrWhiteSpace(typeName))
                applications = applications
                    .Where(a => a.ApplicationType != null && a.ApplicationType.TypeName == typeName)
                    .ToList();

            if (departmentId.HasValue)
                applications = applications
                    .Where(a => a.DepartmentId == departmentId.Value)
                    .ToList();

            var result = new List<SecurityRequestItemViewModel>();

            foreach (var app in applications)
            {
                var visitors = await db.ApplicationVisitors
                    .Where(x => x.ApplicationId == app.ApplicationId)
                    .Join(db.Visitors,
                        av => av.VisitorId,
                        v => v.VisitorId,
                        (av, v) => v)
                    .ToListAsync();

                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    string s = searchText.ToLower();

                    bool found = visitors.Any(v =>
                        ($"{v.LastName} {v.FirstName} {v.MiddleName}".ToLower().Contains(s)) ||
                        ($"{v.PassportSeries}{v.PassportNumber}".ToLower().Contains(s)));

                    if (!found)
                        continue;
                }

                result.Add(new SecurityRequestItemViewModel
                {
                    ApplicationId = app.ApplicationId,
                    TypeName = app.ApplicationType?.TypeName ?? "",
                    DepartmentName = app.Department?.DepartmentName ?? "",
                    EmployeeName = app.Employee?.FullName ?? "",
                    DateStart = app.DateStart.ToString("dd.MM.yyyy"),
                    DateEnd = app.DateEnd.ToString("dd.MM.yyyy"),
                    VisitorNames = string.Join(", ", visitors.Select(v => $"{v.LastName} {v.FirstName} {v.MiddleName}")),
                    PassportNumbers = string.Join(", ", visitors.Select(v => $"{v.PassportSeries} {v.PassportNumber}")),
                    StatusName = app.Status?.StatusName ?? ""
                });
            }

            return result;
        }

        public async Task<List<Visitor>> GetVisitorsByApplicationIdAsync(int applicationId)
        {
            using var db = new AppDbContext();

            return await db.ApplicationVisitors
                .Where(x => x.ApplicationId == applicationId)
                .Join(db.Visitors,
                    av => av.VisitorId,
                    v => v.VisitorId,
                    (av, v) => v)
                .ToListAsync();
        }

        public async Task<ApplicationEntity?> GetApplicationByIdAsync(int applicationId)
        {
            using var db = new AppDbContext();

            return await db.Applications
                .Include(x => x.ApplicationType)
                .Include(x => x.Department)
                .Include(x => x.Employee)
                .Include(x => x.Status)
                .FirstOrDefaultAsync(x => x.ApplicationId == applicationId);
        }

        public async Task AllowAccessAsync(int applicationId, int securityEmployeeId)
        {
            using var db = new AppDbContext();

            var visitors = await db.ApplicationVisitors
                .Where(x => x.ApplicationId == applicationId)
                .ToListAsync();

            foreach (var v in visitors)
            {
                var existing = await db.VisitLogs
                    .FirstOrDefaultAsync(x => x.ApplicationId == applicationId && x.VisitorId == v.VisitorId);

                if (existing == null)
                {
                    db.VisitLogs.Add(new VisitLog
                    {
                        ApplicationId = applicationId,
                        VisitorId = v.VisitorId,
                        SecurityEmployeeId = securityEmployeeId,
                        EntryTime = DateTime.UtcNow,
                        Comment = "Доступ разрешен"
                    });
                }
                else
                {
                    existing.SecurityEmployeeId = securityEmployeeId;
                    existing.EntryTime = DateTime.UtcNow;
                    existing.Comment = "Доступ разрешен";
                }
            }

            await db.SaveChangesAsync();
            SystemSounds.Beep.Play();
        }

        public async Task SetExitTimeAsync(int applicationId)
        {
            using var db = new AppDbContext();

            var logs = await db.VisitLogs
                .Where(x => x.ApplicationId == applicationId)
                .ToListAsync();

            foreach (var log in logs)
            {
                log.ExitTime = DateTime.UtcNow;
            }

            await db.SaveChangesAsync();
        }
    }
}