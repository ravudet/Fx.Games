namespace Fx.Games.Game
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A representation of the board state of a <see cref="PegGame{TPlayer}"/>
    /// </summary>
    public sealed class PegBoard
    {
        private readonly Peg[][] triangle;

        /// <summary>
        /// Initializes a new instance of the <see cref="PegBoard"/> class
        /// </summary>
        /// <param name="blanks">The spaces within the triangle of pegs that should not contain a peg</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="blanks"/> is <see langword="null"/></exception>
        public PegBoard(IEnumerable<PegPosition> blanks)
        {
            if (blanks == null)
            {
                throw new ArgumentNullException(nameof(blanks));
            }

            var height = 5;

            this.triangle = new Peg[height][];
            for (int i = 0; i < height; ++i)
            {
                this.triangle[i] = new Peg[i + 1];
            }

            foreach (var blank in blanks)
            {
                this.triangle[blank.Row][blank.Column] = Peg.Empty;
            }
        }

        /// <summary>
        /// The triangle of pegs that is manipulated while playing <see cref="PegGame{TPlayer}"/>
        /// </summary>
        public Peg[][] Triangle
        {
            get
            {
                return this.triangle.Select(row => (row.Clone() as Peg[])!).ToArray();
            }
        }
    }
}
