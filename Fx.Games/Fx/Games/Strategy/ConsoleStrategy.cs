namespace Fx.Games.Strategy
{
    using System.Linq;
    using System;

    using Fx.Games.Game;

    public sealed class ConsoleStrategy<TGame, TBoard, TMove, TPlayer> : IStrategy<TGame, TBoard, TMove, TPlayer> where TGame : IGame<TGame, TBoard, TMove, TPlayer>
    {
        public TMove SelectMove(TGame game)
        {
            Console.WriteLine("Enter the index of the move you would like to choose:");
            var moves = game.Moves.ToList();
            while (true)
            {
                var input = Console.ReadLine();
                if (!int.TryParse(input, out var selectedMove) || selectedMove >= moves.Count)
                {
                    Console.WriteLine($"The input '{input}' was not the index of a legal move");
                    continue;
                }

                return moves[selectedMove];
            }
        }
    }
}
