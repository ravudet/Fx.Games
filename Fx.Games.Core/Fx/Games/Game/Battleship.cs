using Fx.Distribution;
using System.Collections.Generic;

namespace Fx.Games.Game
{
    public sealed class Battleship<TPlayer> : IGame<Battleship<TPlayer>, BattleshipBoard, BattleshipMove, TPlayer, WeightedDistribution<Battleship<TPlayer>>>
    {
        private readonly (TPlayer Player, BattleshipBoard Board) current;

        private readonly (TPlayer Player, BattleshipBoard Board) other;

        public Battleship(TPlayer first, TPlayer second)
        {
            this.current = (first, new BattleshipBoard.Placement());
            this.other = (second, new BattleshipBoard.Placement());
        }

        public TPlayer CurrentPlayer
        {
            get
            {
                return this.current.Player;
            }
        }

        public BattleshipBoard Board
        {
            get
            {
                //// TODO having multiple derived types for battleshipboard may make it difficult to call this property...
                return this.current.Board;
            }
        }

        public WinnersAndLosers<TPlayer> WinnersAndLosers => throw new System.NotImplementedException();

        public bool IsGameOver => throw new System.NotImplementedException();

        public IEnumerable<BattleshipMove> Moves => throw new System.NotImplementedException();

        public Battleship<TPlayer> CommitMove(BattleshipMove move)
        {
            throw new System.NotImplementedException();
        }

        public WeightedDistribution<Battleship<TPlayer>> ExploreMove(BattleshipMove move)
        {
            if (NoHitsYet())
            {
                var numberOfShipSpaces = 10; //// TODO not the right number, just used for illustration
                var totalNumberOfSpaces = 50; //// TODO not the right number, just used for illustration

                var range = Fx.Range.Range
                    .Instance(Fx.Numerics.Naturals._0, Fx.Numerics.Naturals._1, default(Battleship<TPlayer>)) //// TODO should be a game instance that indicates a hit
                    .FollowedBy(Fx.Numerics.Naturals._5, default(Battleship<TPlayer>)); //// TODO should be a game instance that indicates a miss

                var segment = SegmentV2<Battleship<TPlayer>>.Create(range); //// TODO not sure why the compiler thinks `range` is nullable here...

                return new WeightedDistribution<Battleship<TPlayer>>(new UniformDistribution(new System.Random()), segment);
            }

            throw new System.Exception("TODO");
        }

        private static bool NoHitsYet()
        {
            return false;
        }
    }
}
