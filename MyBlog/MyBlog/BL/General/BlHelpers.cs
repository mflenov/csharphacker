using System;
namespace MyBlog.BL.General
{
    public static class BlHelpers
    {
        public static string NormilizeEmail(string email)
        {
            var parts = email.Split('@');
            if (parts.Length != 2)
                return email;
            string name = parts[0].Replace(".", "");
            if (name.IndexOf("+") > 0)
                name = name.Substring(0, name.IndexOf("+"));
            return name + "@" + parts[1];
        }
    }
}

