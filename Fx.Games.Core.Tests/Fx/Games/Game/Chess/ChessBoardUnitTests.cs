namespace Fx.Games.Game.Chess.Tests
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Unit tests for <see cref="Chess.Board" move generation/>
    /// </summary>
    [TestClass]
    public sealed class ChessBoardUnitTests
    {

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            Trace.Listeners.Add(new ConsoleTraceListener());
        }

        private static readonly System.Text.RegularExpressions.Regex roundRegex = new Regex(@"[0-9]+\.");

        [TestMethod]
        [DataRow("1. c4 e6 2. d4 d5 3. Nf3 Nf6 4. e3 Be7 5. Bd3 O-O 6. b3 b6 7. O-O Bb7 8. Bb2 Nbd7 9. Nc3 a6 10. Rc1 Bd6 11. cxd5 exd5 12. Ne2 Re8 13. Ng3 Ne4 14. Qc2 Nxg3 15. hxg3 Nf6 16. Ne5 Qe7 17. b4 Ne4 18. b5 axb5 19. Bxb5 Rec8 20. Nc6 Qg5 21. a4 h5 22. Qe2 h4 23. gxh4 Qxh4 24. g3")]
        /// <summary>
        /// parses the given game in PGN notation 
        /// for each PGN move:
        ///     ensures each move can be found in the list of legal moves,
        ///     and applies the moves to a board
        /// </summary>
        public void GameScenarioTest(string pgnGame)
        {

            // Trace.WriteLine($"squares: {string.Join(" ", board.GetSquaresOfColor(player))}");
            var pgnMoves = pgnGame.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            var board = new Board();
            var player = Color.White;

            for (int i = 0; i < pgnMoves.Length; i += 1)
            {
                string? part = pgnMoves[i];
                if (roundRegex.IsMatch(part))
                {
                    continue;
                }

                var pgn = PgnMove.Parse(part, null);
                var availableMoves = board.GetMoves(player).ToArray()!;
                Trace.WriteLine($"moves {string.Join(";", availableMoves.Select(m => m.ToString()))}");
                var move = availableMoves
                    .Where(m => pgn.Matches(m, board))
                    .SingleOrDefault();

                Assert.IsNotNull(move, $"can't find a matching move {player}: {part} in [{string.Join(", ", availableMoves.Select(m => $"{board[m.Start].Symbol} {m}"))}]");

                board = board.CommitMove(move);
                player = player == Color.White ? Color.Black : Color.White;
            }
        }
    }



}
