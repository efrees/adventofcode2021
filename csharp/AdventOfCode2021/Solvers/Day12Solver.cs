using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.Solvers
{
    internal class Day12Solver : ISolver
    {
        private const string Name = "Day 12";

        private const string InputFile = "day12input.txt";

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
            var graph = ParseGraph(input);

            return FullSearchSuccessCount(graph, "start", new HashSet<string>());
        }

        private static int GetPart2Answer(List<string> input)
        {
            var graph = ParseGraph(input);

            var visitedCount = graph.Keys.ToDictionary(k => k, k => 0);
            return FullSearchSuccessCount2(graph, "start", visitedCount);
        }

        private static Dictionary<string, HashSet<string>> ParseGraph(List<string> input)
        {
            var graph = new Dictionary<string, HashSet<string>>();
            foreach (var line in input)
            {
                var nodes = line.Split('-');
                SafeAdd(graph, nodes[0], nodes[1]);
                SafeAdd(graph, nodes[1], nodes[0]);
            }

            return graph;
        }

        private static int FullSearchSuccessCount(Dictionary<string, HashSet<string>> graph, string start, HashSet<string> visited)
        {
            if (start == "end")
            {
                return 1;
            }

            visited.Add(start);

            var pathCount = 0;

            foreach (var next in graph[start])
            {
                if (visited.Contains(next) && char.IsLower(next[0]))
                {
                    continue;
                }

                pathCount += FullSearchSuccessCount(graph, next, visited);
            }

            visited.Remove(start);
            return pathCount;
        }

        private static int FullSearchSuccessCount2(Dictionary<string, HashSet<string>> graph, string start, Dictionary<string, int> visited)
        {
            if (start == "end")
            {
                return 1;
            }

            visited[start]++;

            var pathCount = 0;

            foreach (var next in graph[start])
            {
                if (next == "start"
                    || char.IsLower(next[0]) && visited[next] > 0 && visited.Any(pair => char.IsLower(pair.Key[0]) && pair.Value > 1))
                {
                    continue;
                }

                pathCount += FullSearchSuccessCount2(graph, next, visited);
            }

            visited[start]--;
            return pathCount;
        }

        private static void SafeAdd(Dictionary<string, HashSet<string>> graph, string node1, string node2)
        {
            if (!graph.ContainsKey(node1))
            {
                graph[node1] = new HashSet<string>();
            }

            graph[node1].Add(node2);
        }
    }
}