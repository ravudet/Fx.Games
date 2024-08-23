// TODOs
//
// integrate the v2/qlearning branch
// add monty carlo back
// productize tictactoeconsoledisplayer
//
// write a version of game of amazons
// write perf tests for game of amazons
//
// should peg position use uint instead of int?
// you're not actually testing the peggameconsoledisplayer, you're just running it
// null checks in peggameconsoledisplayer
// implement a read only list for peggameutilities.winningsequence
// what are the correct db namespaces?
// does it make sense to use structs for the dbadapters? will boxing end up expensive? you can have a default constructor in c# now, so you probably should just use structs
// what should db ienumerable + ienumerator actually look like
// is there a way to leverage the underling struct enumerators in the enumerable and enumerator dbadapters?
// is it worth it for drvier to require a keyeddictionary just so that it can make a copy of the strategies?
// add back any games or strategies from the initial commit
// productize program class

namespace ConsoleApplication1
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Numerics;
    using Db.System.Collections.Generic;
    using DbAdapters.System.Collections.Generic;
    using Fx.Games.CueLearning;
    using Fx.Games.Displayer;
    using Fx.Games.Driver;
    using Fx.Games.Game;
    using Amazons = Fx.Games.Game.Amazons;
    using Fx.Games.Strategy;

    class Program
    {
        private static readonly IReadOnlyList<(string, Action)> games = new (string, Action)[]
        {
            (nameof(PegsRandom), PegsRandom),
            (nameof(PegsHuman), PegsHuman),
            (nameof(PegsMonteCarlo), PegsMonteCarlo),
            (nameof(TicTacToeHumanVersusHuman), TicTacToeHumanVersusHuman),
            (nameof(TicTacToeHumanVersusRandom), TicTacToeHumanVersusRandom),
            (nameof(TicTacToeMonteCarloVersusRandom), TicTacToeMonteCarloVersusRandom),
            (nameof(TicTacToeMonteCarloVersusHuman), TicTacToeMonteCarloVersusHuman),
            (nameof(AmazonsHumanVsRandom_5x6), AmazonsHumanVsRandom_5x6),
            (nameof(AmazonsRandomVsRandom), AmazonsRandomVsRandom),
            (nameof(ConnectFourRandomVersusRandom), ConnectFourRandomVersusRandom),
            (nameof(ConnectFourRandomVersusMontyCarlo), ConnectFourRandomVersusMontyCarlo),
            (nameof(PegGameCueLearning), PegGameCueLearning),
            (nameof(TicTacToeCueLearningVersusRandom), TicTacToeCueLearningVersusRandom),
        };

        static void Main(string[] args)
        {
            for (int i = 0; true; ++i)
            {
                var sku = GetSkuFromArgsOrConsole(args, i);
                Console.Clear();
                games[sku].Item2();
                Console.WriteLine();
            }
        }

        private static int GetSkuFromArgsOrConsole(string[] args, int arg)
        {
            if (args.Length > arg && int.TryParse(args[arg], out var num))
            {
                return num;
            }

            Console.WriteLine("Available games:");
            for (int i = 0; i < games.Count; ++i)
            {
                Console.WriteLine($"{i}: {games[i].Item1}");
            }

            do
            {
                Console.WriteLine();
                Console.WriteLine("Provide SKU:");
                if (int.TryParse(Console.ReadLine(), out var sku) && sku >= 0 && sku < games.Count)
                {
                    return sku;
                }
            }
            while (true);
        }

        private sealed class TicTacToeBoardComparer : IEqualityComparer<TicTacToeBoard>
        {
            public TicTacToeBoardComparer()
            {
            }

            public static TicTacToeBoardComparer Instance { get; } = new TicTacToeBoardComparer();

            public bool Equals(TicTacToeBoard? x, TicTacToeBoard? y)
            {
                if (object.ReferenceEquals(x, y))
                {
                    return true;
                }

                if (x == null || y == null)
                {
                    return false;
                }

                var xGrid = x.Grid;
                var yGrid = y.Grid;
                for (int i = 0; i < 3; ++i)
                {
                    for (int j = 0; j < 3; ++j)
                    {
                        if (xGrid[i, j] != yGrid[i, j])
                        {
                            return false;
                        }
                    }
                }

                return true;
            }

            public int GetHashCode([DisallowNull] TicTacToeBoard obj)
            {
                var grid = obj.Grid;

                var hashCode = 0;
                for (int i = 0; i < 3; ++i)
                {
                    for (int j = 0; j < 3; ++j)
                    {
                        hashCode ^= grid[i, j].GetHashCode();
                    }
                }

                return hashCode;
            }
        }

        private sealed class TicTacToeMoveComparer : IEqualityComparer<TicTacToeMove>
        {
            public TicTacToeMoveComparer()
            {
            }

            public static TicTacToeMoveComparer Instance { get; } = new TicTacToeMoveComparer();

            public bool Equals(TicTacToeMove? x, TicTacToeMove? y)
            {
                if (object.ReferenceEquals(x, y))
                {
                    return true;
                }

                if (x == null || y == null)
                {
                    return false;
                }

                return
                    x.Row == y.Row &&
                    x.Column == y.Column;
            }

            public int GetHashCode([DisallowNull] TicTacToeMove obj)
            {
                return
                    obj.Row.GetHashCode() ^
                    obj.Column.GetHashCode();
            }
        }

        private static void TicTacToeCueLearningVersusRandom()
        {
            var displayer = new TicTacToeConsoleDisplayer<string>(_ => _);
            var exes = "exes";
            var ohs = "ohs";

            var cueLearningTrainer = new CueLearningTrainer<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>(
                () => new TicTacToe<string>(exes, ohs),
                exes,
                new CueLearningTrainerSettings<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>.Builder()
                {
                    BoardComparer = new TicTacToeBoardComparer(),
                    MoveComparer = new TicTacToeMoveComparer(),
                }.Build());
            var table = cueLearningTrainer.Train(2000); //// TODO why is this so slow

            var game = new TicTacToe<string>(exes, ohs);
            var driver = Driver.Create(
                new[]
                {
                    KeyValuePair.Create(exes, (IStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>)new CueLearningStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>(table)),
                    KeyValuePair.Create(ohs, (IStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>)game.RandomStrategy()),
                }.ToDb().ToDictionary(),
                displayer);
            var result = driver.Run(game);
        }

        private sealed class PegBoardComparer : IEqualityComparer<PegBoard>
        {
            public bool Equals(PegBoard? x, PegBoard? y)
            {
                if (x == y)
                {
                    return true;
                }

                if (x == null || y == null)
                {
                    return false;
                }

                if (x.Triangle.Length != y.Triangle.Length)
                {
                    return false;
                }

                for (int i = 0; i < x.Triangle.Length; ++i)
                {
                    if (x.Triangle[i].Length != y.Triangle[i].Length)
                    {
                        return false;
                    }

                    for (int j = 0; j < x.Triangle[i].Length; ++j)
                    {
                        if (x.Triangle[i][j] != y.Triangle[i][j])
                        {
                            return false;
                        }
                    }
                }

                return true;
            }

            public int GetHashCode([DisallowNull] PegBoard obj)
            {
                var hashCode = 0;
                for (int i = 0; i < obj.Triangle.Length; ++i)
                {
                    for (int j = 0; j < obj.Triangle[i].Length; ++j)
                    {
                        hashCode ^= obj.Triangle[i][j].GetHashCode();
                    }
                }

                return hashCode;
            }
        }

        private sealed class PegMoveComparer : IEqualityComparer<PegMove>
        {
            public bool Equals(PegMove? x, PegMove? y)
            {
                if (x == y)
                {
                    return true;
                }

                if (x == null || y == null)
                {
                    return false;
                }

                return
                    x.Start.Row == y.Start.Row &&
                    x.Start.Column == y.Start.Column &&
                    x.End.Row == y.End.Row &&
                    x.End.Column == y.End.Column;
            }

            public int GetHashCode([DisallowNull] PegMove obj)
            {
                return
                    obj.Start.Row ^
                    obj.Start.Column ^
                    obj.End.Row ^
                    obj.End.Column;
            }
        }

        private static void PegGameCueLearning()
        {
            var player = "player";

            var cueLearningTrainer = new CueLearningTrainer<PegGame<string>, PegBoard, PegMove, string>(
                () => new PegGame<string>(player),
                player,
                new CueLearningTrainerSettings<PegGame<string>, PegBoard, PegMove, string>.Builder()
                {
                    BoardComparer = new PegBoardComparer(),
                    MoveComparer = new PegMoveComparer(),
                }.Build());
            var table = cueLearningTrainer.Train(2000); //// TODO why is this so slow

            var displayer = PegGameConsoleDisplayer<string>.Instance;
            var game = new PegGame<string>(player);
            var driver = Driver.Create(
                new[] //// TODO use a fluent builder?
                {
                    KeyValuePair.Create(player, new CueLearningStrategy<PegGame<string>, PegBoard, PegMove, string>(table)),
                }.ToDb().ToDictionary(),
                displayer);
            var result = driver.Run(game);
        }
        
        private static void ConnectFourRandomVersusMontyCarlo()
        {
            var displayer = new ConnectFourDisplayer<string>(_ => _);
            var player1 = "player1";
            var player2 = "player2";

            var random1 = new Random();

            var game = new ConnectFour<string>(player1, player2);

            var driver = Driver.Create(
                new[]
                {
                    KeyValuePair.Create(player1, (IStrategy<ConnectFour<string>, ConnectFourBoard, ConnectFourMove, string>)new RandomStrategy<ConnectFour<string>, ConnectFourBoard, ConnectFourMove, string>(new RandomStrategySettings<ConnectFour<string>, ConnectFourBoard, ConnectFourMove, string>.Builder() {Random = random1 }.Build())),
                    KeyValuePair.Create(player2, (IStrategy<ConnectFour<string>, ConnectFourBoard, ConnectFourMove, string>)game.MonteCarloStrategy(player2, 1000, game.MonteCarloStrategySettings())),
                }.ToDb().ToDictionary(),
                displayer);
            var result = driver.Run(game);
        }
        
        private static void ConnectFourRandomVersusRandom()
        {
            var displayer = new ConnectFourDisplayer<string>(_ => _);
            var player1 = "player1";
            var player2 = "player2";

            var random1 = new Random(Environment.TickCount);
            var random2 = new Random(Environment.TickCount);

            var game = new ConnectFour<string>(player1, player2);
            var driver = Driver.Create(
                new[]
                {
                    KeyValuePair.Create(player1, (IStrategy<ConnectFour<string>, ConnectFourBoard, ConnectFourMove, string>)new RandomStrategy<ConnectFour<string>, ConnectFourBoard, ConnectFourMove, string>(new RandomStrategySettings<ConnectFour<string>, ConnectFourBoard, ConnectFourMove, string>.Builder() {Random = random1 }.Build())),
                    KeyValuePair.Create(player2, (IStrategy<ConnectFour<string>, ConnectFourBoard, ConnectFourMove, string>)new RandomStrategy<ConnectFour<string>, ConnectFourBoard, ConnectFourMove, string>(new RandomStrategySettings<ConnectFour<string>, ConnectFourBoard, ConnectFourMove, string>.Builder() {Random = random2 }.Build())),
                }.ToDb().ToDictionary(),
                displayer);
            var result = driver.Run(game);
        }

        private static void AmazonsRandomVsRandom()
        {
            var white = "white";
            var black = "black";
            var game = new Amazons.Game<string>(white, black);
            var displayer = new Amazons.Displayer<string>(_ => _);
            var strategies = new[] {
                KeyValuePair.Create(white, game.RandomStrategy()),
                KeyValuePair.Create(black, game.RandomStrategy()),
            };
            var driver = new Driver<Amazons.Game<string>, Amazons.Board, Amazons.Move, string>(
                strategies.ToDb().ToDictionary(),
                displayer);

            var result = driver.Run(game);
        }

        private static void AmazonsHumanVsRandom_5x6()
        {
            var displayer = new Amazons.Displayer<string>(_ => _);

            var white = "white";
            var black = "black";
            var game = new Amazons.Game<string>(white, black, (5, 6));
            var strategies = new[] {
                KeyValuePair.Create<string,IStrategy<Amazons.Game<string>, Amazons.Board, Amazons.Move, string>>(white, game.ConsoleStrategy()),
                KeyValuePair.Create<string,IStrategy<Amazons.Game<string>, Amazons.Board, Amazons.Move, string>>(black, game.RandomStrategy()),
            };
            var driver = new Driver<Amazons.Game<string>, Amazons.Board, Amazons.Move, string>(
                strategies.ToDb().ToDictionary(),
                displayer);

            var result = driver.Run(game);
        }

        private static void TicTacToeMonteCarloVersusHuman()
        {
            var displayer = new TicTacToeConsoleDisplayer<string>(_ => _);
            var exes = "exes";
            var ohs = "ohs";

            var game = new TicTacToe<string>(exes, ohs);
            var driver = Driver.Create(
                new[]
                {
                    KeyValuePair.Create(exes, (IStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>)game.MonteCarloStrategy(exes, 1000, game.MonteCarloStrategySettings())),
                    KeyValuePair.Create(ohs, (IStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>)game.ConsoleStrategy()),
                }.ToDb().ToDictionary(),
                displayer);
            var result = driver.Run(game);
        }

        private static void TicTacToeMonteCarloVersusRandom()
        {
            var displayer = new TicTacToeConsoleDisplayer<string>(_ => _);
            var exes = "exes";
            var ohs = "ohs";

            var game = new TicTacToe<string>(exes, ohs);
            var driver = Driver.Create(
                new[]
                {
                    KeyValuePair.Create(exes, (IStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>)game.MonteCarloStrategy(exes, 1000, game.MonteCarloStrategySettings())),
                    KeyValuePair.Create(ohs, (IStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>)game.RandomStrategy()),
                }.ToDb().ToDictionary(),
                displayer);
            var result = driver.Run(game);
        }

        private static void TicTacToeHumanVersusRandom()
        {
            var displayer = new TicTacToeConsoleDisplayer<string>(_ => _);
            var exes = "exes";
            var ohs = "ohs";

            var game = new TicTacToe<string>(exes, ohs);
            var driver = Fx.Games.Driver.Driver.Create(
                (new[]
                {
                    KeyValuePair.Create(exes, (IStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>)game.ConsoleStrategy()),
                    KeyValuePair.Create(ohs, (IStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>)game.RandomStrategy()),
                }).ToDb().ToDictionary(),
                displayer);
            var result = driver.Run(game);
        }

        private static void TicTacToeHumanVersusHuman()
        {
            var displayer = new TicTacToeConsoleDisplayer<string>(_ => _);
            var exes = "exes";
            var ohs = "ohs";

            var game = new TicTacToe<string>(exes, ohs);
            var driver = Fx.Games.Driver.Driver.Create(
                (new[]
                {
                    KeyValuePair.Create(exes, game.ConsoleStrategy()),
                    KeyValuePair.Create(ohs, game.ConsoleStrategy()),
                }).ToDb().ToDictionary(),
                displayer);
            var result = driver.Run(game);
        }

        private static void PegsMonteCarlo()
        {
            var displayer = PegGameConsoleDisplayer<string>.Instance;
            var player = "player";
            var game = new PegGame<string>(player);
            var driver = Driver.Create(
                new[] //// TODO use a fluent builder?
                {
                    KeyValuePair.Create(player, game.MonteCarloStrategy(player, 10000, game.MonteCarloStrategySettings())),
                }.ToDb().ToDictionary(),
                displayer);
            var result = driver.Run(game);
        }

        private static void PegsHuman()
        {
            var displayer = PegGameConsoleDisplayer<string>.Instance;
            var player = "player";
            var game = new PegGame<string>(player);
            var driver = Fx.Games.Driver.Driver.Create(
                (new[] //// TODO use a fluent builder?
                {
                    KeyValuePair.Create(player, game.ConsoleStrategy()),
                }).ToDb().ToDictionary(),
                displayer);
            var result = driver.Run(game);
        }

        private static void PegsRandom()
        {
            var displayer = PegGameConsoleDisplayer<string>.Instance;
            var player = "random";
            var game = new PegGame<string>(player);
            var driver = Fx.Games.Driver.Driver.Create(
                (new[]
                {
                    KeyValuePair.Create(player, game.RandomStrategy()),
                }).ToDb().ToDictionary(),
                displayer);
            var result = driver.Run(game);
        }
    }
}
