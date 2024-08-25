namespace Fx.Games.Game.Amazons
{
    using System;
    using System.Linq;

    public sealed class Displayer<TPlayer> : Displayer.IDisplayer<Game<TPlayer>, Board, Move, TPlayer>
        where TPlayer : notnull // neccesary to be able to use a TPlayer as a key in an internal dictionary
    {
        private readonly Func<TPlayer, string> playerToString;

        public Displayer(Func<TPlayer, string> playerToString)
        {
            this.playerToString = playerToString;
        }

        public void DisplayBoard(Game<TPlayer> game)
        {
            var board = game.Board;

            Console.WriteLine(board);
        }

        public void DisplayOutcome(Game<TPlayer> game)
        {
            var (winners, losers, drawers) = game.WinnersAndLosers;

            Console.WriteLine($"{string.Join(", ", from winner in winners select playerToString(winner))} wins!");
            Console.WriteLine($"{string.Join(", ", from loser in losers select playerToString(loser))} lose!");
            Console.WriteLine($"{string.Join(", ", from drawer in drawers select playerToString(drawer))} draws!");
        }

        public void DisplayAvailableMoves(Game<TPlayer> game)
        {
            var color = game.PlayerTileMapping[game.CurrentPlayer] == Board.Tile.Black ? "black" : "white";

            // var hierarchy = game.Moves.GroupBy(move => move.Amazon).Select(f => (From: f.Key, f.GroupBy(move => move.Destination).Select(t => (To: t.Key, t.Select(move => move.Target))))).ToArray();


            Console.WriteLine($"Select a move for {color}:");
            int i = 0;
            foreach (var move in game.Moves)
            {
                Console.WriteLine($"{i++}: amazon on {move.Amazon} to {move.Destination} and shoot arrow to {move.Target}");
            }
        }

        private static string Format((int X, int Y) position)
        {
            return $"{(char)('a' + position.X)}{(position.Y + 1) % 10}";
        }

        public void DisplaySelectedMove(Move move)
        {
            Console.WriteLine($"Moving from {move.Amazon} to {move.Destination} and shooting arrow at {move.Target}");
        }
    }
}

