namespace Fx.Games.CueLearning
{
    using Db.System.Collections.Generic;

    using Fx.Games.Game;

    public sealed class CueLearningTable<TGame, TBoard, TMove, TPlayer> where TGame : IGame<TGame, TBoard, TMove, TPlayer>
    {
        private readonly Db.System.Collections.Generic.Dictionary<TBoard, Db.System.Collections.Generic.Dictionary<TMove, double>> cueTable;

        private static string TranscribePiece(TicTacToePiece piece)
        {
            return piece == TicTacToePiece.Ex ? "X" : piece == TicTacToePiece.Oh ? "O" : ".";
        }

        public CueLearningTable(Db.System.Collections.Generic.Dictionary<TBoard, Db.System.Collections.Generic.Dictionary<TMove, double>> cueTable)
        {
            this.cueTable = cueTable;

            /*foreach (var board in this.cueTable)
            {
                for (int i = 0; i < 3; ++i)
                {
                    for (int j = 0; j < 3; ++j)
                    {
                        Console.Write(TranscribePiece(board.Key.Grid[i, j]));
                    }
                }
                Console.Write(": ");
                for (int i = 0; i < 9; ++i)
                {
                    var move = GetMove(i);
                    if (board.Value.TryGetValue(move, out var value))
                    {
                        Console.Write(" {0:F2}   ", value);
                    }
                    else
                    {
                        Console.Write("   /   ");
                    }
                }

                Console.WriteLine();
            }*/
        }

        public TMove? SelectMove(TBoard board)
        {
            if (!this.cueTable.TryGetValue(board, out var cueValues))
            {
                return default; //// TODO
            }

            TMove? move = default; //// TODO
            double? value = null;
            for (int i = 0; i < 9; ++i)
            {
                var currentMove = GetMove(i);
                if (cueValues.TryGetValue(currentMove, out var cueValue) && cueValue > (value ?? 0))
                {
                    move = currentMove;
                    value = cueValue;
                }
            }

            return move;
        }

        private TMove GetMove(int action)
        {
            ////return new TicTacToeMove((uint)(action / 3), (uint)(action % 3));
            //// TODO
            return default;
        }
    }
}
