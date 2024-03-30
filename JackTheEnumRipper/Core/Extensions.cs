using System;
using System.Linq;
using System.Text;

namespace JackTheEnumRipper.Core
{
    public static class Extensions
    {
        public static string Repeat(this string source, int times)
        {
            return (times != 0) ? string.Concat(Enumerable.Repeat(source, times)) : source;
        }

        public static bool IsValidEncoding(this Encoding encoding, string name)
        {
            try
            {
                _ = Encoding.GetEncoding(name);
                return true;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }
    }
}
