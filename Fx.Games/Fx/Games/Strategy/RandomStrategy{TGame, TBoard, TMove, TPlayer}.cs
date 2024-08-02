namespace Fx.Games.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Fx.Games.Game;

    public sealed class RandomStrategy<TGame, TBoard, TMove, TPlayer> : IStrategy<TGame, TBoard, TMove, TPlayer> where TGame : IGame<TGame, TBoard, TMove, TPlayer>
    {
        private readonly Random rng;

        public RandomStrategy()
        {
            rng = new Random();
        }

        public TMove SelectMove(TGame game)
        {

            var moves = game.Moves.ToList();
            var move = moves.Sample(this.rng);
            return move;
        }
    }
}
