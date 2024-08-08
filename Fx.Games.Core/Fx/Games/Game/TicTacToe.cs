namespace Fx.Games.Game
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// An implementation of a game of tic-tac-toe
    /// </summary>
    /// <typeparam name="TPlayer">The type of the players that are playing the game</typeparam>
    public sealed class TicTacToe<TPlayer> : IGame<TicTacToe<TPlayer>, TicTacToeBoard, TicTacToeMove, TPlayer>
    {
        /// <summary>
        /// The players who are playing the game
        /// </summary>
        private readonly TPlayer[] players;

        /// <summary>
        /// The index in <see cref="players"/> of the current player
        /// </summary>
        private readonly int currentPlayerIndex;

        /// <summary>
        /// The current board state of the game
        /// </summary>
        private readonly TicTacToeBoard board;

        /// <summary>
        /// Initializes a new instance of the <see cref="TicTacToe{TPlayer}"/> class
        /// </summary>
        /// <param name="exes">The player who is playing the X's</param>
        /// <param name="ohs">The player who is playing the O's</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="exes"/> or <paramref name="ohs"/> is <see langword="null"/></exception>
        public TicTacToe(TPlayer exes, TPlayer ohs)
            : this(new[] { exes ?? throw new ArgumentNullException(nameof(exes)), ohs ?? throw new ArgumentNullException(nameof(ohs)) }, 0, new TicTacToeBoard())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TicTacToe{TPlayer}"/> class
        /// </summary>
        /// <param name="players">The players who are playing the game</param>
        /// <param name="currentPlayerIndex">The index in <see cref="players"/> of the current player</param>
        /// <param name="newBoard">The current board state of the game</param>
        private TicTacToe(TPlayer[] players, int currentPlayerIndex, TicTacToeBoard newBoard)
        {
            this.players = players;
            this.currentPlayerIndex = currentPlayerIndex;
            this.board = newBoard;
        }

        /// <inheritdoc/>
        public TPlayer CurrentPlayer
        {
            get
            {
                return this.players[this.currentPlayerIndex];
            }
        }

        /// <inheritdoc/>
        public IEnumerable<TicTacToeMove> Moves
        {
            get
            {
                for (uint i = 0; i < 3; ++i)
                {
                    for (uint j = 0; j < 3; ++j)
                    {
                        if (this.board.Grid[i, j] == TicTacToePiece.Empty)
                        {
                            yield return new TicTacToeMove(i, j);
                        }
                    }
                }
            }
        }

        /// <inheritdoc/>
        public TicTacToeBoard Board
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
                var grid = this.board.Grid;
                for (int i = 0; i < 3; ++i)
                {
                    var currentPiece = grid[i, 0];
                    if (currentPiece != TicTacToePiece.Empty)
                    {
                        bool win = true;
                        for (int j = 1; j < 3; ++j)
                        {
                            win = win && currentPiece == grid[i, j];
                        }

                        if (win)
                        {
                            return new WinnersAndLosers<TPlayer>(
                                new[] { GetPlayerFromPiece(currentPiece) }, 
                                new[] { GetPlayerFromPiece(currentPiece == TicTacToePiece.Ex ? TicTacToePiece.Oh : TicTacToePiece.Ex) }, 
                                Enumerable.Empty<TPlayer>());
                        }
                    }
                }

                for (int j = 0; j < 3; ++j)
                {
                    var currentPiece = grid[0, j];
                    if (currentPiece != TicTacToePiece.Empty)
                    {
                        bool win = true;
                        for (int i = 1; i < 3; ++i)
                        {
                            win = win && currentPiece == grid[i, j];
                        }

                        if (win)
                        {
                            return new WinnersAndLosers<TPlayer>(
                                new[] { GetPlayerFromPiece(currentPiece) },
                                new[] { GetPlayerFromPiece(currentPiece == TicTacToePiece.Ex ? TicTacToePiece.Oh : TicTacToePiece.Ex) },
                                Enumerable.Empty<TPlayer>());
                        }
                    }
                }

                var topLeft = grid[0, 0];
                if (topLeft != TicTacToePiece.Empty && topLeft == grid[1, 1] && topLeft == grid[2, 2])
                {
                    return new WinnersAndLosers<TPlayer>(
                        new[] { GetPlayerFromPiece(topLeft) },
                        new[] { GetPlayerFromPiece(topLeft == TicTacToePiece.Ex ? TicTacToePiece.Oh : TicTacToePiece.Ex) },
                        Enumerable.Empty<TPlayer>());
                }

                var bottomLeft = grid[2, 0];
                if (bottomLeft != TicTacToePiece.Empty && bottomLeft == grid[1, 1] && bottomLeft == grid[0, 2])
                {
                    return new WinnersAndLosers<TPlayer>(
                        new[] { GetPlayerFromPiece(bottomLeft) },
                        new[] { GetPlayerFromPiece(bottomLeft == TicTacToePiece.Ex ? TicTacToePiece.Oh : TicTacToePiece.Ex) },
                        Enumerable.Empty<TPlayer>());
                }

                foreach (var piece in grid)
                {
                    if (piece == TicTacToePiece.Empty)
                    {
                        // no one has won the game, and there are empty spaces, so the game is continuing
                        return new WinnersAndLosers<TPlayer>(Enumerable.Empty<TPlayer>(), Enumerable.Empty<TPlayer>(), Enumerable.Empty<TPlayer>());
                    }
                }

                // no one has won the game, and there are no empty spaces, so the game is a draw
                return new WinnersAndLosers<TPlayer>(
                    Enumerable.Empty<TPlayer>(),
                    Enumerable.Empty<TPlayer>(),
                    this.players);
            }
        }

        /// <inheritdoc/>
        public bool IsGameOver
        {
            get
            {
                return this.WinnersAndLosers.Winners.Any() || this.WinnersAndLosers.Losers.Any() || this.WinnersAndLosers.Drawers.Any();
            }
        }

        /// <inheritdoc/>
        public TicTacToe<TPlayer> CommitMove(TicTacToeMove move)
        {
            if (move == null)
            {
                throw new ArgumentNullException(nameof(move));
            }

            var grid = this.board.Grid;
            if (grid[move.Row, move.Column] != TicTacToePiece.Empty)
            {
                throw new IllegalMoveExeption($"The move to place a piece at ({move.Row},{move.Column}) is illegal because {grid[move.Row, move.Column]} already occupies that space");
            }

            var newBoard = this.board.Grid;
            newBoard[move.Row, move.Column] = (TicTacToePiece)(this.currentPlayerIndex + 1);

            return new TicTacToe<TPlayer>(this.players, (this.currentPlayerIndex + 1) % 2, new TicTacToeBoard(newBoard));
        }

        /// <summary>
        /// Gets the <typeparamref name="TPlayer"/> corresponding to the <paramref name="piece"/>
        /// </summary>
        /// <param name="piece">The piece to find the player of</param>
        /// <returns>The player who is playing <paramref name="piece"/></returns>
        private TPlayer GetPlayerFromPiece(TicTacToePiece piece)
        {
            return this.players[(int)piece - 1];
        }
    }
}
