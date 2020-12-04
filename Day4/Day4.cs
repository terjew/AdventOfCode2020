using System;
using System.Collections.Generic;
using System.Linq;
using Utilities;

namespace Day4
{
    public class Passport
    {
        public static bool IsBetween(int value, int min, int max)
        {
            return value >= min && value <= max;
        }

        private static readonly HashSet<string> Eyecolors = new HashSet<string>() { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };

        public static readonly Dictionary<string, Func<string, bool>> RequiredRecords = new Dictionary<string, Func<string, bool>>
        {
            {"byr", (s) => s.Length == 4 && IsBetween(FastString.ParseInt(s, 0, 4), 1920, 2002) },
            {"iyr", (s) => s.Length == 4 && IsBetween(FastString.ParseInt(s, 0, 4), 2010, 2020) },
            {"eyr", (s) => s.Length == 4 && IsBetween(FastString.ParseInt(s, 0, 4), 2020, 2030) },
            {"hgt", (s) => FastString.EndsWith(s, "cm", 2) ? IsBetween(FastString.ParseInt(s, 0, 3), 150, 193) : FastString.EndsWith(s, "in", 2) ? IsBetween(FastString.ParseInt(s, 0, 2), 59, 76) : false },
            {"hcl", (s) => s.StartsWith('#') && IsBetween(FastString.ParseHex(s, 1, 6), 0, 0xFFFFFF)},
            {"ecl", (s) => Eyecolors.Contains(s) },
            {"pid", (s) => s.Length == 9 && IsBetween(FastString.ParseInt(s, 0, 9), 0, 999999999)},
            //"cid"
        };

        public Dictionary<string, string> Records { get; } = new Dictionary<string, string>();

        public Passport(List<string> strings, ref int i)
        {
            while (i < strings.Count)
            {
                if (strings[i].Length <= 4)
                {
                    i++;
                    return;
                }
                var records = strings[i].Split(" ");
                foreach(var record in records)
                {
                    var parts = record.Split(":");
                    Records[parts[0]] = parts[1];
                }
                i++;
            }
        }

        public bool HasRequiredRecords()
        {
            foreach (var req in RequiredRecords)
            {
                if (!Records.ContainsKey(req.Key)) return false;
            }
            return true;
        }

        public bool IsValid()
        {
            if (!HasRequiredRecords()) return false;
            foreach (var req in RequiredRecords)
            {
                var value = Records[req.Key];
                var isValidFunc = req.Value;
                if (!isValidFunc(value))
                {
                    return false;
                }

            }
            return true;
        }
    }

    class Day4
    {
        
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Day 4!");

            int hasFields = 0;
            int validFields = 0;
            Performance.TimeRun("parse and check", () =>
            {
                var lines = TextFile.ReadStringList("input.txt");
                int lineNo = 0;
                var passports = new List<Passport>();
                while (lineNo < lines.Count)
                {
                    passports.Add(new Passport(lines, ref lineNo));
                }
                hasFields = passports.Count(p => p.HasRequiredRecords());
                validFields = passports.Count(p => p.IsValid());
            }, 10, 1000);
            Console.WriteLine($"Valid passports (required fields): {hasFields}");
            Console.WriteLine($"Valid passports (correct values): {validFields}");
        }


    }
}
