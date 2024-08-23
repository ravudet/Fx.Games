namespace Fx.Games.Game.Amazons
{
    using System;
    using System.Linq;

    public sealed class Displayer<TPlayer> : Displayer.IDisplayer<GameOfAmazons<TPlayer>, Board, Move, TPlayer>
        where TPlayer : notnull // neccesary to be able to use a TPlayer as a key in an internal dictionary
    {
        private readonly Func<TPlayer, string> playerToString;

        public Displayer(Func<TPlayer, string> playerToString)
        {
            this.playerToString = playerToString;
        }

        public void DisplayBoard(GameOfAmazons<TPlayer> game)
        {
            var board = game.Board;

            Console.WriteLine("   a  b  c  d  e  f  g  h  i  j");
            for (int j = 0; j < Board.BOARD_SIZE.Height; ++j)
            {
                Console.Write($"{(Board.BOARD_SIZE.Height - j) % 10} ");
                for (int i = 0; i < Board.BOARD_SIZE.Width; ++i)
                {
                    var str = board[i, j] switch
                    {
                        SquareState.Empty => " . ",
                        SquareState.Black => " B ",
                        SquareState.White => " W ",
                        SquareState.Arrow => " * ",
                        _ => throw new InvalidOperationException("Invalid square")
                    };
                    Console.Write(str);
                }
                Console.Write($" \x1b[37m{(Board.BOARD_SIZE.Height - j) % 10}\x1b[0m");
                Console.WriteLine();
            }
            Console.WriteLine("   a  b  c  d  e  f  g  h  i  j");
        }

        public void DisplayOutcome(GameOfAmazons<TPlayer> game)
        {
            var (winners, losers, drawers) = game.WinnersAndLosers;

            Console.WriteLine($"{string.Join(", ", from winner in winners select playerToString(winner))} wins!");
            Console.WriteLine($"{string.Join(", ", from loser in losers select playerToString(loser))} lose!");
            Console.WriteLine($"{string.Join(", ", from drawer in drawers select playerToString(drawer))} draws!");
        }

        public void DisplayAvailableMoves(GameOfAmazons<TPlayer> game)
        {
            var color = game.PlayerPiece[game.CurrentPlayer] == SquareState.Black ? "black" : "white";
            var hierarchy = game.Moves.GroupBy(move => move.From).Select(f => (From: f.Key, f.GroupBy(move => move.To).Select(t => (To: t.Key, t.Select(move => move.Arrow))))).ToArray();

            var i = 0;
            foreach (var (from, tos) in hierarchy)
            {
                Console.WriteLine($"Move {color} Amazon from {Format(from)}");
                foreach (var (to, arrows) in tos)
                {
                    // Console.WriteLine($"    To {Format(to)}");
                    // Console.Write($"        {i} Arrow");
                    foreach (var arrow in arrows)
                    {
                        // Console.Write($" {Format(arrow)}");
                        i += 1;
                    }
                    // Console.WriteLine();
                }
            }
            // foreach (var move in game.Moves)
            // {
            //     Console.WriteLine($"Move {color} amazon from {move.From} to {move.To} and shoot arrow at {move.Arrow}");
            // }
        }

        private static string Format((int X, int Y) position)
        {
            return $"{(char)('a' + position.X)}{(position.Y + 1) % 10}";
        }

        public void DisplaySelectedMove(Move move)
        {
            Console.WriteLine($"Moving from {move.From} to {move.To} and shooting arrow at {move.Arrow}");
        }
    }
}

