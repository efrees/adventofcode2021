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

            return PlayBingo(sequence, boards);
        }

        private static long GetPart2Answer(List<string> lines)
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

            return PlayBingo2(sequence, boards);
        }

        private static int PlayBingo(List<int> sequence, List<BingoBoard> boards)
        {
            foreach (var nextCalled in sequence)
            {
                foreach (var board in boards)
                {
                    board.Mark(nextCalled);
                    if (board.IsWinner)
                    {
                        return board.Score() * nextCalled;
                    }
                }
            }

            return -1;
        }

        private static int PlayBingo2(List<int> sequence, List<BingoBoard> boards)
        {
            foreach (var nextCalled in sequence)
            {
                foreach (var board in boards)
                {
                    board.Mark(nextCalled);
                    if (boards.All(b => b.IsWinner))
                    {
                        return board.Score() * nextCalled;
                    }
                }
            }

            return -1;
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
                            } else
                            {
                                
                            }
                        }
                    }
                }
            }

            public int Score()
            {
                return Board.Sum(row => row.Where(x => x != -1).Sum());
            }
        }
    }
}