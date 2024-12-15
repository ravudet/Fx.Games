namespace Fx.Games.Strategy
{
    using Fx.Distribution;
    using Fx.Games.Game;
    using System.Linq;

    public sealed class MinimizeMovesStrategy<TGame, TBoard, TMove, TPlayer, TDistribution> : IStrategy<TGame, TBoard, TMove, TPlayer, TDistribution> where TGame : IGame<TGame, TBoard, TMove, TPlayer, TDistribution> where TDistribution : IDistribution<TGame>
    {
        private MinimizeMovesStrategy()
        {
        }

        public static MinimizeMovesStrategy<TGame, TBoard, TMove, TPlayer, TDistribution> Instance { get; } = new MinimizeMovesStrategy<TGame, TBoard, TMove, TPlayer, TDistribution>();

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
