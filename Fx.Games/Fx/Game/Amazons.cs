﻿namespace Fx.Game
{
    using System;
    using System.Collections.Generic;

    using Fx.Todo;

    /// <summary>
    /// TODO doesn't work probably
    /// </summary>
    /// <threadsafety static="true" instance="true"/>
    public sealed class Amazons<TPlayer> : IGame<Amazons<TPlayer>, ChessBoard<AmazonsPiece>, ChessMove, TPlayer>
    {
        private readonly ChessBoard<AmazonsPiece> board;

        public Amazons(int size)
        {
            var pieces = new AmazonsPiece[size][];
            for (int i = 0; i < pieces.Length; ++i)
            {
                pieces[i] = new AmazonsPiece[size];
                for (int j = 0; j < pieces[i].Length; ++j)
                {
                    pieces[i][j] = AmazonsPiece.Empty;
                }
            }

            pieces[0][0] = AmazonsPiece.Player1;
            pieces[size - 1][size - 1] = AmazonsPiece.Player2;

            this.board = new ChessBoard<AmazonsPiece>(pieces);
        }

        public TPlayer CurrentPlayer
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public ChessBoard<AmazonsPiece> Board
        {
            get
            {
                return this.board;
            }
        }

        public IEnumerable<ChessMove> Moves
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public WinnersAndLosers<TPlayer> WinnersAndLosers
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsGameOver => throw new NotImplementedException();

        public Amazons<TPlayer> CommitMove(ChessMove move)
        {
            throw new NotImplementedException();
        }
    }
}
