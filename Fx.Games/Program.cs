namespace ConsoleApplication1
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Db.System.Collections.Generic;
    using DbAdapters.System.Collections.Generic;
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
            (nameof(AmazonsHumanVersusRandom_5x6), AmazonsHumanVersusRandom_5x6),
            (nameof(AmazonsHumanVersusMonteCarlo), AmazonsHumanVersusMonteCarlo),
            (nameof(AmazonsHumanVersusMinimizeMoves), AmazonsHumanVersusMinimizeMoves),
            (nameof(AmazonsRandomVersusRandom), AmazonsRandomVersusRandom),
            (nameof(AmazonsMonteCarloVersusMinimizeMoves_5x6), AmazonsMonteCarloVersusMinimizeMoves_5x6),
            (nameof(AmazonsMonteCarloVersusMinimizeMoves_8x8), AmazonsMonteCarloVersusMinimizeMoves_8x8),
            (nameof(ConnectFourRandomVersusRandom), ConnectFourRandomVersusRandom),
            (nameof(ConnectFourRandomVersusMontyCarlo), ConnectFourRandomVersusMontyCarlo),
            (nameof(ConnectFourHumanVersusMontyCarlo), ConnectFourHumanVersusMontyCarlo),
            (nameof(ConnectFourHumanVersusHuman), ConnectFourHumanVersusHuman),
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

        private static void ConnectFourHumanVersusHuman()
        {
            var displayer = new ConnectFourDisplayer<string>(_ => _);
            var player1 = "player1";
            var player2 = "player2";

            var random1 = new Random();

            var game = new ConnectFour<string>(player1, player2);

            var driver = Driver.Create(
                new[]
                {
                    KeyValuePair.Create(player1, (IStrategy<ConnectFour<string>, ConnectFourBoard, ConnectFourMove, string>) ConsoleStrategy<ConnectFour<string>, ConnectFourBoard, ConnectFourMove, string>.Instance),
                    KeyValuePair.Create(player2, (IStrategy<ConnectFour<string>, ConnectFourBoard, ConnectFourMove, string>) ConsoleStrategy<ConnectFour<string>, ConnectFourBoard, ConnectFourMove, string>.Instance),
                }.ToDb().ToDictionary(),
                displayer);
            var result = driver.Run(game);
        }

        private static void ConnectFourHumanVersusMontyCarlo()
        {
            var displayer = new ConnectFourDisplayer<string>(_ => _);
            var player1 = "player1";
            var player2 = "player2";

            var random1 = new Random();

            var game = new ConnectFour<string>(player1, player2);

            var driver = Driver.Create(
                new[]
                {
                    KeyValuePair.Create(player1, (IStrategy<ConnectFour<string>, ConnectFourBoard, ConnectFourMove, string>) ConsoleStrategy<ConnectFour<string>, ConnectFourBoard, ConnectFourMove, string>.Instance),
                    KeyValuePair.Create(player2, (IStrategy<ConnectFour<string>, ConnectFourBoard, ConnectFourMove, string>)game.MonteCarloStrategy(player2, 1000000, game.MonteCarloStrategySettings())),
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

        private static void AmazonsMonteCarloVersusMinimizeMoves_8x8()
        {
            var white = "white";
            var black = "black";
            var game = new Amazons.Game<string>(white, black, (8, 8));
            var displayer = new Amazons.Displayer<string>(_ => _);
            var strategies = new[] {
                KeyValuePair.Create<string,IStrategy<Amazons.Game<string>, Amazons.Board, Amazons.Move, string>>(white, game.MonteCarloStrategy(white, 100000, game.MonteCarloStrategySettings())),
                KeyValuePair.Create<string,IStrategy<Amazons.Game<string>, Amazons.Board, Amazons.Move, string>>(black, game.MinimizeMovesStrategy()),
            };
            var driver = new Driver<Amazons.Game<string>, Amazons.Board, Amazons.Move, string>(
                strategies.ToDb().ToDictionary(),
                displayer);

            var result = driver.Run(game);
        }

        private static void AmazonsMonteCarloVersusMinimizeMoves_5x6()
        {
            var white = "white";
            var black = "black";
            var game = new Amazons.Game<string>(white, black, (5, 6));
            var displayer = new Amazons.Displayer<string>(_ => _);
            var strategies = new[] {
                KeyValuePair.Create<string,IStrategy<Amazons.Game<string>, Amazons.Board, Amazons.Move, string>>(white, game.MonteCarloStrategy(white, 100000, game.MonteCarloStrategySettings())),
                KeyValuePair.Create<string,IStrategy<Amazons.Game<string>, Amazons.Board, Amazons.Move, string>>(black, game.MinimizeMovesStrategy()),
            };
            var driver = new Driver<Amazons.Game<string>, Amazons.Board, Amazons.Move, string>(
                strategies.ToDb().ToDictionary(),
                displayer);

            var result = driver.Run(game);
        }

        private static void AmazonsRandomVersusRandom()
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

        private static void AmazonsHumanVersusMinimizeMoves()
        {
            var displayer = new Amazons.Displayer<string>(_ => _);

            var white = "white";
            var black = "black";
            var game = new Amazons.Game<string>(white, black, (5, 6));
            var strategies = new[] {
                KeyValuePair.Create<string,IStrategy<Amazons.Game<string>, Amazons.Board, Amazons.Move, string>>(white, game.AmazonsConsoleStrategy()),
                KeyValuePair.Create<string,IStrategy<Amazons.Game<string>, Amazons.Board, Amazons.Move, string>>(black, game.MinimizeMovesStrategy()),
            };
            var driver = new Driver<Amazons.Game<string>, Amazons.Board, Amazons.Move, string>(
                strategies.ToDb().ToDictionary(),
                displayer);

            var result = driver.Run(game);
        }

        private static void AmazonsHumanVersusMonteCarlo()
        {
            var displayer = new Amazons.Displayer<string>(_ => _);

            var white = "white";
            var black = "black";
            var game = new Amazons.Game<string>(white, black, (5, 6));
            var strategies = new[] {
                KeyValuePair.Create<string,IStrategy<Amazons.Game<string>, Amazons.Board, Amazons.Move, string>>(white, game.AmazonsConsoleStrategy()),
                KeyValuePair.Create<string,IStrategy<Amazons.Game<string>, Amazons.Board, Amazons.Move, string>>(black, game.MonteCarloStrategy(black, 100000, game.MonteCarloStrategySettings())),
            };
            var driver = new Driver<Amazons.Game<string>, Amazons.Board, Amazons.Move, string>(
                strategies.ToDb().ToDictionary(),
                displayer);

            var result = driver.Run(game);
        }

        private static void AmazonsHumanVersusRandom_5x6()
        {
            var displayer = new Amazons.Displayer<string>(_ => _);

            var white = "white";
            var black = "black";
            var game = new Amazons.Game<string>(white, black, (5, 6));
            var strategies = new[] {
                KeyValuePair.Create<string,IStrategy<Amazons.Game<string>, Amazons.Board, Amazons.Move, string>>(white, game.AmazonsConsoleStrategy()),
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
