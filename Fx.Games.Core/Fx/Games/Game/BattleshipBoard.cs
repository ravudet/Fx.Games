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
            private readonly Shot[,] shots;

            public Discovery(int rows, int columns)
            {
                this.shots = new Shot[rows, columns];
            }

            public Discovery(Shot[,] shots)
            {
                this.shots = shots;
            }

            public int Rows
            {
                get
                {
                    return this.shots.GetLength(0);
                }
            }

            public int Columns
            {
                get
                {
                    return this.shots.GetLength(1);
                }
            }

            public Shot Get(int row, int column)
            {
                return this.shots[row, column];
            }
        }
    }

    public abstract class Shot
    {
        public sealed class Empty : Shot
        {
        }

        public sealed class Miss : Shot
        {
        }

        public sealed class Hit : Shot
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
