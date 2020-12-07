using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Utilities;

namespace Day7
{
    class BagType
    {
        public string Color { get; }
        Dictionary<BagType, int> Children { get; } = new Dictionary<BagType, int>();
        List<BagType> Parents { get; } = new List<BagType>();
        public BagType(string color) { Color = color; }

        public void AddChild(int count, BagType child)
        {
            Children.Add(child, count);
            child.Parents.Add(this);
        }

        public IEnumerable<BagType> GetAncestors()
        {
            foreach(var parent in Parents)
            {
                yield return parent;
                foreach (var grandparent in parent.GetAncestors()) yield return grandparent;
            }
        }

        public int CountDescendents()
        {
            int sum = 0;
            foreach(var child in Children)
            {
                var count = child.Value;
                var childCount = child.Key.CountDescendents();
                sum += count * (childCount + 1); //add the child itself
            }
            return sum;
        }
    }

    class Day7
    {
        static BagType Get(string color, Dictionary<string, BagType> dict)
        {
            if (!dict.ContainsKey(color))
            {
                dict.Add(color, new BagType(color));
            }
            return dict[color];
        }

        
        static void Main(string[] args)
        {
            int ancestorCount = 0;
            int descendentCount = 0;
            Performance.TimeRun("read and solve", () =>
            {

                Dictionary<string, BagType> bagtypes = new Dictionary<string, BagType>();
                foreach (var line in TextFile.ReadStringList("input.txt"))
                {
                    var sentence = line.Split(" bags contain ");
                    var parentType = Get(sentence[0], bagtypes);
                    var children = Regex.Matches(sentence[1], @"(?<count>\d) (?<color>\w* \w*) bags?");
                    foreach (Match match in children)
                    {
                        var count = int.Parse(match.Groups["count"].Value);
                        var color = match.Groups["color"].Value;
                        var childType = Get(color, bagtypes);
                        parentType.AddChild(count, childType);
                    }
                }
                var gold = bagtypes["shiny gold"];

                ancestorCount = gold.GetAncestors().Distinct().Count();
                descendentCount = gold.CountDescendents();
            });

            Console.WriteLine($"{ancestorCount} ancestors");
            Console.WriteLine($"{descendentCount} descendents");
        }
    }
}
