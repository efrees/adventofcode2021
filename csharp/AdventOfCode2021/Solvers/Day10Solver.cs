using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.Solvers
{
    internal class Day10Solver : ISolver
    {
        private const string Name = "Day 10";

        private const string InputFile = "day10input.txt";

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
            var totalScore = 0;
            var openingChars = new int[] { '(', '[', '{', '<' };
            foreach (var line in input)
            {
                var stack = new Stack<char>();
                foreach (var c in line)
                {
                    if (openingChars.Contains(c))
                    {
                        stack.Push(c);
                    }
                    else if (stack.Peek() == MatchFor(c))
                    {
                        stack.Pop();
                    }
                    else
                    {
                        totalScore += ScoreInvalid(c);
                        break;
                    }
                }
            }

            return totalScore;
        }

        private static long GetPart2Answer(List<string> input)
        {
            var openingChars = new int[] { '(', '[', '{', '<' };
            var scores = new List<long>();
            foreach (var line in input)
            {
                var stack = new Stack<char>();
                var invalid = false;
                foreach (var c in line)
                {
                    if (openingChars.Contains(c))
                    {
                        stack.Push(c);
                    }
                    else if (stack.Peek() == MatchFor(c))
                    {
                        stack.Pop();
                    }
                    else
                    {
                        invalid = true;
                        break;
                    }
                }

                if (!invalid)
                {
                    var currentScore = 0L;
                    while (stack.Count > 0)
                    {
                        currentScore *= 5;
                        currentScore += ScoreCompletion(MatchFor(stack.Pop()));
                    }

                    scores.Add(currentScore);
                }
            }

            var middle = scores.Count / 2;
            return scores.OrderBy(s => s).Skip(middle).First();
        }

        private static char MatchFor(char c)
        {
            return c switch
            {
                '(' => ')',
                '[' => ']',
                '{' => '}',
                '<' => '>',
                '>' => '<',
                '}' => '{',
                ']' => '[',
                ')' => '(',
                _ => throw new ArgumentOutOfRangeException(nameof(c), c, null)
            };
        }

        private static int ScoreInvalid(char closing)
        {
            return closing switch
            {
                ')' => 3,
                ']' => 57,
                '}' => 1197,
                '>' => 25137,
                _ => throw new ArgumentOutOfRangeException(nameof(closing), closing, null)
            };
        }

        private static int ScoreCompletion(char closing)
        {
            return closing switch
            {
                ')' => 1,
                ']' => 2,
                '}' => 3,
                '>' => 4,
                _ => throw new ArgumentOutOfRangeException(nameof(closing), closing, null)
            };
        }
    }
}