using System;
using System.Collections.Generic;
using System.Linq;
using Utilities;

namespace Day6
{
    class Group
    {
        public List<HashSet<char>> Answers { get; } = new List<HashSet<char>>();
        public HashSet<char> Union { get; } = new HashSet<char>();
        public HashSet<char> Intersection { get; private set; }

        public void AddLine(string s)
        {
            var answer = new HashSet<char>(s);
            Answers.Add(answer);
            Union.UnionWith(answer);
            if (Intersection == null) Intersection = new HashSet<char>(answer);
            else Intersection.IntersectWith(answer);
        }
    }

    class Day6
    {
        static void Main(string[] args)
        {
            int unionSum = 0;
            int intersectionSum = 0;

            Performance.TimeRun("Read and solve", () =>
            {
                var lines = TextFile.ReadStringList("input.txt");
                List<Group> answerGroups = new List<Group>();
                Group group = new Group();
                foreach (var line in lines)
                {
                    if (line.Length == 0)
                    {
                        answerGroups.Add(group);
                        group = new Group();
                        continue;
                    }
                    group.AddLine(line);
                }
                answerGroups.Add(group);

                unionSum = answerGroups.Sum(g => g.Union.Count);
                intersectionSum = answerGroups.Sum(g => g.Intersection.Count);
            });

            Console.WriteLine($"Sum of answers with at least one yes is {unionSum}");
            Console.WriteLine($"Sum of answers with all yes is {intersectionSum}");
        }


    }
}
