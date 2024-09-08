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

        private readonly int snakeLength;

        private readonly (int row, int column) foodPosition;

        public Game(TPlayer player)
            : this(player, new Random())
        {
        }

        public Game(TPlayer player, Random random)
            : this(
                  player, 
                  random,
                  GenerateBoard(20, 40, random, new[] { (10, 20), (11, 20) }, Space.UpHead, (17, 30)).Item1,
                  new WinnersAndLosers<TPlayer>(Enumerable.Empty<TPlayer>(), Enumerable.Empty<TPlayer>(), Enumerable.Empty<TPlayer>()),
                  false,
                  new[] { (10, 20), (11, 20) },
                  2,
                  (17, 30))
        {
        }

        private Game(
            TPlayer player, 
            Random random,
            Board board, 
            WinnersAndLosers<TPlayer> winnersAndLosers, 
            bool isGameOver,
            IEnumerable<(int row, int column)> snake,
            int snakeLength,
            (int row, int column) foodPosition)
        {
            this.CurrentPlayer = player;
            this.random = random;
            this.Board = board;
            this.WinnersAndLosers = winnersAndLosers;
            this.IsGameOver = isGameOver;
            this.snake = snake;
            this.snakeLength = snakeLength;
            this.foodPosition = foodPosition;
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
                    this.snake,
                    this.snakeLength,
                    this.foodPosition);
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
                var newBoard = GenerateBoard(this.Board.Rows, this.Board.Columns, this.random, this.snake, orientation, this.foodPosition);

                // we ran into a ourselves
                return new Game<TPlayer>(
                    this.CurrentPlayer,
                    this.random,
                    newBoard.Item1,
                    new WinnersAndLosers<TPlayer>(Enumerable.Empty<TPlayer>(), new[] { this.CurrentPlayer }, Enumerable.Empty<TPlayer>()),
                    true,
                    this.snake,
                    this.snakeLength,
                    newBoard.Item2);
            }

            if (targetSpace == Space.Empty)
            {
                var newSnake = this.snake
                    .Prepend((snakeHead.row + direction.row, snakeHead.column + direction.column))
                    .Take(this.snakeLength);
                var newBoard = GenerateBoard(this.Board.Rows, this.Board.Columns, this.random, newSnake, orientation, this.foodPosition);
                return new Game<TPlayer>(
                    this.CurrentPlayer,
                    this.random,
                    newBoard.Item1,
                    new WinnersAndLosers<TPlayer>(Enumerable.Empty<TPlayer>(), Enumerable.Empty<TPlayer>(), Enumerable.Empty<TPlayer>()),
                    false,
                    newSnake,
                    this.snakeLength,
                    newBoard.Item2);
            }

            if (targetSpace == Space.Food)
            {
                var newSnake = this.snake.Prepend((snakeHead.row + direction.row, snakeHead.column + direction.column));
                var newBoard = GenerateBoard(this.Board.Rows, this.Board.Columns, this.random, newSnake, orientation,
                    null);
                return new Game<TPlayer>(
                    this.CurrentPlayer,
                    this.random,
                    newBoard.Item1,
                    new WinnersAndLosers<TPlayer>(Enumerable.Empty<TPlayer>(), Enumerable.Empty<TPlayer>(), Enumerable.Empty<TPlayer>()),
                    false,
                    newSnake,
                    this.snakeLength + 1,
                    newBoard.Item2);
            }

            throw new Exception("TODO");
        }

        private static (Board, (int row, int column)) GenerateBoard(int rows, int columns, Random random, IEnumerable<(int row, int column)> snake, Space snakeOrientation, (int row, int column)? foodPosition)
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

            if (foodPosition == null)
            {
                while (true)
                {
                    var row = random.Next(rows);
                    var column = random.Next(columns);
                    if (spaces[row][column] == Space.Empty)
                    {
                        spaces[row][column] = Space.Food;
                        foodPosition = (row, column);
                        break;
                    }
                }
            }
            else
            {
                spaces[foodPosition.Value.row][foodPosition.Value.column] = Space.Food;
            }

            return (new Board(rows, columns, spaces), foodPosition.Value);
        }
    }
}
