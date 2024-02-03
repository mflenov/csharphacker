using System;
namespace MyBlog.BL.General
{
    public static class Helpers
    {
        public static int? StringToIntDef(string str, int? def)
        {
            int value;
            if (int.TryParse(str, out value))
                return value;
            return def;
        }
    }
}

