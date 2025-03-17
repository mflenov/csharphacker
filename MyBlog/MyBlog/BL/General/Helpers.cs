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

        public static Guid? StringToGuidDef(string str)
        {
            Guid value;
            if (Guid.TryParse(str, out value))
                return value;
            return null;
        }
    }
}

