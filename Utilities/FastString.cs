using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public static class FastString
    {
        public static int ParseInt(string s, int start, int digits)
        {
            if ((start + digits) > s.Length) return -1;
            int y = 0;
            for (int i = 0; i < digits; i++)
            {
                int digit = s[start + i] - '0';
                if (digit < 0 || digit > 9) return -1;
                y = y * 10 + digit;
            }
            return y;
        }

        public static int ParseHex(string s, int start, int digits)
        {
            if ((start + digits) > s.Length) return -1;
            int y = 0;
            for (int i = 0; i < digits; i++)
            {
                char c = s[start + i];
                int digit = 0;
                if (c >= '0' && c <= '9') digit = c - '0';
                else if (c >= 'a' && c <= 'f') digit = 10 + c - 'a';
                else return -1;
                y = y * 16 + digit;
            }
            return y;
        }

        public static bool EndsWith(string s, string end, int count)
        {
            var i1 = s.Length - count;
            var i2 = end.Length - count;
            for (int i = 0; i < count; i++)
            {
                if (s[i1++] != end[i2++]) return false;
            }
            return true;
        }

    }
}
