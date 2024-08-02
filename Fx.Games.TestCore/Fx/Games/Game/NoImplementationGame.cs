namespace Fx.Games.Game
{
    using System.Collections.Generic;

    /// <summary>
    /// A mock implementation of <see cref="IGame{TGame, TBoard, TMove, TPlayer}"/> that is not implemented
    /// </summary>
    public sealed class NoImplementationGame : IGame<NoImplementationGame, string[], string, string>
    {
        /// <summary>
        /// The singleton instance of <see cref="NoImplementationGame"/>
        /// </summary>
        public static NoImplementationGame Instance { get; } = new NoImplementationGame();

        /// <inheritdoc/>
        public string CurrentPlayer => throw new System.NotImplementedException();

        /// <inheritdoc/>
        public IEnumerable<string> Moves => throw new System.NotImplementedException();

        /// <inheritdoc/>
        public string[] Board => throw new System.NotImplementedException();

        /// <inheritdoc/>
        public WinnersAndLosers<string> WinnersAndLosers => throw new System.NotImplementedException();

        /// <inheritdoc/>
        public bool IsGameOver => throw new System.NotImplementedException();

        /// <inheritdoc/>
        public NoImplementationGame CommitMove(string move)
        {
            throw new System.NotImplementedException();
        }
    }
}
