using System;

namespace KeeperProWpf.Security.Models
{
    public class GroupVisitor
    {
        public string LastName { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string MiddleName { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Email { get; set; } = "";
        public string Organization { get; set; } = "";
        public DateTime BirthDate { get; set; }
        public string PassportSeries { get; set; } = "";
        public string PassportNumber { get; set; } = "";
    }
}