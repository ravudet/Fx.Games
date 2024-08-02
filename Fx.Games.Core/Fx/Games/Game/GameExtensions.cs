namespace Fx.Games.Game
{
    using Fx.Games.Displayer;
    using Fx.Games.Driver;

    /// <summary>
    /// Extension methods for <see cref="IGame{TGame, TBoard, TMove, TPlayer}"/>
    /// </summary>
    public static class GameExtensions
    {
        /// <summary>
        /// Gets the <see cref="Fx.Games.Displayer.NullDisplayer{TGame, TBoard, TMove, TPlayer}"/> for <paramref name="game"/>
        /// </summary>
        /// <typeparam name="TGame">The type of the game that is being displayed</typeparam>
        /// <typeparam name="TBoard">The type of the board that the <typeparamref name="TGame"/> uses</typeparam>
        /// <typeparam name="TMove">The type of the moves that the <typeparamref name="TGame"/> uses</typeparam>
        /// <typeparam name="TPlayer">The type of the player that is playing the <typeparamref name="TGame"/></typeparam>
        /// <param name="game">The game to be displayed</param>
        /// <returns>The <see cref="Fx.Games.Displayer.NullDisplayer{TGame, TBoard, TMove, TPlayer}"/> for <paramref name="game"/></returns>
        public static NullDisplayer<TGame, TBoard, TMove, TPlayer> NullDisplayer<TGame, TBoard, TMove, TPlayer>(this IGame<TGame, TBoard, TMove, TPlayer> game) where TGame : IGame<TGame, TBoard, TMove, TPlayer>
        {
            return Fx.Games.Displayer.NullDisplayer<TGame, TBoard, TMove, TPlayer>.Instance;
        }

        public static DriverSettings<TGame, TBoard, TMove, TPlayer>.Builder DriverSettings<TGame, TBoard, TMove, TPlayer>(this IGame<TGame, TBoard, TMove, TPlayer> game) where TGame : IGame<TGame, TBoard, TMove, TPlayer>
        {
            return new DriverSettings<TGame, TBoard, TMove, TPlayer>.Builder();
        }
    }
}
