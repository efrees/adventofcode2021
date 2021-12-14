using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2021.Solvers
{
    internal class Day14Solver : ISolver
    {
        private const string Name = "Day 14";

        private const string InputFile = "day14input.txt";

        public void Solve()
        {
            Console.WriteLine(Name);
            var lines = Input.GetLinesFromFile(InputFile)
                .ToList();

            Console.WriteLine($"Output (part 1): {GetPart1Answer(lines)}");
            //Console.WriteLine($"Output (part 2): {GetPart2Answer(lines)}");
        }

        private static int GetPart1Answer(List<string> input)
        {
            var template = input.First();

            var rules = new Dictionary<string, char>();
            foreach (var rule in input.Skip(2))
            {
                var splits = rule.Split(" -> ");
                rules.Add(splits[0], splits[1].Single());
            }

            for (var i = 0; i < 10; i++)
            {
                template = ApplyRules(rules, template);
            }

            var counts = template.GroupBy(c => c).Select(g => g.Count()).OrderBy(g => g).ToList();
            return counts.Last() - counts.First();
        }

        private static string ApplyRules(Dictionary<string, char> rules, string template)
        {
            var sb = new StringBuilder();
            foreach (var pair in template.Zip(template.Skip(1)))
            {
                sb.Append(pair.First);
                var key = new string(new[] { pair.First, pair.Second });

                if (rules.ContainsKey(key))
                {
                    sb.Append(rules[key]);
                }
            }

            sb.Append(template.Last());

            return sb.ToString();
        }
    }
}