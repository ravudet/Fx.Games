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

        IEnumerable<BattleshipMove> IGame<Battleship<TPlayer>, BattleshipBoard, BattleshipMove, TPlayer, WeightedDistribution<Battleship<TPlayer>>>.Moves => throw new System.NotImplementedException();

        Battleship<TPlayer> IGame<Battleship<TPlayer>, BattleshipBoard, BattleshipMove, TPlayer, WeightedDistribution<Battleship<TPlayer>>>.CommitMove(BattleshipMove move)
        {
            throw new System.NotImplementedException();
        }

        WeightedDistribution<Battleship<TPlayer>> IGame<Battleship<TPlayer>, BattleshipBoard, BattleshipMove, TPlayer, WeightedDistribution<Battleship<TPlayer>>>.ExploreMove(BattleshipMove move)
        {
            throw new System.NotImplementedException();
        }
    }
}
