using System.Linq;

namespace Fx.Games.Game
{
    using Fx.Games.Displayer;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text.Json.Serialization;

    public sealed class ConnectFourDisplayer<TPlayer> : IDisplayer<ConnectFour<TPlayer>, ConnectFourBoard, ConnectFourMove, TPlayer>
    {
        private readonly Func<TPlayer, string> playerToString;

        public ConnectFourDisplayer(Func<TPlayer, string> playerToString)
        {
            if (playerToString == null)
            {
                throw new ArgumentNullException(nameof(playerToString));
            }

            this.playerToString = playerToString;
        }

        public void DisplayAvailableMoves(ConnectFour<TPlayer> game)
        {
            Console.WriteLine("Select a move (column):");
            int i = 0;
            foreach (var move in game.Moves)
            {
                Console.WriteLine($"{i++}: ({move.Column})");
            }

            Console.WriteLine();
        }

        public void DisplayBoard(ConnectFour<TPlayer> game)
        {
            for (int j = 5; j >= 0; --j)
            {
                for (int i = 0; i < 7; ++i)
                {
                    Console.Write($" {FromPiece(game.Board.Stacks[i].Get(j))} ");
                }

                Console.WriteLine();
            }
        }

        public void DisplayOutcome(ConnectFour<TPlayer> game)
        {
            try
            {
                var winner = game.WinnersAndLosers.Winners.First();
                Console.WriteLine($"{playerToString(winner)} wins!");
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("The game was a draw...");
            }
        }

        public void DisplaySelectedMove(ConnectFourMove move)
        {
            Console.WriteLine($"{new string(' ', move.Column * 3)}({move.Column})");
        }

        private static char FromPiece(ConnectFourBoardSpace piece)
        {
            switch (piece)
            {
                case ConnectFourBoardSpace.Empty:
                    return '.';
                case ConnectFourBoardSpace.Red:
                    return 'X';
                case ConnectFourBoardSpace.Yellow:
                    return 'O';
            }

            throw new InvalidOperationException("TODO");
        }
    }

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
                //// TODO refactor the whole stack thing bcaeuse it currently doesn'ty provide benefits over just using a cloning lists in the calling code
                var newSpaces = this.spaces.ToList();
                newSpaces.Add(space);

                return new ConnectFourStack(newSpaces);
            }

            public ConnectFourBoardSpace Get(int index)
            {
                return index >= this.spaces.Count ? ConnectFourBoardSpace.Empty : this.spaces[index];
            }
        }
    }

    public enum ConnectFourBoardSpace
    {
        Empty = 0,
        Red = 1,
        Yellow = 2,
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

        private readonly TPlayer yellowPlayer;

        private readonly TPlayer opponent;

        public ConnectFour(TPlayer player1, TPlayer player2)
            : this(player1, player2, new ConnectFourBoard(), player1, player2)
        {
            //// TODO what about tplayer red, tplayer yellow, and tplayer first
        }

        private ConnectFour(TPlayer player1, TPlayer player2, ConnectFourBoard board, TPlayer redPlayer, TPlayer yellowPlayer)
        {
            this.CurrentPlayer = player1;
            this.opponent = player2;
            this.Board = board;
            this.redPlayer = redPlayer;
            this.yellowPlayer = yellowPlayer;
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
                    .Where(tuple => tuple.Item1)
                    .Select(tuple => new ConnectFourMove(tuple.index));
            }
        }

        private bool FourOfTheSame((int X, int Y) ix0, (int X, int Y) ix1, (int X, int Y) ix2, (int X, int Y) ix3, [MaybeNullWhen(false)] out WinnersAndLosers<TPlayer> wnl)
        {
            var currentPiece = this.Board.Stacks[ix0.X].Get(ix0.Y);
            if (currentPiece == ConnectFourBoardSpace.Empty)
            {
                wnl = default;
                return false;
            }

            if (currentPiece == this.Board.Stacks[ix1.X].Get(ix1.Y) &&
                currentPiece == this.Board.Stacks[ix2.X].Get(ix2.Y) &&
                currentPiece == this.Board.Stacks[ix3.X].Get(ix3.Y))
            {
                var winner = currentPiece == ConnectFourBoardSpace.Red ? this.redPlayer : this.yellowPlayer;
                var loser = currentPiece == ConnectFourBoardSpace.Red ? this.yellowPlayer : this.redPlayer;

                wnl = new WinnersAndLosers<TPlayer>(
                    new[] { winner },
                    new[] { loser },
                    Enumerable.Empty<TPlayer>());

                return true;
            }
            wnl = default;
            return false;
        }


        public WinnersAndLosers<TPlayer> WinnersAndLosers
        {
            get
            {
                // check if there are vertical wins
                for (int i = 0; i < 7; ++i)
                {
                    for (int j = 0; j < 3; ++j)
                    {
                        if (FourOfTheSame((i, j), (i, j + 1), (i, j + 2), (i, j + 3), out var wnl))
                        {
                            return wnl;
                        }
                    }
                }

                // check if there are horizontal wins
                for (int i = 0; i < 4; ++i)
                {
                    for (int j = 0; j < 6; ++j)
                    {
                        if (FourOfTheSame((i, j), (i + 1, j), (i + 2, j), (i + 3, j), out var wnl))
                        {
                            return wnl;
                        }
                    }
                }

                // check if there are up and right diagonal wins
                for (int i = 0; i < 4; ++i)
                {
                    for (int j = 0; j < 3; ++j)
                    {
                        if (FourOfTheSame((i, j), (i + 1, j + 1), (i + 2, j + 2), (i + 3, j + 3), out var wnl))
                        {
                            return wnl;
                        }
                    }
                }

                // check if there are down and right diagonal wins
                for (int i = 0; i < 4; ++i)
                {
                    for (int j = 5; j >= 3; --j)
                    {
                        if (FourOfTheSame((i, j), (i + 1, j - 1), (i + 2, j - 2), (i + 3, j - 3), out var wnl))
                        {
                            return wnl;
                        }
                    }
                }

                if (!this.Moves.Any())
                {
                    return new WinnersAndLosers<TPlayer>(Enumerable.Empty<TPlayer>(), Enumerable.Empty<TPlayer>(), new[] { this.CurrentPlayer, this.opponent });
                }

                return new WinnersAndLosers<TPlayer>(Enumerable.Empty<TPlayer>(), Enumerable.Empty<TPlayer>(), Enumerable.Empty<TPlayer>());
            }
        }

        public bool IsGameOver
        {
            get
            {
                var winnersAndLoser = this.WinnersAndLosers;
                return winnersAndLoser.Winners.Any() || winnersAndLoser.Losers.Any() || winnersAndLoser.Drawers.Any();
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
                                //// TODO this probably has issuies if boxing happens on tplayer
                                return stack.Drop(object.ReferenceEquals(this.CurrentPlayer, this.redPlayer) ? ConnectFourBoardSpace.Red : ConnectFourBoardSpace.Yellow);
                            }
                            else
                            {
                                return stack;
                            }
                        })
                        .ToArray()),
                this.redPlayer,
                this.yellowPlayer);
        }
    }
}
