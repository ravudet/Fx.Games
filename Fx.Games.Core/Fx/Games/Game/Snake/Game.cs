namespace Fx.Games.Game.Snake
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Numerics;
    using System.Reflection;

    public sealed class Game<TPlayer> : IGame<Game<TPlayer>, Board, Move, TPlayer>
    {
        private readonly Random random;

        private readonly IEnumerable<(int row, int column)> snake;

        public Game(TPlayer player)
            : this(player, new Random())
        {
        }

        public Game(TPlayer player, Random random)
            : this(
                  player, 
                  random,
                  GenerateBoard(20, 40, random, new[] { (10, 20), (11, 20) }, true, Space.UpHead),
                  new WinnersAndLosers<TPlayer>(Enumerable.Empty<TPlayer>(), Enumerable.Empty<TPlayer>(), Enumerable.Empty<TPlayer>()),
                  false,
                  new[] { (10, 20), (11, 20) })
        {
        }

        private Game(
            TPlayer player, 
            Random random,
            Board board, 
            WinnersAndLosers<TPlayer> winnersAndLosers, 
            bool isGameOver,
            IEnumerable<(int row, int column)> snake)
        {
            this.CurrentPlayer = player;
            this.random = random;
            this.Board = board;
            this.WinnersAndLosers = winnersAndLosers;
            this.IsGameOver = isGameOver;
            this.snake = snake;
        }

        public TPlayer CurrentPlayer { get; }

        public IEnumerable<Move> Moves
        {
            get
            {
                var snakeHead = this.snake.First();
                var orientation = this.Board.Grid[snakeHead.row][snakeHead.column];
                if (orientation == Space.UpHead)
                {
                    yield return Move.Up;
                    yield return Move.Left;
                    yield return Move.Right;

                    yield break;
                }


                if (orientation == Space.DownHead)
                {
                    yield return Move.Down;
                    yield return Move.Left;
                    yield return Move.Right;

                    yield break;
                }


                if (orientation == Space.LeftHead)
                {
                    yield return Move.Up;
                    yield return Move.Down;
                    yield return Move.Left;

                    yield break;
                }


                if (orientation == Space.RightHead)
                {
                    yield return Move.Up;
                    yield return Move.Down;
                    yield return Move.Right;

                    yield break;
                }
            }
        }

        public Board Board { get; }

        public WinnersAndLosers<TPlayer> WinnersAndLosers { get; }

        public bool IsGameOver { get; }

        public Game<TPlayer> CommitMove(Move move)
        {
            var snakeHead = this.snake.First();

            if (move == Move.Up && snakeHead.row == 0 ||
                move == Move.Down && snakeHead.row == this.Board.Rows - 1 ||
                move == Move.Left && snakeHead.column == 0||
                move == Move.Right && snakeHead.column == this.Board.Columns - 1)
            {
                // we ran into a wall
                return new Game<TPlayer>(
                    this.CurrentPlayer, 
                    this.random,
                    null, 
                    new WinnersAndLosers<TPlayer>(Enumerable.Empty<TPlayer>(), new[] { this.CurrentPlayer }, Enumerable.Empty<TPlayer>()), 
                    true,
                    this.snake);
            }

            (int row, int column) direction;
            Space orientation;
            if (move == Move.Up)
            {
                direction = (-1, 0);
                orientation = Space.UpHead;
            }
            else if (move == Move.Down)
            {
                direction = (1, 0);
                orientation = Space.DownHead;
            }
            else if (move == Move.Left)
            {
                direction = (0, -1);
                orientation = Space.LeftHead;
            }
            else if (move == Move.Right)
            {
                direction = (0, 1);
                orientation = Space.RightHead;
            }
            else
            {
                throw new Exception("tODO");
            }

            var targetSpace = this.Board.Grid[snakeHead.row + direction.row][snakeHead.column + direction.column];
            if (targetSpace >= Space.Snake && targetSpace <= Space.RightHead)
            {
                // we ran into a ourselves
                return new Game<TPlayer>(
                    this.CurrentPlayer,
                    this.random,
                    GenerateBoard(this.Board.Rows, this.Board.Columns, this.random, this.snake, false, orientation),
                    new WinnersAndLosers<TPlayer>(Enumerable.Empty<TPlayer>(), new[] { this.CurrentPlayer }, Enumerable.Empty<TPlayer>()),
                    true,
                    this.snake);
            }

            if (targetSpace == Space.Empty)
            {
                var newSnake = this.snake.Select(position => (position.row + direction.row, position.column + direction.column));
                return new Game<TPlayer>(
                    this.CurrentPlayer,
                    this.random,
                    GenerateBoard(this.Board.Rows, this.Board.Columns, this.random, newSnake, false, orientation),
                    new WinnersAndLosers<TPlayer>(Enumerable.Empty<TPlayer>(), Enumerable.Empty<TPlayer>(), Enumerable.Empty<TPlayer>()),
                    false,
                    newSnake);
            }

            if (targetSpace == Space.Food)
            {
                var newSnake = this.snake.Prepend((snakeHead.row + direction.row, snakeHead.column + direction.column));
                return new Game<TPlayer>(
                    this.CurrentPlayer,
                    this.random,
                    GenerateBoard(this.Board.Rows, this.Board.Columns, this.random, newSnake, true, orientation),
                    new WinnersAndLosers<TPlayer>(Enumerable.Empty<TPlayer>(), Enumerable.Empty<TPlayer>(), Enumerable.Empty<TPlayer>()),
                    false,
                    newSnake);
            }

            throw new Exception("TODO");
        }

        private static Board GenerateBoard(int rows, int columns, Random random, IEnumerable<(int row, int column)> snake, bool newFood, Space snakeOrientation)
        {
            var spaces = new Space[rows][];
            for (int i = 0; i < rows; ++i)
            {
                spaces[i] = new Space[columns];
            }

            foreach (var snakeSpace in snake)
            {
                spaces[snakeSpace.row][snakeSpace.column] = Space.Snake;
            }

            var snakeHead = snake.First();
            spaces[snakeHead.row][snakeHead.column] = snakeOrientation;

            if (newFood)
            {
                while (true)
                {
                    var row = random.Next(rows);
                    var column = random.Next(columns);
                    if (spaces[row][column] == Space.Empty)
                    {
                        spaces[row][column] = Space.Food;
                        break;
                    }
                }
            }

            return new Board(rows, columns, spaces);
        }
    }
}
