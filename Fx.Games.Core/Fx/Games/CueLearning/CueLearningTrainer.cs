namespace Fx.Games.CueLearning
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using Db.System.Collections.Generic;
    using Fx.Games.Game;

    public sealed class CueLearningTrainer<TGame, TBoard, TMove, TPlayer> where TGame : IGame<TGame, TBoard, TMove, TPlayer>
    {
        private readonly Func<TGame> gameFactory;

        private readonly IEqualityComparer<TBoard> boardComparer;

        private readonly IEqualityComparer<TMove> moveComparer;

        private readonly TPlayer player;

        private readonly Random random;

        private readonly double explorationRate;

        private readonly double learningRate;

        private readonly double discountFactor;

        private readonly Db.System.Collections.Generic.Dictionary<TBoard, Db.System.Collections.Generic.Dictionary<TMove, double>> cueTable;

        public CueLearningTrainer(Func<TGame> gameFactory, TPlayer player)
            : this(gameFactory, player, CueLearningTrainerSettings<TGame, TBoard, TMove, TPlayer>.Default)
        {
        }

        public CueLearningTrainer(Func<TGame> gameFactory, TPlayer player, CueLearningTrainerSettings<TGame, TBoard, TMove, TPlayer> settings)
        {
            this.gameFactory = gameFactory;
            this.boardComparer = settings.BoardComparer;
            this.moveComparer = settings.MoveComparer;
            this.player = player;
            this.random = settings.Random;
            this.explorationRate = settings.ExplorationRate;
            this.learningRate = settings.LearningRate;
            this.discountFactor = settings.DiscountFactor;

            this.cueTable = new Db.System.Collections.Generic.Dictionary<TBoard, Db.System.Collections.Generic.Dictionary<TMove, double>>(this.boardComparer);
        }

        public CueLearningTable<TGame, TBoard, TMove, TPlayer> Train(int episodeCount)
        {
            //// TODO use the name "agent"
            //// TODO why is it called episode?
            for (int episode = 0; episode < episodeCount; ++episode)
            {
                var game = this.gameFactory();
                var sequences = new List<Tuple<TBoard, TMove>>();
                while (!game.IsGameOver)
                {
                    var board = game.Board;
                    var move = ChooseMove(game);

                    var sequence = Tuple.Create(board, move);
                    if (object.ReferenceEquals(game.CurrentPlayer, this.player)) //// TODO use a comparer
                    {
                        sequences.Add(sequence);
                    }

                    game = game.CommitMove(move);

                    if (game.IsGameOver)
                    {
                        var winners = game.WinnersAndLosers;
                        if (winners.Drawers.Contains(this.player))
                        {
                            ////UpdateCueTable(sequences, 0.5);
                            UpdateCueTable(game.Board, sequence.Item1, sequence.Item2, 0.5);
                        }
                        else if (winners.Winners.Contains(this.player)) //// TODO isn't this the opposite player that we want?
                        {
                            ////UpdateCueTable(sequences, 1);
                            UpdateCueTable(game.Board, sequence.Item1, sequence.Item2, 1);
                        }
                        else
                        {
                            ////UpdateCueTable(sequences, 1);
                            UpdateCueTable(game.Board, sequence.Item1, sequence.Item2, -1);
                        }
                    }
                    else
                    {
                        ////UpdateCueTable(sequences, 0);
                        UpdateCueTable(game.Board, sequence.Item1, sequence.Item2, 0);
                    }
                }
            }

            return new CueLearningTable<TGame, TBoard, TMove, TPlayer>(this.cueTable); //// TODO this should be a copy of the cuetable instead so that future training doesn't affect instances that have already been returned
        }

        private TMove ChooseMove(TGame game)
        {
            var board = game.Board;

            if (!this.cueTable.TryGetValue(board, out var cueValues))
            {
                cueValues = new Db.System.Collections.Generic.Dictionary<TMove, double>(this.moveComparer);
                this.cueTable.Add(board, cueValues);
            }

            var moves = game.Moves.ToList();
            if (this.random.NextDouble() < 0.1)
            {
                return moves.Sample(this.random);
            }
            else
            {
                //// TODO this will return null if there are no moves, but we shouldn't get to this line if there are no moves, right?
                return moves.MaxBy(move => cueValues.TryGetValue(move, out var value) ? value : 0);
            }
        }

        private TMove GetMove(int action)
        {
            //// TODO
            return default;
            ////return new TicTacToeMove((uint)(action / 3), (uint)(action % 3));
        }


        /*private void UpdateCueTable(List<Tuple<TicTacToeBoard, TicTacToeMove>> seuences, double reward)
        {
            foreach (var sequence in seuences)
            {
                UpdateCueTable(sequence.Item1, sequence.Item2, reward);
            }
        }*/

        private void UpdateCueTable(TBoard newBoard, TBoard oldBoard, TMove move, double reward)
        {
            if (!this.cueTable.TryGetValue(newBoard, out var cueValuesForNewBoard))
            {
                cueValuesForNewBoard = new Db.System.Collections.Generic.Dictionary<TMove, double>();
            }

            if (!this.cueTable.TryGetValue(oldBoard, out var cueValuesForOldBoard))
            {
                cueValuesForOldBoard = new Db.System.Collections.Generic.Dictionary<TMove, double>(this.moveComparer);
                this.cueTable.Add(oldBoard, cueValuesForOldBoard);
            }

            if (!cueValuesForOldBoard.TryGetValue(move, out var cueValue))
            {
                cueValue = 0;
            }

            var newValue = cueValue + 0.1 * (reward + 0.9 * MaxCueValue(cueValuesForNewBoard) - cueValue);
            if (newValue > 1.0)
            {
            }

            cueValuesForOldBoard.Index(move, newValue);
        }

        private double MaxCueValue(Db.System.Collections.Generic.Dictionary<TMove, double> cueValues)
        {
            var max = 0.0;
            foreach (var key in cueValues.Keys)
            {
                if (cueValues.TryGetValue(key, out var cueValue) && cueValue > max)
                {
                    max = cueValue;
                }
            }

            return max;
        }
    }
}
