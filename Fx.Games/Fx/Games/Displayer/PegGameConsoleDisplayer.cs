namespace Fx.Games.Displayer
{
    using System;
    using System.Linq;

    using Fx.Games.Game;

    public sealed class PegGameConsoleDisplayer<TPlayer> : IDisplayer<PegGame<TPlayer>, PegBoard, PegMove, TPlayer>
    {
        public void DisplayAvailableMoves(PegGame<TPlayer> game)
        {
            //// TODO figureou thow to add this back with the undoable game
            //// Console.WriteLine("Available moves:");
            int i = 0;
            foreach (var move in game.Moves)
            {
                Console.WriteLine($"{i++}: {TranscribeMove(move)}");
            }

            Console.WriteLine();
        }

        public void DisplayBoard(PegGame<TPlayer> game)
        {
            for (int i = 0; i < game.Board.Triangle.Length; ++i)
            {
                Console.Write(new string(' ', game.Board.Triangle.Length - i));
                for (int j = 0; j < game.Board.Triangle[i].Length; ++j)
                {
                    Console.Write(game.Board.Triangle[i][j] == Peg.Empty ? "o " : "* ");
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }

        public void DisplayOutcome(PegGame<TPlayer> game)
        {
            if (game.WinnersAndLosers.Winners.Any())
            {
                Console.WriteLine("You win!");
            }
            else
            {
                Console.WriteLine("You lose!");
            }
        }

        public void DisplaySelectedMove(PegMove move)
        {
            Console.WriteLine($"The selected move was {TranscribeMove(move)}");
            Console.WriteLine();
        }

        private static string TranscribeMove(PegMove move)
        {
            return $"({move.Start.Item1},{move.Start.Item2}) => ({move.End.Item1}, {move.End.Item2})";
        }
    }
}
