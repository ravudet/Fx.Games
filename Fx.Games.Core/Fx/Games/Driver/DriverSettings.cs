namespace Fx.Games.Driver
{
    using System;

    using Fx.Games.Game;

    /// <summary>
    /// The settings used to instantiate a <see cref="Driver{TGame, TBoard, TMove, TPlayer}"/>
    /// </summary>
    public sealed class DriverSettings<TGame, TBoard, TMove, TPlayer> where TGame : IGame<TGame, TBoard, TMove, TPlayer>
    {
        /// <summary>
        /// Prevents the initialization of the <see cref="DriverSettings{TGame, TBoard, TMove, TPlayer}"/> class
        /// </summary>
        /// <param name="playerTranscriber">Converts a <see cref="TPlayer"/> to a string for logging and error handling</param>
        private DriverSettings(Func<TPlayer, string> playerTranscriber)
        {
            this.PlayerTranscriber = playerTranscriber;
        }

        /// <summary>
        /// The default instance of <see cref="DriverSettings{TGame, TBoard, TMove, TPlayer}"/>
        /// </summary>
        public static DriverSettings<TGame, TBoard, TMove, TPlayer> Default { get; } = 
            new DriverSettings<TGame, TBoard, TMove, TPlayer>(
                player => $"{player}");

        /// <summary>
        /// Converts a <see cref="TPlayer"/> to a string for logging and error handling
        /// </summary>
        public Func<TPlayer, string> PlayerTranscriber { get; }

        /// <summary>
        /// A builder for <see cref="DriverSettings{TGame, TBoard, TMove, TPlayer}"/>
        /// </summary>
        public sealed class Builder
        {
            /// <summary>
            /// Converts a <see cref="TPlayer"/> to a string for logging and error handling
            /// </summary>
            public Func<TPlayer, string> PlayerTranscriber { get; set; } = Default.PlayerTranscriber;

            /// <summary>
            /// Creates a new instance of <see cref="DriverSettings{TGame, TBoard, TMove, TPlayer}"/> from the properties configured on this <see cref="Builder"/>
            /// </summary>
            /// <returns>The new instance of <see cref="DriverSettings{TGame, TBoard, TMove, TPlayer}"/></returns>
            /// <exception cref="ArgumentNullException">Thrown if <see cref="Builder.PlayerTranscriber"/> is <see langword="null"/></exception>
            public DriverSettings<TGame, TBoard, TMove, TPlayer> Build()
            {
                if (this.PlayerTranscriber == null)
                {
                    throw new ArgumentNullException(nameof(this.PlayerTranscriber));
                }

                return new DriverSettings<TGame, TBoard, TMove, TPlayer>(this.PlayerTranscriber);
            }
        }
    }
}
