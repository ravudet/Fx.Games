﻿namespace Fx.Games.Game
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class ConnectFourBoard
    {
        // 7 wide and 6 deep

        private readonly ConnectFourStack[] stacks;

        public ConnectFourBoard()
            : this(Enumerable.Range(0, 7).Select(_ => new ConnectFourStack()).ToArray())
        {
        }

        public ConnectFourBoard(ConnectFourStack[] stacks)
        {
            this.stacks = (stacks.Clone() as ConnectFourStack[])!; //// TODO
        }

        public IReadOnlyList<ConnectFourStack> Stacks
        {
            get
            {
                return this.stacks; //// TODO have an adapter type
            }
        }

        public sealed class ConnectFourStack
        {
            private readonly List<ConnectFourBoardSpace> spaces;

            public ConnectFourStack()
                : this(new List<ConnectFourBoardSpace>())
            {
                //// TODO hide this constrcutor
            }

            public ConnectFourStack(List<ConnectFourBoardSpace> spaces)
            {
                //// TODO hide this constructor
                this.spaces = spaces.ToList();
            }

            public bool IsFull
            {
                get
                {
                    return this.spaces.Count >= 6;
                }
            }

            public ConnectFourStack Drop(ConnectFourBoardSpace space)
            {
                var newSpaces = this.spaces.ToList();
                newSpaces.Add(space);

                return new ConnectFourStack(newSpaces);
            }
        }
    }

    public enum ConnectFourBoardSpace
    {
        Red = 0,
        Yellow = 1,
    }

    public sealed class ConnectFourMove
    {
        public ConnectFourMove(int column)
        {
            if (column >= 7)
            {
                throw new ArgumentOutOfRangeException(nameof(column), $"The clumn must be gbetwen 0 and 6 inclusive");
            }

            this.Column = column;
        }

        public int Column { get; }
    }

    public sealed class ConnectFour<TPlayer> : IGame<ConnectFour<TPlayer>, ConnectFourBoard, ConnectFourMove, TPlayer>
    {
        private readonly TPlayer redPlayer;

        private readonly TPlayer opponent;

        public ConnectFour(TPlayer player1, TPlayer player2)
            : this(player1, player2, new ConnectFourBoard(), player1)
        {
        }

        private ConnectFour(TPlayer player1, TPlayer player2, ConnectFourBoard board, TPlayer redPlayer)
        {
            this.CurrentPlayer = player1;
            this.opponent = player2;
            this.Board = board;
            this.redPlayer = redPlayer;
        }

        public TPlayer CurrentPlayer { get; }

        public ConnectFourBoard Board { get; }

        public IEnumerable<ConnectFourMove> Moves
        {
            get
            {
                return this
                    .Board
                    .Stacks
                    .Select((stack, index) => (!stack.IsFull, index))
                    .Where(tuple => !tuple.Item1)
                    .Select(tuple => new ConnectFourMove(tuple.index));
            }
        }

        public WinnersAndLosers<TPlayer> WinnersAndLosers
        {
            get
            {
                throw new Exception("TODO xtofs apparently has the intelligence to do this right but i dont");
            }
        }

        public bool IsGameOver
        {
            get
            {
                return this.WinnersAndLosers.Winners.Any() || !this.Moves.Any();
            }
        }

        public ConnectFour<TPlayer> CommitMove(ConnectFourMove move)
        {
            var stack = this.Board.Stacks[move.Column];
            if (stack.IsFull)
            {
                throw new IllegalMoveExeption("tODO");
            }

            return new ConnectFour<TPlayer>(
                this.opponent,
                this.CurrentPlayer,
                new ConnectFourBoard(
                    this
                        .Board
                        .Stacks
                        .Select((stack, index) =>
                        {
                            if (index == move.Column)
                            {
                                return stack.Drop(object.ReferenceEquals(this.CurrentPlayer, this.redPlayer) ? ConnectFourBoardSpace.Red : ConnectFourBoardSpace.Yellow);
                            }
                            else
                            {
                                return stack;
                            }
                        })
                        .ToArray()),
                this.redPlayer);
        }
    }
}
