using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Utilities
{
    public class TextFile
    {
        public static List<int> ReadPositiveIntList(string path)
        {
            List<int> list = new List<int>();
            using (StreamReader sr = File.OpenText(path))
            {
                string s = String.Empty;
                while ((s = sr.ReadLine()) != null)
                {
                    int y = 0;
                    for (int i = 0; i < s.Length; i++)
                        y = y * 10 + (s[i] - '0');
                    list.Add(y);
                }
            }
            return list;
        }

    }
}
