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

        public int Width;
        
        static readonly Regex re = new Regex(@"^(?<name>[\w\s]+): (?<n1>\d+)-(?<n2>\d+) or (?<n3>\d+)-(?<n4>\d+).*$", RegexOptions.Compiled);
        public Constraint(string line)
        {
            var match = re.Match(line);
            Name = match.Groups["name"].Value;
            Min1 = int.Parse(match.Groups["n1"].Value);
            Max1 = int.Parse(match.Groups["n2"].Value);
            Min2 = int.Parse(match.Groups["n3"].Value);
            Max2 = int.Parse(match.Groups["n4"].Value);
            Width = Max2 - Min2 + Max1 - Min1;
        }

        public bool Valid(int val)
        {
            return (val >= Min1 && val <= Max1) || (val >= Min2 && val <= Max2);
        }

        public override string ToString()
        {
            return Name;
        }
    }

    class TicketScanner
    {
        public List<Constraint> Constraints = new List<Constraint>();
        public List<int[]> Tickets = new List<int[]>();
        public int[] OwnTicket;

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

        private IEnumerable<(Constraint, List<int>)> GetValidCombinations()
        {
            var validTickets = Tickets.Where(t => t.All(f => Constraints.Any(c => c.Valid(f)))).ToArray();
            foreach(var constraint in Constraints)
            {
                List<int> validColumns = new List<int>();
                for(int column = 0; column < OwnTicket.Length; column++)
                {
                    if (validTickets.All(t => constraint.Valid(t[column])))
                    {
                        validColumns.Add(column);
                    }
                }
                yield return (constraint, validColumns);
            }
        }

        public IEnumerable<(Constraint, int)> MatchColumns()
        {
            List<int> possibleColumns = Enumerable.Range(0, OwnTicket.Length).ToList();
            var fields = GetValidCombinations().OrderBy(tuple => tuple.Item2.Count);
            foreach(var field in fields)
            {
                var column = field.Item2.Single(c => possibleColumns.Contains(c));
                possibleColumns.Remove(column);
                yield return (field.Item1, column);
            }
        }
    }

    class Day16
    {
        static void Main(string[] args)
        {
            int part1 = 0;
            long part2 = 0;

            Performance.TimeRun("part 1 and 2", () =>
            {
                var input = TextFile.ReadStringList("input.txt");
                var scanner = new TicketScanner(input);
                part1 = scanner.GetErrorRate();
                var matchedColumns = scanner.MatchColumns();
                var myTicket = scanner.OwnTicket;
                part2 = matchedColumns.Where(tuple => tuple.Item1.Name.StartsWith("departure")).Select(tuple => (long)myTicket[tuple.Item2]).Aggregate((a, b) => a * b);
            });

            Console.WriteLine(part1);
            Console.WriteLine(part2);
        }
    }
}
