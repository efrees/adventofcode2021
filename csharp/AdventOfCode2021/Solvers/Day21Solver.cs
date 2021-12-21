using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.Solvers
{
    internal class Day21Solver : ISolver
    {
        private const string Name = "Day 21";

        private const string InputFile = "day21input.txt";

        public void Solve()
        {
            Console.WriteLine(Name);
            var lines = Input.GetLinesFromFile(InputFile)
                .Select(ParseStartingPosition)
                .ToList();

            Console.WriteLine($"Output (part 1): {GetPart1Answer(lines)}");
            //Console.WriteLine($"Output (part 2): {GetPart2Answer(lines)}");
        }

        private int ParseStartingPosition(string line)
        {
            return int.Parse(line.Substring("Player 1 starting position:".Length));
        }

        private static int GetPart1Answer(List<int> input)
        {
            var position1 = input.First();
            var position2 = input.Last();

            var nextRoll = 100;
            var rollCount = 0;
            var score1 = 0;
            var score2 = 0;

            while (score1 < 1000 && score2 < 1000)
            {
                var totalMove = RollDeterministically(ref nextRoll);
                totalMove += RollDeterministically(ref nextRoll);
                totalMove += RollDeterministically(ref nextRoll);
                rollCount+=3;

                position1 = MovePlayer(position1, totalMove);
                score1 += position1;

                if (score1 >= 1000)
                {
                    break;
                }


                totalMove = RollDeterministically(ref nextRoll);
                totalMove += RollDeterministically(ref nextRoll);
                totalMove += RollDeterministically(ref nextRoll);
                rollCount += 3;

                position2 = MovePlayer(position2, totalMove);
                score2 += position2;
            }

            return Math.Min(score1, score2) * rollCount;
        }

        private static int RollDeterministically(ref int nextRoll)
        {
            nextRoll %= 100;
            nextRoll += 1;
            return nextRoll;
        }

        private static int MovePlayer(int currentPosition, int totalMove)
        {
            return (currentPosition + totalMove - 1) % 10 + 1;
        }
    }
}