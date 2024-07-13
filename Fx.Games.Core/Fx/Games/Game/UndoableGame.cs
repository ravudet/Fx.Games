namespace Fx.Games.Game
{
    using System.Collections.Generic;
    using System.Linq;

    public abstract class UndoableMove<TMove>
    {
        private UndoableMove()
        {
        }

        public sealed class Undo : UndoableMove<TMove>
        {
            private Undo()
            {
            }

            public static Undo Instance { get; } = new Undo();
        }

        public sealed class Move : UndoableMove<TMove>
        {
            public Move(TMove value)
            {
                this.Value = value;
            }

            public TMove Value { get; }
        }
    }

    public static class UndoableMove
    {
        public static UndoableMove<TMove> Undo<TMove>()
        {
            return UndoableMove<TMove>.Undo.Instance;
        }

        public static UndoableMove<TMove> Move<TMove>(TMove value)
        {
            return new UndoableMove<TMove>.Move(value);
        }
    }

    /// <summary>
    /// TODO is this a good idea? look into getting the displayer right; maybe the driver should be responsible for undoable games; besides, is "undo" really a "move"?
    /// </summary>
    /// <typeparam name="TGame"></typeparam>
    /// <typeparam name="TBoard"></typeparam>
    /// <typeparam name="TMove"></typeparam>
    /// <typeparam name="TPlayer"></typeparam>
    public sealed class UndoableGame<TGame, TBoard, TMove, TPlayer> : IGame<UndoableGame<TGame, TBoard, TMove, TPlayer>, TBoard, UndoableMove<TMove>, TPlayer> where TGame : IGame<TGame, TBoard, TMove, TPlayer>
    {
        private readonly UndoableGame<TGame, TBoard, TMove, TPlayer>? previousGame;

        public UndoableGame(TGame game)
            : this(game, default)
        {
        }

        private UndoableGame(TGame game, UndoableGame<TGame, TBoard, TMove, TPlayer>? previousGame)
        {
            this.Game = game;
            this.previousGame = previousGame;
        }

        public TGame Game { get; }

        public TPlayer CurrentPlayer
        {
            get
            {
                return this.Game.CurrentPlayer;
            }
        }

        public IEnumerable<UndoableMove<TMove>> Moves
        {
            get
            {
                var moves = this.Game
                    .Moves
                    .Select(move => UndoableMove.Move(move));
                if (this.previousGame != null)
                {
                    moves = moves.Append(UndoableMove.Undo<TMove>());
                }

                return moves;
            }
        }

        public TBoard Board
        {
            get
            {
                return this.Game.Board;
            }
        }

        public WinnersAndLosers<TPlayer> WinnersAndLosers
        {
            get
            {
                return this.Game.WinnersAndLosers;
            }
        }

        public bool IsGameOver
        {
            get
            {
                return this.Game.IsGameOver;
            }
        }

        public UndoableGame<TGame, TBoard, TMove, TPlayer> CommitMove(UndoableMove<TMove> move)
        {
            if (move is UndoableMove<TMove>.Move gameMove)
            {
                return new UndoableGame<TGame, TBoard, TMove, TPlayer>(this.Game.CommitMove(gameMove.Value), this);
            }
            else
            {
                if (this.previousGame == null)
                {
                    throw new IllegalMoveExeption($"Undo cannot be performed on the first state of the game");
                }

                return this.previousGame;
            }
        }
    }
}
