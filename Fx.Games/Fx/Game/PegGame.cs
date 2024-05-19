﻿namespace Fx.Game
{
    public sealed class PegGame<TPlayer> : IGame<PegGame<TPlayer>, PegBoard, PegMove, TPlayer>
    {
        private readonly TPlayer player;

        private readonly PegBoard board;

        public PegGame(TPlayer player)
            : this(player, new PegBoard(5, new[] { (0, 0) }))
        {
        }

        private PegGame(TPlayer player, PegBoard board)
        {
            this.player = player;
            this.board = board;
        }

        public TPlayer CurrentPlayer
        {
            get
            {
                return player;
            }
        }

        public IEnumerable<PegMove> Moves
        {
            get
            {
                for (int i = 0; i < board.Triangle.Length; ++i)
                {
                    for (int j = 0; j < board.Triangle[i].Length; ++j)
                    {
                        if (board.Triangle[i][j] != Peg.Empty)
                        {
                            if (i + 1 < board.Triangle.Length && i + 2 < board.Triangle.Length &&
                                board.Triangle[i + 1][j] != Peg.Empty && board.Triangle[i + 2][j] == Peg.Empty)
                            {
                                yield return new PegMove((i, j), (i + 2, j));
                            }

                            if (i + 1 < board.Triangle.Length && i + 2 < board.Triangle.Length &&
                                j + 1 < board.Triangle[i + 1].Length && j + 2 < board.Triangle[i + 2].Length &&
                                board.Triangle[i + 1][j + 1] != Peg.Empty && board.Triangle[i + 2][j + 2] == Peg.Empty)
                            {
                                yield return new PegMove((i, j), (i + 2, j + 2));
                            }

                            if (j + 1 < board.Triangle[i].Length && j + 2 < board.Triangle[i].Length &&
                                board.Triangle[i][j + 1] != Peg.Empty && board.Triangle[i][j + 2] == Peg.Empty)
                            {
                                yield return new PegMove((i, j), (i, j + 2));
                            }

                            if (j - 1 >= 0 && j - 2 >= 0 &&
                                board.Triangle[i][j - 1] != Peg.Empty && board.Triangle[i][j - 2] == Peg.Empty)
                            {
                                yield return new PegMove((i, j), (i, j - 2));
                            }

                            if (i - 1 >= 0 && i - 2 >= 0 &&
                                j < board.Triangle[i - 1].Length && j < board.Triangle[i - 2].Length &&
                                board.Triangle[i - 1][j] != Peg.Empty && board.Triangle[i - 2][j] == Peg.Empty)
                            {
                                yield return new PegMove((i, j), (i - 2, j));
                            }

                            if (i - 1 >= 0 && i - 2 >= 0 &&
                                j - 1 >= 0 && j - 2 >= 0 &&
                                board.Triangle[i - 1][j - 1] != Peg.Empty && board.Triangle[i - 2][j - 2] == Peg.Empty)
                            {
                                yield return new PegMove((i, j), (i - 2, j - 2));
                            }
                        }
                    }
                }
            }
        }

        public PegBoard Board
        {
            get
            {
                return board;
            }
        }

        public WinnersAndLosers<TPlayer> WinnersAndLosers
        {
            get
            {
                if (Moves.Any())
                {
                    return new WinnersAndLosers<TPlayer>(Enumerable.Empty<TPlayer>(), Enumerable.Empty<TPlayer>(), Enumerable.Empty<TPlayer>());
                }

                var pieces = false;
                for (int i = 0; i < board.Triangle.Length; ++i)
                {
                    for (int j = 0; j < board.Triangle[i].Length; ++j)
                    {
                        if (board.Triangle[i][j] != Peg.Empty)
                        {
                            if (pieces)
                            {
                                return new WinnersAndLosers<TPlayer>(Enumerable.Empty<TPlayer>(), new[] { player }, Enumerable.Empty<TPlayer>());
                            }
                            else
                            {
                                pieces = true;
                            }
                        }
                    }
                }

                return new WinnersAndLosers<TPlayer>(new[] { player }, Enumerable.Empty<TPlayer>(), Enumerable.Empty<TPlayer>());
            }
        }

        public bool IsGameOver
        {
            get
            {
                return !this.Moves.Any();
            }
        }

        public PegGame<TPlayer> CommitMove(PegMove move)
        {
            var blanks = new List<(int, int)>();
            for (int i = 0; i < board.Triangle.Length; ++i)
            {
                for (int j = 0; j < board.Triangle[i].Length; ++j)
                {
                    if (i == move.Start.Item1 && j == move.Start.Item2)
                    {
                        blanks.Add((i, j));
                    }
                    else if (i == move.End.Item1 && j == move.End.Item2)
                    {
                    }
                    else if (i == move.Start.Item1 + (move.End.Item1 - move.Start.Item1) / 2 && j == move.Start.Item2 + (move.End.Item2 - move.Start.Item2) / 2)
                    {
                        blanks.Add((i, j));
                    }
                    else if (board.Triangle[i][j] == Peg.Empty)
                    {
                        blanks.Add((i, j));
                    }
                }
            }

            return new PegGame<TPlayer>(player, new PegBoard(board.Triangle.Length, blanks));
        }
    }
}
