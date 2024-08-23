
using Fx.Games.Game.Amazons;

namespace Fx.Games.Game.Amazons
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Threading;

    public sealed class GameOfAmazons<TPlayer> : IGame<GameOfAmazons<TPlayer>, Board, Move, TPlayer>
        where TPlayer : notnull
    {
        private readonly TPlayer opposingPlayer;
        private readonly Dictionary<TPlayer, SquareState> playerPiece;
        private readonly TPlayer currentPlayer;

        public GameOfAmazons(TPlayer white, TPlayer black)
        {
            currentPlayer = white;
            opposingPlayer = black;
            playerPiece = new Dictionary<TPlayer, SquareState>() // TODO: since this uses EqualityComparer<TPlayer>.Default), check if this works for all TPlayer types
            {
                { white, SquareState.White },
                { black, SquareState.Black }
            };

            Board = new Board();

            // no winners or losers yet
            WinnersAndLosers = new WinnersAndLosers<TPlayer>(Array.Empty<TPlayer>(), Array.Empty<TPlayer>(), Array.Empty<TPlayer>());
        }


        private GameOfAmazons(TPlayer white, TPlayer black, Board board, WinnersAndLosers<TPlayer> winnersAndLosers, Dictionary<TPlayer, SquareState> pieceLookup)
        {
            currentPlayer = white;
            opposingPlayer = black;
            Board = board;
            WinnersAndLosers = winnersAndLosers;
            this.playerPiece = pieceLookup;
        }

        public TPlayer CurrentPlayer => currentPlayer;

        public TPlayer? OpposingPlayer => opposingPlayer;

        public IReadOnlyDictionary<TPlayer, SquareState> PlayerPiece => playerPiece;

        public IEnumerable<Move> Moves
        {
            get
            {
                var playerPiece = this.playerPiece[currentPlayer];
                // var opponentPiece = this.playerPiece[opposingPlayer];

                foreach (var from in GetAmazons(playerPiece))
                {
                    // remove the piece on <From> from the board to allow the arrow to be placed there
                    var original = this.Board[from];
                    this.Board[from] = SquareState.Empty;

                    try
                    {
                        foreach (var to in MovesFrom(Board, from))
                        {
                            foreach (var arrow in MovesFrom(Board, to))
                            {
                                yield return new Move(from, to, arrow);
                            }
                        }
                    }
                    finally
                    {
                        this.Board[from] = original;
                    }
                }
            }
        }


        #region move generation
        private IEnumerable<(int X, int Y)> GetAmazons(SquareState amazon)
        {
            var (width, height) = Board.Size;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (Board[i, j] == amazon)
                    {
                        yield return (i, j);
                    }
                }
            }
        }

        private IEnumerable<(int X, int Y)> MovesFrom(Board board, (int X, int Y) from)
        {
            var (width, height) = Board.Size;
            foreach (var dir in Direction.All)
            {
                var (dx, dy) = dir;
                var (x, y) = from;
                while (true)
                {
                    x += dx;
                    y += dy;
                    if (x < 0 || x >= width || y < 0 || y >= height)
                    {
                        break; // stop moving in that direction if we hit the edge of the board
                    }

                    if (Board[x, y] == SquareState.Empty)
                    {
                        yield return (x, y);
                    }
                    else
                    {
                        break; // stop moving in that direction if we hit a piece
                    }
                }
            }
        }



        #endregion

        public Board Board { get; }

        public WinnersAndLosers<TPlayer> WinnersAndLosers { get; }

        /// Gets a value indicating whether the game is over.
        /// </summary>
        /// <remarks>
        /// The game is considered over if there no more moves available.
        /// </remarks>
        public bool IsGameOver => !Moves.Any();

        public GameOfAmazons<TPlayer> CommitMove(Move move)
        {
            var board = Board.Clone();

            // swap the piece from the From square with the piece on the To square            
            (board[move.From], board[move.To]) = (board[move.To], board[move.From]);
            // place arrow on the board
            board[move.Arrow] = SquareState.Arrow;

            // TODO: determine winners and loosers
            var winnersAndLosers = Game.WinnersAndLosers.None(currentPlayer, opposingPlayer);

            return new GameOfAmazons<TPlayer>(opposingPlayer, currentPlayer, board, winnersAndLosers, playerPiece);
        }
    }
}

