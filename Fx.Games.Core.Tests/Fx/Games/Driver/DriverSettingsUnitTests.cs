namespace Fx.Games.Driver
{
    using System;
    using System.Collections.Generic;

    using Fx.Games.Game;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Unit tests for <see cref="DriverSettings{TGame, TBoard, TMove, TPlayer}"/>
    /// </summary>
    [TestClass]
    public sealed class DriverSettingsUnitTests
    {
        /// <summary>
        /// Creates default <see cref="DriverSettings{TGame, TBoard, TMove, TPlayer}"/>
        /// </summary>
        [TestMethod]
        public void DefaultBuilder()
        {
            var builder = new DriverSettings<MockGame, string[], string, string>.Builder()
            {
            };

            var settings = builder.Build();

            Assert.AreEqual(builder.PlayerTranscriber, settings.PlayerTranscriber);
        }

        /// <summary>
        /// Creates default <see cref="DriverSettings{TGame, TBoard, TMove, TPlayer}"/>
        /// </summary>
        [TestMethod]
        public void DefaultSettings()
        {
            var builder = new DriverSettings<MockGame, string[], string, string>.Builder()
            {
            };

            var settings = builder.Build();

            Assert.AreEqual(
                DriverSettings<MockGame, string[], string, string>.Default.PlayerTranscriber, 
                settings.PlayerTranscriber);
        }

        /// <summary>
        /// Creates <see cref="DriverSettings{TGame, TBoard, TMove, TPlayer}"/> with a <see langword="null"/> player transcriber
        /// </summary>
        [TestMethod]
        public void NullPlayerTranscriber()
        {
            var builder = new DriverSettings<MockGame, string[], string, string>.Builder()
            {
                PlayerTranscriber = null,
            };

            Assert.ThrowsException<ArgumentNullException>(() => builder.Build());
        }

        /// <summary>
        /// A mock implementation of <see cref="IGame{TGame, TBoard, TMove, TPlayer}"/> that is not implemented
        /// </summary>
        private sealed class MockGame : IGame<MockGame, string[], string, string>
        {
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
            public MockGame CommitMove(string move)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
