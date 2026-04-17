using KeeperProWpf.Data;
using KeeperProWpf.Models;
using KeeperProWpf.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace KeeperProWpf.Services
{
    public class OfficeRequestService
    {
        public async Task<List<OfficeRequestItemViewModel>> GetApplicationsAsync(
            string? typeFilter,
            int? departmentId,
            int? statusId)
        {
            using var db = new AppDbContext();

            var query = db.Applications
                .Include(a => a.ApplicationType)
                .Include(a => a.Department)
                .Include(a => a.Employee)
                .Include(a => a.Status)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(typeFilter))
                query = query.Where(a => a.ApplicationType != null && a.ApplicationType.TypeName == typeFilter);

            if (departmentId.HasValue)
                query = query.Where(a => a.DepartmentId == departmentId.Value);

            if (statusId.HasValue)
                query = query.Where(a => a.StatusId == statusId.Value);

            var applications = await query
                .OrderByDescending(a => a.ApplicationId)
                .ToListAsync();

            var result = new List<OfficeRequestItemViewModel>();

            foreach (var app in applications)
            {
                var visitorNames = await db.ApplicationVisitors
                    .Where(x => x.ApplicationId == app.ApplicationId)
                    .Join(db.Visitors,
                        av => av.VisitorId,
                        v => v.VisitorId,
                        (av, v) => v.LastName + " " + v.FirstName + " " + (v.MiddleName ?? ""))
                    .ToListAsync();

                result.Add(new OfficeRequestItemViewModel
                {
                    ApplicationId = app.ApplicationId,
                    TypeName = app.ApplicationType?.TypeName ?? "",
                    DepartmentName = app.Department?.DepartmentName ?? "",
                    EmployeeName = app.Employee?.FullName ?? "",
                    DateStart = app.DateStart.ToString("dd.MM.yyyy"),
                    DateEnd = app.DateEnd.ToString("dd.MM.yyyy"),
                    VisitPurpose = app.VisitPurpose,
                    StatusName = app.Status?.StatusName ?? "",
                    VisitorNames = string.Join(", ", visitorNames)
                });
            }

            return result;
        }

        public async Task<List<Department>> GetDepartmentsAsync()
        {
            using var db = new AppDbContext();
            return await db.Departments.OrderBy(x => x.DepartmentName).ToListAsync();
        }

        public async Task<List<ApplicationStatus>> GetStatusesAsync()
        {
            using var db = new AppDbContext();
            return await db.ApplicationStatuses.OrderBy(x => x.StatusName).ToListAsync();
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

        public async Task<bool> IsBlackListedAsync(int applicationId)
        {
            return false;
        }

        public async Task ApproveApplicationAsync(int applicationId)
        {
            using var db = new AppDbContext();

            var application = await db.Applications.FirstAsync(x => x.ApplicationId == applicationId);
            int approvedStatusId = await db.ApplicationStatuses
                .Where(x => x.StatusName == "Одобрена")
                .Select(x => x.StatusId)
                .FirstAsync();

            application.StatusId = approvedStatusId;
            application.UpdatedAt = DateTime.UtcNow;

            await db.SaveChangesAsync();
        }

        public async Task RejectApplicationAsync(int applicationId, string reason)
        {
            using var db = new AppDbContext();

            var application = await db.Applications.FirstAsync(x => x.ApplicationId == applicationId);
            int rejectedStatusId = await db.ApplicationStatuses
                .Where(x => x.StatusName == "Не одобрена" || x.StatusName == "Отклонена")
                .Select(x => x.StatusId)
                .FirstAsync();

            application.StatusId = rejectedStatusId;
            application.RejectionReason = reason;
            application.UpdatedAt = DateTime.UtcNow;

            await db.SaveChangesAsync();
        }
    }
}