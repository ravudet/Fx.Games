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
            var builder = new DriverSettings<NoImplementationGame, string[], string, string>.Builder()
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
            var builder = new DriverSettings<NoImplementationGame, string[], string, string>.Builder()
            {
            };

            var settings = builder.Build();

            Assert.AreEqual(
                DriverSettings<NoImplementationGame, string[], string, string>.Default.PlayerTranscriber, 
                settings.PlayerTranscriber);
        }

        /// <summary>
        /// Creates <see cref="DriverSettings{TGame, TBoard, TMove, TPlayer}"/> with a <see langword="null"/> player transcriber
        /// </summary>
        [TestMethod]
        public void NullPlayerTranscriber()
        {
            var builder = new DriverSettings<NoImplementationGame, string[], string, string>.Builder()
            {
                PlayerTranscriber = null,
            };

            Assert.ThrowsException<ArgumentNullException>(() => builder.Build());
        }
    }
}
