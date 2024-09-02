namespace Fx.Games.Game
{
    using Fx.Games.Displayer;
    using Fx.Games.Driver;
    using Fx.Games.Strategy;

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

        /// <summary>
        /// Gets a builder instance for the <see cref="DriverSettings{TGame, TBoard, TMove, TPlayer}(IGame{TGame, TBoard, TMove, TPlayer})"/> that would be used to configure a <see cref="Driver{TGame, TBoard, TMove, TPlayer}"/> that plays <paramref name="game"/>
        /// </summary>
        /// <typeparam name="TGame">The type of the game that the <see cref="Driver{TGame, TBoard, TMove, TPlayer}"/> would play</typeparam>
        /// <typeparam name="TBoard">The type of the board that <typeparamref name="TGame"/> uses</typeparam>
        /// <typeparam name="TMove">The type of the moves that the <typeparamref name="TGame"/> uses</typeparam>
        /// <typeparam name="TPlayer">The type of the player that is playing the <typeparamref name="TGame"/></typeparam>
        /// <param name="game">The game that the <see cref="Driver{TGame, TBoard, TMove, TPlayer}"/> will play</param>
        /// <returns>A builder instance for the <see cref="DriverSettings{TGame, TBoard, TMove, TPlayer}(IGame{TGame, TBoard, TMove, TPlayer})"/> that would be used to configure a <see cref="Driver{TGame, TBoard, TMove, TPlayer}"/> that plays <paramref name="game"/></returns>
        public static DriverSettings<TGame, TBoard, TMove, TPlayer>.Builder DriverSettings<TGame, TBoard, TMove, TPlayer>(this IGame<TGame, TBoard, TMove, TPlayer> game) where TGame : IGame<TGame, TBoard, TMove, TPlayer>
        {
            return new DriverSettings<TGame, TBoard, TMove, TPlayer>.Builder();
        }

        /// <summary>
        /// Gets a <see cref="Fx.Games.Strategy.RandomStrategy{TGame, TBoard, TMove, TPlayer}"/> that can be used to play <paramref name="self"/>
        /// </summary>
        /// <typeparam name="TGame">The type of the game that the <see cref="Fx.Games.Strategy.RandomStrategy{TGame, TBoard, TMove, TPlayer}"/> would play</typeparam>
        /// <typeparam name="TBoard">The type of the board that <typeparamref name="TGame"/> uses</typeparam>
        /// <typeparam name="TMove">The type of the moves that the <typeparamref name="TGame"/> uses</typeparam>
        /// <typeparam name="TPlayer">The type of the player that is playing the <typeparamref name="TGame"/></typeparam>
        /// <param name="self">The game that the <see cref="Fx.Games.Strategy.RandomStrategy{TGame, TBoard, TMove, TPlayer}"/> will play</param>
        /// <returns>A <see cref="Fx.Games.Strategy.RandomStrategy{TGame, TBoard, TMove, TPlayer}"/> that can be used to play <paramref name="self"/></returns>
        public static RandomStrategy<TGame, TBoard, TMove, TPlayer> RandomStrategy<TGame, TBoard, TMove, TPlayer>(this IGame<TGame, TBoard, TMove, TPlayer> self) where TGame : IGame<TGame, TBoard, TMove, TPlayer>
        {
            return new RandomStrategy<TGame, TBoard, TMove, TPlayer>();
        }

        /// <summary>
        /// Gets a <see cref="Fx.Games.Strategy.ConsoleStrategy{TGame, TBoard, TMove, TPlayer}"/> that can be used to play <paramref name="self"/>
        /// </summary>
        /// <typeparam name="TGame">The type of the game that the <see cref="Fx.Games.Strategy.ConsoleStrategy{TGame, TBoard, TMove, TPlayer}"/> would play</typeparam>
        /// <typeparam name="TBoard">The type of the board that <typeparamref name="TGame"/> uses</typeparam>
        /// <typeparam name="TMove">The type of the moves that the <typeparamref name="TGame"/> uses</typeparam>
        /// <typeparam name="TPlayer">The type of the player that is playing the <typeparamref name="TGame"/></typeparam>
        /// <param name="self">The game that the <see cref="Fx.Games.Strategy.ConsoleStrategy{TGame, TBoard, TMove, TPlayer}"/> will play</param>
        /// <returns>A <see cref="Fx.Games.Strategy.ConsoleStrategy{TGame, TBoard, TMove, TPlayer}"/> that can be used to play <paramref name="self"/></returns>
        public static ConsoleStrategy<TGame, TBoard, TMove, TPlayer> ConsoleStrategy<TGame, TBoard, TMove, TPlayer>(this IGame<TGame, TBoard, TMove, TPlayer> self) where TGame : IGame<TGame, TBoard, TMove, TPlayer>
        {
            return Fx.Games.Strategy.ConsoleStrategy<TGame, TBoard, TMove, TPlayer>.Instance;
        }

        /// <summary>
        /// Gets a <see cref="Fx.Games.Strategy.MonteCarloStrategy{TGame, TBoard, TMove, TPlayer}"/> that can be used to play <paramref name="self"/>
        /// </summary>
        /// <typeparam name="TGame">The type of the game that the <see cref="Fx.Games.Strategy.MonteCarloStrategy{TGame, TBoard, TMove, TPlayer}"/> would play</typeparam>
        /// <typeparam name="TBoard">The type of the board that <typeparamref name="TGame"/> uses</typeparam>
        /// <typeparam name="TMove">The type of the moves that the <typeparamref name="TGame"/> uses</typeparam>
        /// <typeparam name="TPlayer">The type of the player that is playing the <typeparamref name="TGame"/></typeparam>
        /// <param name="self">The game that the <see cref="Fx.Games.Strategy.MonteCarloStrategy{TGame, TBoard, TMove, TPlayer}"/> will play</param>
        /// <returns>A <see cref="Fx.Games.Strategy.MonteCarloStrategy{TGame, TBoard, TMove, TPlayer}"/> that can be used to play <paramref name="self"/></returns>
        public static MonteCarloStrategy<TGame, TBoard, TMove, TPlayer> MonteCarloStrategy<TGame, TBoard, TMove, TPlayer>(this IGame<TGame, TBoard, TMove, TPlayer> self, TPlayer player, int maxDecisionCount, MonteCarloStrategySettings<TGame, TBoard, TMove, TPlayer> settings) where TGame : IGame<TGame, TBoard, TMove, TPlayer>
        {
            return new MonteCarloStrategy<TGame, TBoard, TMove, TPlayer>(player, maxDecisionCount, settings);
        }

        /// <summary>
        /// Gets the default <see cref="Fx.Games.Strategy.MonteCarloStrategySettings{TGame, TBoard, TMove, TPlayer}"/> that can be used create a <see cref="Fx.Games.Strategy.MonteCarloStrategy{TGame, TBoard, TMove, TPlayer}"/> to play <paramref name="self"/>
        /// </summary>
        /// <typeparam name="TGame">The type of the game that the <see cref="Fx.Games.Strategy.MonteCarloStrategy{TGame, TBoard, TMove, TPlayer}"/> would play</typeparam>
        /// <typeparam name="TBoard">The type of the board that <typeparamref name="TGame"/> uses</typeparam>
        /// <typeparam name="TMove">The type of the moves that the <typeparamref name="TGame"/> uses</typeparam>
        /// <typeparam name="TPlayer">The type of the player that is playing the <typeparamref name="TGame"/></typeparam>
        /// <param name="self">The game that the <see cref="Fx.Games.Strategy.MonteCarloStrategy{TGame, TBoard, TMove, TPlayer}"/> will play</param>
        /// <returns>A <see cref="Fx.Games.Strategy.MonteCarloStrategySettings{TGame, TBoard, TMove, TPlayer}"/> that can be used create a <see cref="Fx.Games.Strategy.MonteCarloStrategy{TGame, TBoard, TMove, TPlayer}"/> to play <paramref name="self"/></returns>
        public static MonteCarloStrategySettings<TGame, TBoard, TMove, TPlayer> MonteCarloStrategySettings<TGame, TBoard, TMove, TPlayer>(this IGame<TGame, TBoard, TMove, TPlayer> self) where TGame : IGame<TGame, TBoard, TMove, TPlayer>
        {
            return Fx.Games.Strategy.MonteCarloStrategySettings<TGame, TBoard, TMove, TPlayer>.Default;
        }

        public static GameOfAmazonsConsoleStrategy<TPlayer> AmazonsConsoleStrategy<TPlayer>(this Amazons.Game<TPlayer> self)
        {
            return GameOfAmazonsConsoleStrategy<TPlayer>.Instance;
        }
    }
}
