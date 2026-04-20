using KeeperProWpf.Data;
using KeeperProWpf.Office.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace KeeperProWpf.Services
{
    public class OfficeReportService
    {
        private DateTime ToUtcDate(DateTime dt)
        {
            return DateTime.SpecifyKind(dt.Date, DateTimeKind.Utc);
        }

        public async Task<List<VisitCountReportItem>> GetVisitCountReportAsync(string periodType, DateTime selectedDate)
        {
            using var db = new AppDbContext();

            DateTime startUtc;
            DateTime endUtc;

            var baseDate = ToUtcDate(selectedDate);

            if (periodType == "День")
            {
                startUtc = baseDate;
                endUtc = startUtc.AddDays(1);
            }
            else if (periodType == "Месяц")
            {
                startUtc = new DateTime(baseDate.Year, baseDate.Month, 1, 0, 0, 0, DateTimeKind.Utc);
                endUtc = startUtc.AddMonths(1);
            }
            else // Год
            {
                startUtc = new DateTime(baseDate.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                endUtc = startUtc.AddYears(1);
            }

            var query = db.VisitLogs
                .Include(vl => vl.Application)
                    .ThenInclude(a => a.Department)
                .Where(vl =>
                    vl.EntryTime.HasValue &&
                    vl.EntryTime.Value >= startUtc &&
                    vl.EntryTime.Value < endUtc);

            return await query
                .GroupBy(vl => vl.Application != null && vl.Application.Department != null
                    ? vl.Application.Department.DepartmentName
                    : "Неизвестно")
                .Select(g => new VisitCountReportItem
                {
                    DepartmentName = g.Key,
                    VisitCount = g.Count()
                })
                .OrderBy(x => x.DepartmentName)
                .ToListAsync();
        }

        public async Task<List<CurrentPersonsReportItem>> GetCurrentPersonsReportAsync()
        {
            using var db = new AppDbContext();

            return await db.VisitLogs
                .Include(vl => vl.Application)
                    .ThenInclude(a => a.Department)
                .Include(vl => vl.Visitor)
                .Where(vl =>
                    vl.EntryTime.HasValue &&
                    !vl.ExitTime.HasValue &&
                    !vl.DepartmentExitTime.HasValue)
                .Select(vl => new CurrentPersonsReportItem
                {
                    DepartmentName = vl.Application != null && vl.Application.Department != null
                        ? vl.Application.Department.DepartmentName
                        : "Неизвестно",
                    VisitorFullName = vl.Visitor != null
                        ? (vl.Visitor.LastName + " " + vl.Visitor.FirstName + " " + (vl.Visitor.MiddleName ?? "")).Trim()
                        : "",
                    Passport = vl.Visitor != null
                        ? (vl.Visitor.PassportSeries + " " + vl.Visitor.PassportNumber)
                        : "",
                    EntryTime = vl.EntryTime.HasValue
                        ? vl.EntryTime.Value.ToString("dd.MM.yyyy HH:mm")
                        : ""
                })
                .OrderBy(x => x.DepartmentName)
                .ThenBy(x => x.VisitorFullName)
                .ToListAsync();
        }
    }
}