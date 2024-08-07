namespace Fx.Games.Game
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// An implementation of the peg game: http://www.thepeggame.com/
    /// </summary>
    /// <typeparam name="TPlayer">The type of the player that is playing the game</typeparam>
    public sealed class PegGame<TPlayer> : IGame<PegGame<TPlayer>, PegBoard, PegMove, TPlayer>
    {
        /// <summary>
        /// The current player of the game
        /// </summary>
        private readonly TPlayer player;

        /// <summary>
        /// The current board state of the game
        /// </summary>
        private readonly PegBoard board;

        /// <summary>
        /// Initializes a new instance of the <see cref="PegGame{TPlayer}"/> class
        /// </summary>
        /// <param name="player">The player of the game</param>
        public PegGame(TPlayer player)
            : this(player, new PegBoard(new[] { new PegPosition(0, 0) }))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PegGame{TPlayer}"/> class
        /// </summary>
        /// <param name="player">The player of the game</param>
        /// <param name="board">The board state that the game should have</param>
        private PegGame(TPlayer player, PegBoard board)
        {
            this.player = player;
            this.board = board;
        }

        /// <inheritdoc/>
        public TPlayer CurrentPlayer
        {
            get
            {
                return this.player;
            }
        }

        /// <inheritdoc/>
        public IEnumerable<PegMove> Moves
        {
            get
            {
                var triangle = this.board.Triangle;

                for (int i = 0; i < triangle.Length; ++i)
                {
                    for (int j = 0; j < triangle[i].Length; ++j)
                    {
                        if (triangle[i][j] != Peg.Empty)
                        {
                            if (i + 1 < triangle.Length && i + 2 < triangle.Length &&
                                triangle[i + 1][j] != Peg.Empty && triangle[i + 2][j] == Peg.Empty)
                            {
                                yield return new PegMove(new PegPosition(i, j), new PegPosition(i + 2, j));
                            }

                            if (i + 1 < triangle.Length && i + 2 < triangle.Length &&
                                j + 1 < triangle[i + 1].Length && j + 2 < triangle[i + 2].Length &&
                                triangle[i + 1][j + 1] != Peg.Empty && triangle[i + 2][j + 2] == Peg.Empty)
                            {
                                yield return new PegMove(new PegPosition(i, j), new PegPosition(i + 2, j + 2));
                            }

                            if (j + 1 < triangle[i].Length && j + 2 < triangle[i].Length &&
                                triangle[i][j + 1] != Peg.Empty && triangle[i][j + 2] == Peg.Empty)
                            {
                                yield return new PegMove(new PegPosition(i, j), new PegPosition(i, j + 2));
                            }

                            if (j - 1 >= 0 && j - 2 >= 0 &&
                                triangle[i][j - 1] != Peg.Empty && triangle[i][j - 2] == Peg.Empty)
                            {
                                yield return new PegMove(new PegPosition(i, j), new PegPosition(i, j - 2));
                            }

                            if (i - 1 >= 0 && i - 2 >= 0 &&
                                j < triangle[i - 1].Length && j < triangle[i - 2].Length &&
                                triangle[i - 1][j] != Peg.Empty && triangle[i - 2][j] == Peg.Empty)
                            {
                                yield return new PegMove(new PegPosition(i, j), new PegPosition(i - 2, j));
                            }

                            if (i - 1 >= 0 && i - 2 >= 0 &&
                                j - 1 >= 0 && j - 2 >= 0 &&
                                triangle[i - 1][j - 1] != Peg.Empty && triangle[i - 2][j - 2] == Peg.Empty)
                            {
                                yield return new PegMove(new PegPosition(i, j), new PegPosition(i - 2, j - 2));
                            }
                        }
                    }
                }
            }
        }

        /// <inheritdoc/>
        public PegBoard Board
        {
            get
            {
                return this.board;
            }
        }

        /// <inheritdoc/>
        public WinnersAndLosers<TPlayer> WinnersAndLosers
        {
            get
            {
                if (this.Moves.Any())
                {
                    return new WinnersAndLosers<TPlayer>(Enumerable.Empty<TPlayer>(), Enumerable.Empty<TPlayer>(), Enumerable.Empty<TPlayer>());
                }

                var triangle = this.board.Triangle;

                var pieces = false;
                for (int i = 0; i < triangle.Length; ++i)
                {
                    for (int j = 0; j < triangle[i].Length; ++j)
                    {
                        if (triangle[i][j] != Peg.Empty)
                        {
                            if (pieces)
                            {
                                return new WinnersAndLosers<TPlayer>(Enumerable.Empty<TPlayer>(), new[] { this.player }, Enumerable.Empty<TPlayer>());
                            }
                            else
                            {
                                pieces = true;
                            }
                        }
                    }
                }

                return new WinnersAndLosers<TPlayer>(new[] { this.player }, Enumerable.Empty<TPlayer>(), Enumerable.Empty<TPlayer>());
            }
        }

        /// <inheritdoc/>
        public bool IsGameOver
        {
            get
            {
                return !this.Moves.Any();
            }
        }

        /// <inheritdoc/>
        public PegGame<TPlayer> CommitMove(PegMove move)
        {
            var triange = this.board.Triangle;

            var blanks = new List<PegPosition>();
            for (int i = 0; i < triange.Length; ++i)
            {
                for (int j = 0; j < triange[i].Length; ++j)
                {
                    if (i == move.Start.Row && j == move.Start.Column)
                    {
                        blanks.Add(new PegPosition(i, j));
                    }
                    else if (i == move.End.Row && j == move.End.Column)
                    {
                    }
                    else if (i == move.Start.Row + (move.End.Row - move.Start.Row) / 2 && j == move.Start.Column + (move.End.Column - move.Start.Column) / 2)
                    {
                        blanks.Add(new PegPosition(i, j));
                    }
                    else if (triange[i][j] == Peg.Empty)
                    {
                        blanks.Add(new PegPosition(i, j));
                    }
                }
            }

            return new PegGame<TPlayer>(this.player, new PegBoard(blanks));
        }
    }
}
