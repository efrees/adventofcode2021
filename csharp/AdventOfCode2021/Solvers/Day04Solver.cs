using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.Solvers
{
    internal class Day04Solver : ISolver
    {
        private const string Name = "Day 4";

        private const string InputFile = "day04input.txt";

        public void Solve()
        {
            Console.WriteLine(Name);
            var lines = Input.GetLinesFromFile(InputFile).ToList();

            Console.WriteLine($"Output (part 1): {GetPart1Answer(lines)}");
            Console.WriteLine($"Output (part 2): {GetPart2Answer(lines)}");
        }

        private static long GetPart1Answer(List<string> lines)
        {
            var (sequence, boards) = ParseBoards(lines);

            return GetWinningScoreSequence(sequence, boards).First();
        }

        private static long GetPart2Answer(List<string> lines)
        {
            var (sequence, boards) = ParseBoards(lines);

            return GetWinningScoreSequence(sequence, boards).Last();
        }

        private static (List<int> sequence, List<BingoBoard> boards) ParseBoards(List<string> lines)
        {
            var sequence = lines.First().Split(',').Select(int.Parse).ToList();

            var boards = new List<BingoBoard>();

            var currentBoard = new BingoBoard();
            foreach (var line in lines.Skip(2))
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    boards.Add(currentBoard);
                    currentBoard = new BingoBoard();
                    continue;
                }

                var row = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
                currentBoard.Board.Add(row);
            }

            return (sequence, boards);
        }

        private static IEnumerable<int> GetWinningScoreSequence(List<int> sequence, List<BingoBoard> boards)
        {
            var alreadyWon = new HashSet<BingoBoard>();
            foreach (var nextCalled in sequence)
            {
                foreach (var board in boards.Except(alreadyWon))
                {
                    board.Mark(nextCalled);
                    if (board.IsWinner)
                    {
                        alreadyWon.Add(board);
                        yield return board.GetScore() * nextCalled;
                    }
                }
            }
        }

        private class BingoBoard
        {
            public IList<IList<int>> Board { get; set; } = new List<IList<int>>();
            public bool IsWinner { get; set; }

            public void Mark(int called)
            {
                for (var j = 0; j < Board.Count; j++)
                {
                    var row = Board[j];
                    for (var i = 0; i < row.Count; i++)
                    {
                        if (row[i] == called)
                        {
                            row[i] = -1;

                            if (row.All(x => x == -1)
                                || Board.All(r => r[i] == -1))
                            {
                                IsWinner = true;
                            }

                            return;
                        }
                    }
                }
            }

            public int GetScore()
            {
                return Board.Sum(row => row.Where(x => x != -1).Sum());
            }
        }
    }
}