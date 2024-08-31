namespace Fx.Games.Game.Chess.Tests
{
    using System;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Unit tests for <see cref="Chess.Board" move generation/>
    /// </summary>
    [TestClass]
    public sealed class ChessBoardUnitTests
    {

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
            var board = new Board();
            var player = Color.White;

            var moves = pgnGame.Split(' ');
            for (int i = 0; i < moves.Length; i++)
            {
                string? part = moves[i];
                if (System.Text.RegularExpressions.Regex.IsMatch(part, @"[0-9]+\."))
                {
                    continue;
                }
                var pgn = PgnMove.Parse(part, null);

                var move = board
                    .GetMoves(player)
                    .Where(m => pgn.Matches(m, board))
                    .SingleOrDefault();

                Assert.IsNotNull(move, $"can't find a matching move {i} {part}");

                // Console.WriteLine("{0} {1,-7} -> {2:L} on {3} moves to {4}{5}", player, part, board[move.Start], move.Start, move.End, move.Capture ? $" capturing {board[move.End].Kind}" : "");
                board = board.Apply(move);
                player = player == Color.White ? Color.Black : Color.White;
            }
        }
    }



}
