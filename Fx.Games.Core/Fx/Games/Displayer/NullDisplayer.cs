namespace Fx.Games.Displayer
{
    using Fx.Games.Game;

    /// <summary>
    /// A <see cref="IDisplayer{TGame, TBoard, TMove, TPlayer}"/> that performs no-ops for each of the display methods
    /// </summary>
    /// <typeparam name="TGame">The type of the game that is being displayed</typeparam>
    /// <typeparam name="TBoard">The type of the board that the <typeparamref name="TGame"/> uses</typeparam>
    /// <typeparam name="TMove">The type of the moves that the <typeparamref name="TGame"/> uses</typeparam>
    /// <typeparam name="TPlayer">The type of the player that is playing the <typeparamref name="TGame"/></typeparam>
    public sealed class NullDisplayer<TGame, TBoard, TMove, TPlayer> : IDisplayer<TGame, TBoard, TMove, TPlayer> where TGame : IGame<TGame, TBoard, TMove, TPlayer>
    {
        /// <summary>
        /// Prevents the initialization of the <see cref="NullDisplayer{TGame, TBoard, TMove, TPlayer}"/> class
        /// </summary>
        private NullDisplayer()
        {
        }

        /// <summary>
        /// The singleton instance of <see cref="NullDisplayer{TGame, TBoard, TMove, TPlayer}"/>
        /// </summary>
        public static NullDisplayer<TGame, TBoard, TMove, TPlayer> Instance { get; } = new NullDisplayer<TGame, TBoard, TMove, TPlayer>();

        /// <inheritdoc/>
        public void DisplayAvailableMoves(TGame game)
        {
        }

        /// <inheritdoc/>
        public void DisplayBoard(TGame game)
        {
        }

        /// <inheritdoc/>
        public void DisplayOutcome(TGame game)
        {
        }

        /// <inheritdoc/>
        public void DisplaySelectedMove(TMove move)
        {
        }
    }
}
