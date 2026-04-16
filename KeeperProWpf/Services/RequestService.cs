using KeeperProWpf.Data;
using KeeperProWpf.Models;
using KeeperProWpf.Session;
using KeeperProWpf.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace KeeperProWpf.Services
{
    public class RequestService
    {
        public async Task<List<Department>> GetDepartmentsAsync()
        {
            using var db = new AppDbContext();

            return await db.Departments
                .OrderBy(x => x.DepartmentName)
                .ToListAsync();
        }

        public async Task<List<Employee>> GetEmployeesByDepartmentAsync(int departmentId)
        {
            using var db = new AppDbContext();

            return await db.Employees
                .Where(x => x.DepartmentId == departmentId && x.IsActive)
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .ToListAsync();
        }

        public async Task<int> GetIndividualApplicationTypeIdAsync()
        {
            using var db = new AppDbContext();

            return await db.ApplicationTypes
                .Where(x => x.TypeName == "individual")
                .Select(x => x.ApplicationTypeId)
                .FirstAsync();
        }

        public async Task<int> GetCheckingStatusIdAsync()
        {
            using var db = new AppDbContext();

            return await db.ApplicationStatuses
                .Where(x => x.StatusName == "На проверке")
                .Select(x => x.StatusId)
                .FirstAsync();
        }

        public async Task<List<ApplicationListItem>> GetUserApplicationsAsync(int userId)
        {
            using var db = new AppDbContext();

            var items = await db.Applications
                .Include(x => x.ApplicationType)
                .Include(x => x.Department)
                .Include(x => x.Employee)
                .Include(x => x.Status)
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new ApplicationListItem
                {
                    ApplicationId = x.ApplicationId,
                    ApplicationType = x.ApplicationType != null ? x.ApplicationType.TypeName : "",
                    DepartmentName = x.Department != null ? x.Department.DepartmentName : "",
                    EmployeeName = x.Employee != null
                        ? (x.Employee.LastName + " " + x.Employee.FirstName + " " + (x.Employee.MiddleName ?? "")).Trim()
                        : "",
                    DateStart = x.DateStart.ToString("dd.MM.yyyy"),
                    DateEnd = x.DateEnd.ToString("dd.MM.yyyy"),
                    VisitPurpose = x.VisitPurpose,
                    StatusName = x.Status != null ? x.Status.StatusName : "",
                    RejectionReason = x.RejectionReason ?? "",
                    CreatedAt = x.CreatedAt.ToString("dd.MM.yyyy HH:mm")
                })
                .ToListAsync();

            return items;
        }

        public async Task SaveIndividualRequestAsync(
            DateTime dateStart,
            DateTime dateEnd,
            string visitPurpose,
            int departmentId,
            int employeeId,
            string lastName,
            string firstName,
            string? middleName,
            string? phone,
            string email,
            string? organization,
            string note,
            DateTime birthDate,
            string passportSeries,
            string passportNumber,
            string? photoSourcePath,
            string passportPdfSourcePath)
        {
            if (UserSession.CurrentUser == null)
                throw new Exception("Пользователь не авторизован.");

            using var db = new AppDbContext();

            int applicationTypeId = await GetIndividualApplicationTypeIdAsync();
            int statusId = await GetCheckingStatusIdAsync();

            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string photosDir = Path.Combine(baseDir, "Files", "Photos");
            string docsDir = Path.Combine(baseDir, "Files", "Documents");

            Directory.CreateDirectory(photosDir);
            Directory.CreateDirectory(docsDir);

            string? savedPhotoPath = null;

            if (!string.IsNullOrWhiteSpace(photoSourcePath))
            {
                string photoExt = Path.GetExtension(photoSourcePath).ToLower();
                string photoFileName = $"{Guid.NewGuid()}{photoExt}";
                string targetPhotoPath = Path.Combine(photosDir, photoFileName);

                File.Copy(photoSourcePath, targetPhotoPath, true);
                savedPhotoPath = targetPhotoPath;
            }

            string pdfExt = Path.GetExtension(passportPdfSourcePath).ToLower();
            string pdfFileName = $"{Guid.NewGuid()}{pdfExt}";
            string targetPdfPath = Path.Combine(docsDir, pdfFileName);

            File.Copy(passportPdfSourcePath, targetPdfPath, true);

            try
            {
                var visitor = new Visitor
                {
                    LastName = lastName,
                    FirstName = firstName,
                    MiddleName = middleName,
                    Phone = phone,
                    Email = email,
                    Organization = organization,
                    BirthDate = DateTime.SpecifyKind(birthDate.Date, DateTimeKind.Utc),
                    PassportSeries = passportSeries,
                    PassportNumber = passportNumber,
                    PhotoPath = savedPhotoPath
                };

                db.Visitors.Add(visitor);
                await db.SaveChangesAsync();

                var application = new ApplicationEntity
                {
                    UserId = UserSession.CurrentUser.UserId,
                    ApplicationTypeId = applicationTypeId,
                    DepartmentId = departmentId,
                    EmployeeId = employeeId,
                    DateStart = DateTime.SpecifyKind(dateStart.Date, DateTimeKind.Utc),
                    DateEnd = DateTime.SpecifyKind(dateEnd.Date, DateTimeKind.Utc),
                    VisitPurpose = visitPurpose,
                    Note = note,
                    StatusId = statusId,
                    RejectionReason = null,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                db.Applications.Add(application);
                await db.SaveChangesAsync();

                var appVisitor = new ApplicationVisitor
                {
                    ApplicationId = application.ApplicationId,
                    VisitorId = visitor.VisitorId,
                    VisitorOrder = 1
                };

                db.ApplicationVisitors.Add(appVisitor);
                await db.SaveChangesAsync();

                var document = new DocumentEntity
                {
                    ApplicationId = application.ApplicationId,
                    VisitorId = visitor.VisitorId,
                    DocumentTypeId = 1,
                    FileName = Path.GetFileName(passportPdfSourcePath),
                    FilePath = targetPdfPath,
                    UploadedAt = DateTime.UtcNow
                };

                db.Documents.Add(document);
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                if (ex.InnerException != null)
                    msg += "\n\nINNER:\n" + ex.InnerException.Message;

                throw new Exception(msg);
            }
        }
    }
}