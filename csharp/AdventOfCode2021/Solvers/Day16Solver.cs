using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2021.Solvers
{
    internal class Day16Solver : ISolver
    {
        private const string Name = "Day 16";

        private const string InputFile = "day16input.txt";

        public void Solve()
        {
            Console.WriteLine(Name);
            var input = Input.GetLinesFromFile(InputFile)
                .Select(GetBinaryStringFromHex)
                .First();

            Console.WriteLine($"Output (part 1): {GetPart1Answer(input)}");
            Console.WriteLine($"Output (part 2): {GetPart2Answer(input)}");
        }

        private static int GetPart1Answer(string input)
        {
            var cursor = 0;
            return ParsePacketAndSumVersionNumbers(input, ref cursor);
        }

        private static long GetPart2Answer(string input)
        {
            var cursor = 0;

            return ParsePacketAndComputeValue(input, ref cursor);
        }

        static string GetBinaryStringFromHex(string hexInput)
        {
            var sb = new StringBuilder(hexInput.Length * 4);
            foreach (var hex in hexInput)
            {
                sb.Append(hex switch
                {
                    '0' => "0000",
                    '1' => "0001",
                    '2' => "0010",
                    '3' => "0011",
                    '4' => "0100",
                    '5' => "0101",
                    '6' => "0110",
                    '7' => "0111",
                    '8' => "1000",
                    '9' => "1001",
                    'A' => "1010",
                    'B' => "1011",
                    'C' => "1100",
                    'D' => "1101",
                    'E' => "1110",
                    'F' => "1111",
                    _ => throw new ArgumentOutOfRangeException(nameof(hexInput), "Invalid hex input")
                });
            }

            return sb.ToString();
        }

        private static int ParsePacketAndSumVersionNumbers(string binary, ref int cursor)
        {
            var version = GetInt(binary, cursor, 3);
            var typeId = GetInt(binary, cursor + 3, 3);
            cursor += 6;

            var versionSum = version;
            if (typeId == 4)
            {
                ReadLiteral(binary, ref cursor);
            }
            else if (binary[cursor++] == '1')
            {
                var nestedCount = GetInt(binary, cursor, 11);
                cursor += 11;
                foreach (var _ in Enumerable.Range(1, nestedCount))
                {
                    versionSum += ParsePacketAndSumVersionNumbers(binary, ref cursor);
                }
            }
            else
            {
                var nestedSize = GetInt(binary, cursor, 15);
                cursor += 15;
                var endPosition = cursor + nestedSize;
                while (cursor < endPosition)
                {
                    versionSum += ParsePacketAndSumVersionNumbers(binary, ref cursor);
                }
            }

            return versionSum;
        }

        private static long ParsePacketAndComputeValue(string binary, ref int cursor)
        {
            GetInt(binary, cursor, 3);
            var typeId = GetInt(binary, cursor + 3, 3);
            cursor += 6;

            if (typeId == 4)
            {
                return ReadLiteral(binary, ref cursor);
            }

            var nestedValues = new List<long>();
            var lengthTypeIsCount = binary[cursor++] == '1';
            if (lengthTypeIsCount)
            {
                var nestedCount = GetInt(binary, cursor, 11);
                cursor += 11;

                foreach (var _ in Enumerable.Range(1, nestedCount))
                {
                    nestedValues.Add(ParsePacketAndComputeValue(binary, ref cursor));
                }
            }
            else
            {
                var nestedSize = GetInt(binary, cursor, 15);
                cursor += 15;
                var endPosition = cursor + nestedSize;
                while (cursor < endPosition)
                {
                    nestedValues.Add(ParsePacketAndComputeValue(binary, ref cursor));
                }
            }

            return typeId switch
            {
                0 => nestedValues.Sum(),
                1 => nestedValues.Aggregate((product, current) => product * current),
                2 => nestedValues.Min(),
                3 => nestedValues.Max(),
                5 => nestedValues.First() > nestedValues.Last()
                    ? 1
                    : 0,
                6 => nestedValues.First() < nestedValues.Last()
                    ? 1
                    : 0,
                7 => nestedValues.First() == nestedValues.Last()
                    ? 1
                    : 0,
                _ => throw new ArgumentOutOfRangeException(nameof(binary), "Invalid operation type")
            };
        }

        private static long ReadLiteral(string binary, ref int cursor)
        {
            var result = 0L;

            while (binary[cursor] == '1')
            {
                result *= 16;
                result += GetInt(binary, cursor + 1, 4);
                cursor += 5;
            }
            
            result *= 16;
            result += GetInt(binary, cursor + 1, 4);
            cursor += 5;

            return result;
        }

        private static int GetInt(string binary, int start, int count)
        {
            return Convert.ToInt32(binary[start..(start + count)], 2);
        }
    }
}