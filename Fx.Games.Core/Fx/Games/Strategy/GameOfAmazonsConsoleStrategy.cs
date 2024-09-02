namespace Fx.Games.Strategy
{
    using System.Linq;
    using System;

    using Fx.Games.Game;
    using Fx.Games.Game.Amazons;

    /// <summary>
    /// A strategy specific to <see cref="Game{TPlayer}"/> that reads the details of the next move from the console
    /// </summary>
    /// <typeparam name="TPlayer">The type of the player playing <see cref="Game{TPlayer}"/></typeparam>
    public sealed class GameOfAmazonsConsoleStrategy<TPlayer> : IStrategy<Game<TPlayer>, Board, Move, TPlayer>
    {
        private GameOfAmazonsConsoleStrategy()
        {
        }

        public static GameOfAmazonsConsoleStrategy<TPlayer> Instance { get; } = new GameOfAmazonsConsoleStrategy<TPlayer>();

        public Move SelectMove(Game<TPlayer> game)
        {
            if (game == null)
            {
                throw new ArgumentNullException(nameof(game));
            }

            var moves = game.Moves.ToList();
            Square amazonSquare;
            while (true)
            {
                Console.WriteLine("Enter the coordinates of the amazon that you would like to move (for example, 'B3') or enter 'all' to display all available moves:");
                var input = Console.ReadLine()!;

                if (string.Equals(input, "all", StringComparison.OrdinalIgnoreCase))
                {
                    return game.ConsoleStrategy().SelectMove(game);
                }

                if (input[0] < 'a' || input[0] > ('a' + game.Board.Size.Width))
                {
                    Console.WriteLine($"The input '{input}' did not start with the character designation of a column");
                    continue;
                }

                if (input[1] < '1' || input[1] > ('1' + game.Board.Size.Height))
                {
                    Console.WriteLine($"The input '{input}' did not end with the numeral designation of a row");
                    continue;
                }

                amazonSquare = new Square(input[0] - 'a', input[1] - '1');
                if (!moves.Any(move => move.Amazon.X == amazonSquare.X && move.Amazon.Y == amazonSquare.Y))
                {
                    Console.WriteLine($"The input '{input}' did correspond to any legal moves");
                    continue;
                }

                break;
            }

            Square amazonDestinationSquare;
            while (true)
            {
                Console.WriteLine("Enter the coordinates of the square that you would like to move the amazon to (for example, 'B3'):");
                var input = Console.ReadLine()!;

                if (input[0] < 'a' || input[0] > ('a' + game.Board.Size.Width))
                {
                    Console.WriteLine($"The input '{input}' did not start with the character designation of a column");
                    continue;
                }

                if (input[1] < '1' || input[1] > ('1' + game.Board.Size.Height))
                {
                    Console.WriteLine($"The input '{input}' did not end with the numeral designation of a row");
                    continue;
                }

                amazonDestinationSquare = new Square(input[0] - 'a', input[1] - '1');
                if (!moves.Any(move => move.Destination.X == amazonDestinationSquare.X && move.Destination.Y == amazonDestinationSquare.Y))
                {
                    Console.WriteLine($"The input '{input}' did correspond to any legal moves");
                    continue;
                }

                break;
            }

            Square targetSquare;
            while (true)
            {
                Console.WriteLine("Enter the coordinates of the square that you would like the amazon to shoot with an arrow (for example, 'B3'):");
                var input = Console.ReadLine()!;

                if (input[0] < 'a' || input[0] > ('a' + game.Board.Size.Width))
                {
                    Console.WriteLine($"The input '{input}' did not start with the character designation of a column");
                    continue;
                }

                if (input[1] < '1' || input[1] > ('1' + game.Board.Size.Height))
                {
                    Console.WriteLine($"The input '{input}' did not end with the numeral designation of a row");
                    continue;
                }

                targetSquare = new Square(input[0] - 'a', input[1] - '1');
                if (!moves.Any(move => move.Target.X == targetSquare.X && move.Target.Y == targetSquare.Y))
                {
                    Console.WriteLine($"The input '{input}' did correspond to any legal moves");
                    continue;
                }

                break;
            }

            return new Move(amazonSquare, amazonDestinationSquare, targetSquare);
        }
    }
}
