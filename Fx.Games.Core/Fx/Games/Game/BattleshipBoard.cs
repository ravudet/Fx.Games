using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Games.Game
{
    public abstract class BattleshipBoard
    {
        private BattleshipBoard()
        {
        }

        public sealed class Placement : BattleshipBoard
        {
            public Placement()
            {
            }

            public Placement Place(Ship ship, (int X, int Y) location, Direction direction)
            {
                throw new Exception("TODO");
            }

            public Discovery Complete()
            {
                throw new Exception("TODO");
            }
        }

        public sealed class Discovery : BattleshipBoard
        {
        }
    }

    public abstract class Direction
    {
        private Direction()
        {
        }
    }

    public abstract class Ship
    {
        private Ship()
        {
        }
    }
}
