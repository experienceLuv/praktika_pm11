using KeeperProWpf.Security.Models;

namespace KeeperProWpf.Security.Session
{
    public static class SecuritySession
    {
        public static Employee? CurrentEmployee { get; set; }
    }
}