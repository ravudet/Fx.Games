using Fx.Games.Game;
using System;

namespace Fx.Games.Driver
{
    /// <summary>
    /// The settings used to instantiate a <see cref="Driver{TGame, TBoard, TMove, TPlayer}"/>
    /// </summary>
    public sealed class DriverSettings<TGame, TBoard, TMove, TPlayer> where TGame : IGame<TGame, TBoard, TMove, TPlayer>
    {
        /// <summary>
        /// TODO
        /// </summary>
        private DriverSettings()
        {
        }

        /// <summary>
        /// Converts a <see cref="TPlayer"/> to a string for logging and error handling
        /// </summary>
        public Func<TPlayer, string> PlayerTranscriber { get; }

        /// <summary>
        /// 
        /// </summary>
        public sealed class Builder
        {
            /// <summary>
            /// Converts a <see cref="TPlayer"/> to a string for logging and error handling
            /// </summary>
            public Func<TPlayer, string> PlayerTranscriber { get; set; } = player => $"{player}";

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            /// <exception cref="ArgumentNullException"></exception>
            public DriverSettings<TGame, TBoard, TMove, TPlayer> Build()
            {
                if (this.PlayerTranscriber == null)
                {
                    throw new ArgumentNullException(nameof(this.PlayerTranscriber));
                }

                return new DriverSettings<TGame, TBoard, TMove, TPlayer>();
            }
        }
    }
}
