namespace Fx.Games.Strategy
{
    using Fx.Games.Game;

    public static class ConsoleStrategyExtensions
    {
        public static ConsoleStrategy<TGame, TBoard, TMove, TPlayer> ConsoleStrategy<TGame, TBoard, TMove, TPlayer>(this IGame<TGame, TBoard, TMove, TPlayer> self) where TGame : IGame<TGame, TBoard, TMove, TPlayer>
        {
            return Fx.Games.Strategy.ConsoleStrategy<TGame, TBoard, TMove, TPlayer>.Instance;
        }
    }
}
