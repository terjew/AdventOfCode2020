using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Utilities
{
    public static class Extensions
    {
        public static int GetInt(this Match m, string groupName)
        {
            int y = 0;
            var s = m.Groups[groupName].ToString();
            for (int i = 0; i < s.Length; i++)
                y = y * 10 + (s[i] - '0');
            return y;
        }
    }
}
