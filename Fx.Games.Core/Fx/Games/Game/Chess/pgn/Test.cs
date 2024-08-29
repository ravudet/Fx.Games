

namespace Fx.Games.Game.Chess
{
    using System;
    using System.Data;
    using System.Text.RegularExpressions;
    using Fx.Games.Game.Chess;
    using Microsoft.Extensions.Primitives;

    Tes class Chess
    {

        static void Test()
        {
            const string game = """1. c4 e6 2. d4 d5 3. Nf3 Nf6 4. e3 Be7 5. Bd3 O-O 6. b3 b6 7. O-O Bb7 8. Bb2 Nbd7 9. Nc3 a6 10. Rc1 Bd6 11. cxd5 exd5 12. Ne2 Re8 13. Ng3 Ne4 14. Qc2 Nxg3 15. hxg3 Nf6 16. Ne5 Qe7 17. b4 Ne4 18. b5 axb5 19. Bxb5 Rec8 20. Nc6 Qg5 21. a4 h5 22. Qe2 h4 23. gxh4 Qxh4 24. g3""";

            var board = new Board();
            var player = Color.White;

            var moves = game.Split(' ');// .Where(m => !Regex.IsMatch(m, @"[0-9]+\.")).ToArray();
            foreach (var part in moves)
            {
                if (Regex.IsMatch(part, @"[0-9]+\."))
                {
                    Console.WriteLine(part);
                    continue;
                }
                var pgn = PgnMove.Parse(part, null);

                try
                {
                    var move = board.GetMoves(player).Where(m => pgn.Matches(m, board)).Single();

                    Console.WriteLine("{0,-7} -> {1} {2}", part, board[move.From], move);
                    board = board.Apply(move);
                    player = player == Color.White ? Color.Black : Color.White;
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine("can't find a move matching {0}", part);
                    break;
                }
            }

        }
    }
}