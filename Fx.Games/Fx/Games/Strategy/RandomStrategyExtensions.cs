namespace Fx.Games.Strategy
{
    using Fx.Games.Game;

    public static class RandomStrategyExtensions
    {
        public static RandomStrategy<TGame, TBoard, TMove, TPlayer> RandomStrategy<TGame, TBoard, TMove, TPlayer>(this IGame<TGame, TBoard, TMove, TPlayer> self) where TGame : IGame<TGame, TBoard, TMove, TPlayer>
        {
            return new RandomStrategy<TGame, TBoard, TMove, TPlayer>();
        }
    }
}
