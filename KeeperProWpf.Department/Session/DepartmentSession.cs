using KeeperProWpf.Department.Models;

namespace KeeperProWpf.Department.Session
{
    public static class DepartmentSession
    {
        public static Employee? CurrentEmployee { get; set; }
    }
}