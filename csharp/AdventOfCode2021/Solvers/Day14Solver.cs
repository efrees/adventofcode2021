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
            Console.WriteLine($"Output (part 2): {GetPart2Answer(lines)}");
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

        private static long GetPart2Answer(List<string> input)
        {
            var template = input.First();

            var rules = new Dictionary<(char, char), char>();
            foreach (var rule in input.Skip(2))
            {
                var splits = rule.Split(" -> ");
                rules.Add((splits[0][0], splits[0][1]), splits[1].Single());
            }

            var uniqueLetters = input.SelectMany(line => line).Where(char.IsLetter).Distinct().ToArray();
            var resultingCounts = new Dictionary<(char, char, int), Dictionary<char, long>>();
            foreach (var first in uniqueLetters)
            {
                foreach (var second in uniqueLetters)
                {
                    resultingCounts.Add((first, second, 0), SingleCountsDictionary(first, second));
                }
            }

            for (var i = 1; i <= 40; i++)
            {
                foreach (var first in uniqueLetters)
                {
                    foreach (var second in uniqueLetters)
                    {
                        if (!rules.ContainsKey((first, second)))
                        {
                            resultingCounts.Add((first, second, i), SingleCountsDictionary(first, second));
                        }
                        else
                        {
                            var inserted = rules[(first, second)];
                            var leftCounts = resultingCounts[(first, inserted, i - 1)];
                            var rightCounts = resultingCounts[(inserted, second, i - 1)];
                            var combined = CombineCounts(leftCounts, rightCounts);
                            combined[inserted]--; //remove overlap;
                            resultingCounts.Add((first, second, i), combined);
                        }
                    }
                }
            }

            var totalCounts = new Dictionary<char, long>();
            foreach (var pair in template.Zip(template.Skip(1)))
            {
                var counts = resultingCounts[(pair.First, pair.Second, 40)];
                totalCounts = CombineCounts(totalCounts, counts);
            }

            foreach (var ch in template[1..^1])
            {
                totalCounts[ch]--;
            }

            var totals = totalCounts.Values.OrderBy(x => x).ToList();
            return totals.Last() - totals.First();
        }

        private static Dictionary<char, long> SingleCountsDictionary(char char1, char char2)
        {
            if (char1 == char2)
            {
                return new Dictionary<char, long>
                {
                    { char1, 2 }
                };
            }

            return new Dictionary<char, long>
            {
                { char1, 1 },
                { char2, 1 }
            };
        }

        private static Dictionary<char, long> CombineCounts(Dictionary<char, long> firstCounts, Dictionary<char, long> secondCounts)
        {
            var newFirst = new Dictionary<char, long>(firstCounts);
            foreach (var (ch, value) in secondCounts)
            {
                SafeAddToCount(newFirst, ch, value);
            }

            return newFirst;
        }

        private static void SafeAddToCount(Dictionary<char, long> totalCounts, char ch, long value)
        {
            totalCounts[ch] = totalCounts.GetValueOrDefault(ch, 0) + value;
        }
    }
}