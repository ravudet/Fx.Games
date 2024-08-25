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
            (nameof(TicTacToeHumanVersusHuman), TicTacToeHumanVersusHuman),
            (nameof(TicTacToeHumanVersusRandom), TicTacToeHumanVersusRandom),
            (nameof(AmazonsHumanVsRandom_5x6), AmazonsHumanVsRandom_5x6),
            (nameof(AmazonsRandomVsRandom), AmazonsRandomVsRandom),
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

        private static void AmazonsHumanVsRandom_5x6()
        {
            var white = "white";
            var black = "black";
            var game = new Amazons.Game<string>(white, black, (5, 6));

            var displayer = new Amazons.Displayer<string>(_ => _);

            var driver = new Driver<Amazons.Game<string>, Amazons.Board, Amazons.Move, string>(
                            (new[]                             {
                                KeyValuePair.Create<string,IStrategy<Amazons.Game<string>, Amazons.Board, Amazons.Move, string>>(white, game.ConsoleStrategy()),
                                KeyValuePair.Create<string,IStrategy<Amazons.Game<string>, Amazons.Board, Amazons.Move, string>>(black, game.RandomStrategy()),
                            }).ToDb().ToDictionary(),
                            displayer);

            var result = driver.Run(game);
        }

        private static void AmazonsRandomVsRandom()
        {
            var white = "white";
            var black = "black";
            var game = new Amazons.Game<string>(white, black);

            var displayer = new Amazons.Displayer<string>(_ => _);

            var driver = new Driver<Amazons.Game<string>, Fx.Games.Game.Amazons.Board, Fx.Games.Game.Amazons.Move, string>(
                            (new[]                             {
                                KeyValuePair.Create(white, game.RandomStrategy()),
                                KeyValuePair.Create(black, game.RandomStrategy()),
                            }).ToDb().ToDictionary(),
                            displayer);

            var result = driver.Run(game);
        }
    }
}
