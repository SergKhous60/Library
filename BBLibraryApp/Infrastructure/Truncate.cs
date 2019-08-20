using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BBLibraryApp.Infrastructure
{
    public static class Truncate
    {
        public static string TruncateString(this string source, int lengthExpected, string separator = "...")
        {
            if (source == null)
            {
                source = "";
            }
            if (source.Length <= lengthExpected)
            {
                return source;
            }

            int sepLen = separator.Length;
            int charsToShow = lengthExpected - sepLen;

            return source.Substring(0, charsToShow) + separator;
        }
    }
}
