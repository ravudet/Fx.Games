namespace Fx.Games.Game.Snake
{
    using Fx.Games.Displayer;
    using System;

    public sealed class Displayer<TPlayer> : IDisplayer<Game<TPlayer>, Board, Move, TPlayer>
    {
        public void DisplayAvailableMoves(Game<TPlayer> game)
        {
            Console.WriteLine("Available moves:");
            int i = 0;
            foreach (var move in game.Moves)
            {
                Console.WriteLine($"{i++}: {move.ToString()}");
            }

            Console.WriteLine();
        }

        public void DisplayBoard(Game<TPlayer> game)
        {
            Console.WriteLine(new string('*', game.Board.Columns + 2));
            for (int i = 0; i < game.Board.Grid.Count; ++i)
            {
                Console.Write("*");
                for (int j = 0; j < game.Board.Grid[i].Count; ++j)
                {
                    if (game.Board.Grid[i][j] == Space.Food)
                    {
                        Console.Write("+");
                    }
                    else if (game.Board.Grid[i][j] == Space.Snake)
                    {
                        Console.Write(".");
                    }
                    else if (game.Board.Grid[i][j] == Space.UpHead)
                    {
                        Console.Write("^");
                    }
                    else if (game.Board.Grid[i][j] == Space.DownHead)
                    {
                        Console.Write("!");
                    }
                    else if (game.Board.Grid[i][j] == Space.LeftHead)
                    {
                        Console.Write("<");
                    }
                    else if (game.Board.Grid[i][j] == Space.RightHead)
                    {
                        Console.Write(">");
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }

                Console.Write("*");
                Console.WriteLine();
            }

            Console.WriteLine(new string('*', game.Board.Columns + 2));
        }

        public void DisplayOutcome(Game<TPlayer> game)
        {
        }

        public void DisplaySelectedMove(Move move)
        {
            Console.WriteLine(move.ToString());
        }
    }
}
