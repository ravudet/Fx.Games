using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Games.Game.Snake
{
    public sealed class Board
    {
        public Board(int rows, int columns, IReadOnlyList<IReadOnlyList<Space>> grid)
        {
            this.Rows = rows;
            this.Columns = columns;

            this.Grid = grid.ToArray();
        }

        public int Rows { get; }

        public int Columns { get; }

        public IReadOnlyList<IReadOnlyList<Space>> Grid { get; }
    }
}
