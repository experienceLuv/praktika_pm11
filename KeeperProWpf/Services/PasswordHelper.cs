using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace KeeperProWpf.Services
{
    public static class PasswordHelper
    {
        public static string ToMd5(string input)
        {
            using MD5 md5 = MD5.Create();
            byte[] bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sb = new StringBuilder();

            foreach (byte b in bytes)
                sb.Append(b.ToString("x2"));

            return sb.ToString();
        }

        public static bool IsPasswordValid(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            if (password.Length < 8)
                return false;

            if (!Regex.IsMatch(password, "[A-ZА-Я]"))
                return false;

            if (!Regex.IsMatch(password, "[a-zа-я]"))
                return false;

            if (!Regex.IsMatch(password, "[0-9]"))
                return false;

            if (!Regex.IsMatch(password, @"[^a-zA-Zа-яА-Я0-9]"))
                return false;

            return true;
        }
    }
}