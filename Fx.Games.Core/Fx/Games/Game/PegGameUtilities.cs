namespace Fx.Games.Game
{
    using System.Collections.Generic;

    /// <summary>
    /// Utilities for the <see cref="PegGame{TPlayer}"/> class
    /// </summary>
    public static class PegGameUtilities
    {
        /// <summary>
        /// A winning sequence of moves for the <see cref="PegGame{TPlayer}"/>
        /// </summary>
        public static IReadOnlyList<PegMove> WinningSequence { get; } = new List<PegMove>()
        {
            new PegMove(new PegPosition(2, 0), new PegPosition(0, 0)),
            new PegMove(new PegPosition(2, 2), new PegPosition(2, 0)),
            new PegMove(new PegPosition(0, 0), new PegPosition(2, 2)),
            new PegMove(new PegPosition(3, 0), new PegPosition(1, 0)),
            new PegMove(new PegPosition(4, 3), new PegPosition(2, 0)),
            new PegMove(new PegPosition(1, 0), new PegPosition(3, 0)),
            new PegMove(new PegPosition(3, 3), new PegPosition(3, 1)),
            new PegMove(new PegPosition(3, 0), new PegPosition(3, 2)),
            new PegMove(new PegPosition(4, 4), new PegPosition(4, 2)),
            new PegMove(new PegPosition(4, 1), new PegPosition(4, 3)),
            new PegMove(new PegPosition(2, 2), new PegPosition(4, 2)),
            new PegMove(new PegPosition(4, 3), new PegPosition(4, 1)),
            new PegMove(new PegPosition(4, 0), new PegPosition(4, 2)),
        };
    }
}
