using KeeperProWpf.Department.Data;
using KeeperProWpf.Department.Models;
using KeeperProWpf.Department.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace KeeperProWpf.Department.Services
{
    public class DepartmentVisitService
    {
        public async Task<List<DepartmentRequestItemViewModel>> GetApprovedDepartmentApplicationsAsync(
            int departmentId,
            DateTime? dateFrom,
            DateTime? dateTo)
        {
            using var db = new AppDbContext();

            int approvedStatusId = await db.ApplicationStatuses
                .Where(x => x.StatusName == "Одобрена")
                .Select(x => x.StatusId)
                .FirstAsync();

            var query = db.Applications
                .Include(a => a.ApplicationType)
                .Include(a => a.Department)
                .Include(a => a.Employee)
                .Include(a => a.Status)
                .Where(a => a.DepartmentId == departmentId && a.StatusId == approvedStatusId);

            if (dateFrom.HasValue)
                query = query.Where(a => a.DateStart.Date >= dateFrom.Value.Date);

            if (dateTo.HasValue)
                query = query.Where(a => a.DateEnd.Date <= dateTo.Value.Date);

            var applications = await query
                .OrderByDescending(a => a.ApplicationId)
                .ToListAsync();

            var result = new List<DepartmentRequestItemViewModel>();

            foreach (var app in applications)
            {
                var visitors = await db.ApplicationVisitors
                    .Where(x => x.ApplicationId == app.ApplicationId)
                    .Join(db.Visitors,
                        av => av.VisitorId,
                        v => v.VisitorId,
                        (av, v) => v)
                    .ToListAsync();

                result.Add(new DepartmentRequestItemViewModel
                {
                    ApplicationId = app.ApplicationId,
                    TypeName = app.ApplicationType?.TypeName ?? "",
                    DepartmentName = app.Department?.DepartmentName ?? "",
                    EmployeeName = app.Employee?.FullName ?? "",
                    DateStart = app.DateStart.ToString("dd.MM.yyyy"),
                    DateEnd = app.DateEnd.ToString("dd.MM.yyyy"),
                    VisitorNames = string.Join(", ", visitors.Select(v => $"{v.LastName} {v.FirstName} {v.MiddleName}")),
                    StatusName = app.Status?.StatusName ?? ""
                });
            }

            return result;
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

        public async Task<(bool ok, string message)> ConfirmDepartmentArrivalAsync(int applicationId, int departmentEmployeeId)
        {
            using var db = new AppDbContext();

            var application = await db.Applications
                .Include(x => x.Department)
                .FirstAsync(x => x.ApplicationId == applicationId);

            var logs = await db.VisitLogs
                .Where(x => x.ApplicationId == applicationId)
                .ToListAsync();

            if (!logs.Any() || logs.Any(x => x.EntryTime == null))
            {
                return (false, "Сотрудник подразделения не может подтвердить приход, пока охрана не разрешила доступ.");
            }

            int transferMinutes = application.Department != null ? application.Department.TransferMinutesLimit : 15;
            bool violation = false;

            foreach (var log in logs)
            {
                log.DepartmentEmployeeId = departmentEmployeeId;
                log.DepartmentEntryTime = DateTime.UtcNow;

                if (log.EntryTime.HasValue)
                {
                    var diff = DateTime.UtcNow - log.EntryTime.Value;
                    if (diff.TotalMinutes > transferMinutes)
                    {
                        violation = true;
                        log.ViolationMessage = $"Превышено время перемещения: {Math.Round(diff.TotalMinutes)} мин.";
                    }
                }
            }

            await db.SaveChangesAsync();

            if (violation)
                return (true, "Приход подтвержден. Обнаружено превышение времени перемещения, требуется оповещение.");

            return (true, "Время прихода подтверждено.");
        }

        public async Task ConfirmDepartmentExitAsync(int applicationId, int departmentEmployeeId)
        {
            using var db = new AppDbContext();

            var logs = await db.VisitLogs
                .Where(x => x.ApplicationId == applicationId)
                .ToListAsync();

            foreach (var log in logs)
            {
                log.DepartmentEmployeeId = departmentEmployeeId;
                log.DepartmentExitTime = DateTime.UtcNow;
            }

            await db.SaveChangesAsync();
        }

        public async Task AddVisitorToBlackListAsync(int visitorId, string reason, int employeeId)
        {
            using var db = new AppDbContext();

            db.BlackListEntries.Add(new BlackListEntry
            {
                VisitorId = visitorId,
                Reason = reason,
                AddedByEmployeeId = employeeId,
                CreatedAt = DateTime.UtcNow
            });

            await db.SaveChangesAsync();
        }
    }
}