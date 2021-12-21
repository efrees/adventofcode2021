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
            Console.WriteLine($"Output (part 2): {GetPart2Answer(lines)}");
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
                rollCount += 3;

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

        private static long GetPart2Answer(List<int> input)
        {
            var position1 = input.First();
            var position2 = input.Last();
            var score1 = 0;
            var score2 = 0;

            var totalRollOccurrences = GetTotalRollDistribution();
            return CountPlayerOneWins(position1, position2, score1, score2, totalRollOccurrences);
        }

        private static long CountPlayerOneWins(int position1,
            int position2,
            int score1,
            int score2,
            Dictionary<int, int> totalRollOccurrences)
        {
            if (score1 >= 21)
            {
                return 1;
            }

            if (score2 >= 21)
            {
                return 0;
            }

            var allWins = 0L;
            foreach (var (totalRoll1, weight1) in totalRollOccurrences)
            {
                var nextPosition1 = MovePlayer(position1, totalRoll1);
                var nextScore1 = score1 + nextPosition1;

                if (nextScore1 >= 21)
                {
                    allWins += weight1;
                    continue;
                }

                foreach (var (totalRoll2, weight2) in totalRollOccurrences)
                {
                    var nextPosition2 = MovePlayer(position2, totalRoll2);

                    var nestedWins = CountPlayerOneWins(nextPosition1, nextPosition2, nextScore1, score2 + nextPosition2, totalRollOccurrences);

                    allWins += nestedWins * weight2 * weight1;
                }
            }

            return allWins;
        }

        private static Dictionary<int, int> GetTotalRollDistribution()
        {
            var totalRollOccurrences = new Dictionary<int, int>();
            var possibleRolls = new[] { 1, 2, 3 };
            var allRollCombinations =
                from x in possibleRolls
                from y in possibleRolls
                from z in possibleRolls
                select x + y + z;

            foreach (var totalRoll in allRollCombinations)
            {
                totalRollOccurrences[totalRoll] = totalRollOccurrences.GetValueOrDefault(totalRoll, 0) + 1;
            }

            return totalRollOccurrences;
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