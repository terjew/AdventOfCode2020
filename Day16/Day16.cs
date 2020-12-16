using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Utilities;

namespace Day16
{
    class Constraint
    {
        public string Name;
        public int Min1;
        public int Max1;
        public int Min2;
        public int Max2;
        
        static readonly Regex re = new Regex(@"^(?<name>[\w\s]+): (?<n1>\d+)-(?<n2>\d+) or (?<n3>\d+)-(?<n4>\d+).*$", RegexOptions.Compiled);
        public Constraint(string line)
        {
            var match = re.Match(line);
            Name = match.Groups["name"].Value;
            Min1 = int.Parse(match.Groups["n1"].Value);
            Max1 = int.Parse(match.Groups["n2"].Value);
            Min2 = int.Parse(match.Groups["n3"].Value);
            Max2 = int.Parse(match.Groups["n4"].Value);
        }

        public bool Valid(int val)
        {
            return (val >= Min1 && val <= Max1) || (val >= Min2 && val <= Max2);
        }
    }

    class TicketScanner
    {
        List<Constraint> Constraints = new List<Constraint>();
        List<int[]> Tickets = new List<int[]>();
        int[] OwnTicket;

        public TicketScanner(List<string> input)
        {
            int part = 0;
            foreach (var line in input)
            {
                if (line.Length == 0)
                {
                    part++;
                    continue;
                }
                if (line.StartsWith("your ticket") || line.StartsWith("nearby tickets")) continue;
                if (part == 0) AddConstraint(line);
                else if (part == 1) AddOwnTicket(line);
                else AddNearbyTicket(line);
            }
        }

        private void AddOwnTicket(string line)
        {
            OwnTicket = line.Split(",").Select(s => int.Parse(s)).ToArray();
        }

        private void AddNearbyTicket(string line)
        {
            Tickets.Add(line.Split(",").Select(s => int.Parse(s)).ToArray());
        }

        private void AddConstraint(string line)
        {
            Constraints.Add(new Constraint(line));
        }

        public int GetErrorRate()
        {
            return Tickets.SelectMany(t => t).Where(f => Constraints.All(c => !c.Valid(f))).Sum();
        }

    }

    class Day16
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Day16!");

            var input = TextFile.ReadStringList("input.txt");
            var scanner = new TicketScanner(input);
            Console.WriteLine(scanner.GetErrorRate());
            
        }
    }
}
