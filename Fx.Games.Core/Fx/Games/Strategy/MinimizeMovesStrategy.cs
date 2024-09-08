namespace Fx.Games.Strategy
{
    using Fx.Games.Game;
    using System.Linq;

    public sealed class MinimizeMovesStrategy<TGame, TBoard, TMove, TPlayer> : IStrategy<TGame, TBoard, TMove, TPlayer> where TGame : IGame<TGame, TBoard, TMove, TPlayer>
    {
        private MinimizeMovesStrategy()
        {
        }

        public static MinimizeMovesStrategy<TGame, TBoard, TMove, TPlayer> Instance { get; } = new MinimizeMovesStrategy<TGame, TBoard, TMove, TPlayer>();

        public TMove SelectMove(TGame game)
        {
            var moves = game.Moves.ToList();
            if (moves.Count == 0)
            {
                throw new InvalidGameException("tODO");
            }

            //// TODO we already assert that there are moves
            return moves.MinBy(move => game.CommitMove(move).Moves.Count())!;
        }
    }
}
