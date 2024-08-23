namespace Fx.Games.Strategy
{
    using System.Linq;

    using Fx.Games.CueLearning;
    using Fx.Games.Game;

    public sealed class CueLearningStrategy<TGame, TBoard, TMove, TPlayer> : IStrategy<TGame, TBoard, TMove, TPlayer> where TGame : IGame<TGame, TBoard, TMove, TPlayer>
    {
        private readonly CueLearningTable<TGame, TBoard, TMove, TPlayer> cueLearningTable;

        public CueLearningStrategy(CueLearningTable<TGame, TBoard, TMove, TPlayer> cueLearningTable)
        {
            this.cueLearningTable = cueLearningTable;
        }

        public TMove SelectMove(TGame game)
        {
            var move = this.cueLearningTable.SelectMove(game.Board);
            if (move == null) //// TODO this is always called for peggame
            {
                return game.Moves.First(); //// TODO we can probably avoid not having a moved selected by the table...
            }

            return move;
        }
    }
}
