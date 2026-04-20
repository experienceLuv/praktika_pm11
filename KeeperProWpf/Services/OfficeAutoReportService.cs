using KeeperProWpf.Data;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Windows.Threading;

namespace KeeperProWpf.Services
{
    public class OfficeAutoReportService
    {
        private readonly DispatcherTimer _timer = new DispatcherTimer();

        public void Start()
        {
            EnsureBaseFolders();

            _timer.Interval = TimeSpan.FromMinutes(1);
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private async void Timer_Tick(object? sender, EventArgs e)
        {
            DateTime now = DateTime.Now;

            if (now.Minute == 0 && (now.Hour % 3 == 0))
            {
                string markerFile = GetMarkerFilePath(now);

                if (!File.Exists(markerFile))
                {
                    await GenerateVisitorsCountReportAsync(now);
                    File.WriteAllText(markerFile, now.ToString("dd.MM.yyyy HH:mm"));
                }
            }
        }

        private void EnsureBaseFolders()
        {
            string reportsRoot = GetReportsRootFolder();

            if (!Directory.Exists(reportsRoot))
                Directory.CreateDirectory(reportsRoot);

            string todayFolder = GetTodayFolder();
            if (!Directory.Exists(todayFolder))
                Directory.CreateDirectory(todayFolder);
        }

        private string GetReportsRootFolder()
        {
            string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            return Path.Combine(documents, "Отчеты ТБ");
        }

        private string GetTodayFolder()
        {
            string reportsRoot = GetReportsRootFolder();
            string folderName = DateTime.Now.ToString("dd_MM_yyyy");
            return Path.Combine(reportsRoot, folderName);
        }

        private string GetMarkerFilePath(DateTime dt)
        {
            string todayFolder = GetTodayFolder();
            string markerName = $"report_{dt:HH_mm}.done";
            return Path.Combine(todayFolder, markerName);
        }

        public async Task GenerateVisitorsCountReportAsync(DateTime reportTime)
        {
            using var db = new AppDbContext();

            DateTime endTime = reportTime.ToUniversalTime();
            DateTime startTime = endTime.AddHours(-3);

            var items = await db.VisitLogs
                .Include(vl => vl.Application)
                    .ThenInclude(a => a.Department)
                .Where(vl =>
                    vl.EntryTime.HasValue &&
                    vl.EntryTime.Value >= startTime &&
                    vl.EntryTime.Value < endTime)
                .GroupBy(vl => vl.Application != null && vl.Application.Department != null
                    ? vl.Application.Department.DepartmentName
                    : "Неизвестно")
                .Select(g => new
                {
                    DepartmentName = g.Key,
                    VisitCount = g.Count()
                })
                .OrderBy(x => x.DepartmentName)
                .ToListAsync();

            string todayFolder = GetTodayFolder();
            if (!Directory.Exists(todayFolder))
                Directory.CreateDirectory(todayFolder);

            string fileName = $"Отчет_ТБ_{reportTime:dd_MM_yyyy_HH_mm}.txt";
            string fullPath = Path.Combine(todayFolder, fileName);

            using var writer = new StreamWriter(fullPath, false, System.Text.Encoding.UTF8);

            await writer.WriteLineAsync("ОТЧЕТ ТБ");
            await writer.WriteLineAsync($"Период: {startTime.ToLocalTime():dd.MM.yyyy HH:mm} - {endTime.ToLocalTime():dd.MM.yyyy HH:mm}");
            await writer.WriteLineAsync("");

            if (items.Count == 0)
            {
                await writer.WriteLineAsync("За указанный период посещений не найдено.");
            }
            else
            {
                await writer.WriteLineAsync("Подразделение | Количество посетителей");
                await writer.WriteLineAsync("--------------------------------------");

                foreach (var item in items)
                {
                    await writer.WriteLineAsync($"{item.DepartmentName} | {item.VisitCount}");
                }
            }
        }
    }
}