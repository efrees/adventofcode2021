using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.Solvers
{
    internal class Day18Solver : ISolver
    {
        private const string Name = "Day 18";

        private const string InputFile = "day18input.txt";

        public void Solve()
        {
            Console.WriteLine(Name);
            var lines = Input.GetLinesFromFile(InputFile)
                .ToList();

            Console.WriteLine($"Output (part 1): {GetPart1Answer(lines)}");
            Console.WriteLine($"Output (part 2): {GetPart2Answer(lines)}");
        }

        private static long GetPart1Answer(List<string> input)
        {
            var parsedNumbers = input.Select(SmallfishNumber.Parse).ToList();

            var totalSum = parsedNumbers.Aggregate(Add);

            return totalSum.GetMagnitude();
        }

        private static long GetPart2Answer(List<string> input)
        {
            var maxSum = 0L;
            for (var i = 0; i < input.Count; i++)
            {
                for (var j = 0; j < input.Count; j++)
                {
                    if (i == j) continue;

                    var first = SmallfishNumber.Parse(input[i]);
                    var second = SmallfishNumber.Parse(input[j]);

                    var sum = Add(first, second).GetMagnitude();
                    maxSum = Math.Max(maxSum, sum);
                }
            }

            return maxSum;
        }

        private static SmallfishNumber Add(SmallfishNumber first, SmallfishNumber second)
        {
            var newNumber = new SmallfishNumber
            {
                Left = first,
                Right = second
            };
            newNumber.Reduce();
            return newNumber;
        }

        private class SmallfishNumber
        {
            private int? Literal { get; set; }
            public SmallfishNumber Left { get; set; }
            public SmallfishNumber Right { get; set; }

            public long GetMagnitude()
            {
                return Literal
                       ?? 3 * Left.GetMagnitude() + 2 * Right.GetMagnitude();
            }

            public void Reduce()
            {
                bool hasChanged;
                do
                {
                    hasChanged = ExplodeFirst(this, 0) is not null
                                 || SplitFirst(Left)
                                 || SplitFirst(Right);
                } while (hasChanged);
            }

            private SmallfishNumber ExplodeFirst(SmallfishNumber number, int depth)
            {
                if (number.Literal is not null)
                {
                    return null;
                }

                if (depth == 4)
                {
                    return number;
                }

                if (ExplodeFirst(number.Left, depth + 1) is { } pairFromLeft)
                {
                    if (depth == 3)
                    {
                        number.Left = new SmallfishNumber { Literal = 0 };
                    }

                    if (pairFromLeft is { Right: { Literal: int rightValue } })
                    {
                        AddFromLeft(number.Right, rightValue);
                        return new SmallfishNumber
                        {
                            Left = pairFromLeft.Left
                        };
                    }

                    return pairFromLeft;
                }

                if (ExplodeFirst(number.Right, depth + 1) is SmallfishNumber pairFromRight)
                {
                    if (depth == 3)
                    {
                        number.Right = new SmallfishNumber { Literal = 0 };
                    }

                    if (pairFromRight is { Left: { Literal: int leftValue } })
                    {
                        AddFromRight(number.Left, leftValue);
                        return new SmallfishNumber
                        {
                            Right = pairFromRight.Right
                        };
                    }

                    return pairFromRight;
                }

                return null;
            }

            private void AddFromLeft(SmallfishNumber number, int additionalValue)
            {
                while (number.Literal == null)
                {
                    number = number.Left;
                }

                number.Literal += additionalValue;
            }

            private void AddFromRight(SmallfishNumber number, int additionalValue)
            {
                while (number.Literal == null)
                {
                    number = number.Right;
                }

                number.Literal += additionalValue;
            }

            private bool SplitFirst(SmallfishNumber number)
            {
                if (number.Literal >= 10)
                {
                    var left = number.Literal / 2;
                    var right = number.Literal - left;
                    number.Literal = null;
                    number.Left = new SmallfishNumber { Literal = left };
                    number.Right = new SmallfishNumber { Literal = right };
                    return true;
                }

                if (number.Literal is null)
                {
                    return SplitFirst(number.Left) || SplitFirst(number.Right);
                }

                return false;
            }

            public override string ToString()
            {
                if (Literal is not null)
                {
                    return Literal.Value.ToString();
                }

                return $"[{Left},{Right}]";
            }

            public static SmallfishNumber Parse(string line)
            {
                var parseQueue = new Queue<char>(line);
                return Parse(parseQueue);
            }

            private static SmallfishNumber Parse(Queue<char> parseQueue)
            {
                while (new[] { ',', ']' }.Contains(parseQueue.Peek()))
                {
                    parseQueue.Dequeue();
                }

                var next = parseQueue.Dequeue();
                if (char.IsDigit(next))
                {
                    return new SmallfishNumber
                    {
                        Literal = next - '0'
                    };
                }

                return new SmallfishNumber
                {
                    Left = Parse(parseQueue),
                    Right = Parse(parseQueue)
                };
            }
        }
    }
}