using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

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

        public static List<long> ReadPositiveLongList(string path)
        {
            List<long> list = new List<long>();
            using (StreamReader sr = File.OpenText(path))
            {
                string s = String.Empty;
                while ((s = sr.ReadLine()) != null)
                {
                    long y = 0;
                    for (int i = 0; i < s.Length; i++)
                        y = y * 10 + (s[i] - '0');
                    list.Add(y);
                }
            }
            return list;
        }

        public static List<string> ReadStringList(string path)
        {
            List<string> list = new List<string>();
            using (StreamReader sr = File.OpenText(path))
            {
                string s = String.Empty;
                while ((s = sr.ReadLine()) != null)
                {
                    list.Add(s);
                }
            }
            return list;
        }


    }


}
