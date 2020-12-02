using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Utilities;

namespace Day2
{
    class PasswordLine
    {
        public int min;
        public int max;
        public char c;
        public string password;

        public bool IsValidRule1()
        {
            int count = 0;
            for (int i = 0; i < password.Length; i++)
            {
                if (password[i] == c) count++;
            }
            if (count < min || count > max) return false;
            return true;
        }


        public bool IsValidRule2()
        {
            return password[min - 1] == c ^ password[max - 1] == c;
        }
    }

    class Day2
    {
        static void Main(string[] args)
        {
            int valid1 = 0;
            int valid2 = 0;
            Performance.TimeRun("Read and check passwords", () =>
            {
                var passwords = ParsePasswords("puzzle.txt");
                valid1 = 0;
                valid2 = 0;
                for (int i = 0; i < passwords.Count; i++)
                {
                    if (passwords[i].IsValidRule1()) valid1++;
                    if (passwords[i].IsValidRule2()) valid2++;
                }
            });
            Console.WriteLine($"{valid1} valid passwords found using rule 1");
            Console.WriteLine($"{valid2} valid passwords found using rule 2");
        }

        static List<PasswordLine> ParsePasswords(string path)
        {
            List<PasswordLine> lines = new List<PasswordLine>();
            using (StreamReader sr = File.OpenText(path))
            {
                string s = String.Empty;
                while ((s = sr.ReadLine()) != null)
                {
                    var line = new PasswordLine();

                    int num = 0;
                    int i = 0;
                    char c = s[i];
                    while(c != '-')
                    {
                        num = num * 10 + (c - '0');
                        c = s[++i];
                    }
                    line.min = num;
            
                    num = 0;
                    c = s[++i];
                    while (c != ' ')
                    {
                        num = num * 10 + (c - '0');
                        c = s[++i];
                    }
                    line.max = num;

                    line.c = s[++i];

                    line.password = s.Substring(i+3);
                    lines.Add(line);
                }
            }
            return lines;
        }

        static List<PasswordLine> ParsePasswordsRegex(string path)
        {
            List<PasswordLine> passwords = new List<PasswordLine>();
            Regex re = new Regex(@"(?<min>\d+)-(?<max>\d+) (?<char>\w): (?<password>.*)", RegexOptions.Compiled);
            int i = 0;
            using (StreamReader sr = File.OpenText(path))
            {
                string s = String.Empty;
                while ((s = sr.ReadLine()) != null)
                {
                    var match = re.Match(s);
                    var password = new PasswordLine();
                    password.min = match.GetInt("min");
                    password.max = match.GetInt("max");
                    password.c = match.Groups["char"].ToString()[0];
                    password.password = match.Groups["password"].ToString();
                    passwords.Add(password);
                    i++;
                }
            }
            return passwords;
        }

    }
}
