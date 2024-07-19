namespace ConsoleApplication1
{
    using System;

    using Db.System.Collections.Generic;

    using Fx.Games.Displayer;
    using Fx.Games.Driver;
    using Fx.Games.Game;
    using Fx.Games.Strategy;

    class Program
    {
        private static readonly global::System.Collections.Generic.IReadOnlyList<(string, Action)> games = new (string, Action)[]
        {
            (nameof(PegsRandom), PegsRandom),
            (nameof(PegsHuman), PegsHuman),
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

        private static void PegsHuman()
        {
            var displayer = new PegGameConsoleDisplayer<string>();
            var player = "player";
            var game = new PegGame<string>(player);
            var driver = Driver.Create(
                //// TODO dictionary.create
                new Dictionary<string, IStrategy<PegGame<string>, PegBoard, PegMove, string>>
                {
                    { player, new ConsoleStrategy<PegGame<string>, PegBoard, PegMove, string>() },
                },
                displayer);
            var result = driver.Run(game);
        }

        private static void PegsRandom()
        {
            var displayer = new PegGameConsoleDisplayer<string>();
            var player = "random";
            var game = new PegGame<string>(player);
            var driver = Driver.Create(
                //// TODO dictionary.create
                new Dictionary<string, IStrategy<PegGame<string>, PegBoard, PegMove, string>>
                {
                    { player, new RandomStrategy<PegGame<string>, PegBoard, PegMove, string>() },
                },
                displayer);
            var result = driver.Run(game);
        }
    }
}
