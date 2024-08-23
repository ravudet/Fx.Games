namespace Fx.Games.CueLearning
{
    using System;
    using System.Collections.Generic;
    using Fx.Games.Game;

    public sealed class CueLearningTrainerSettings<TGame, TBoard, TMove, TPlayer> where TGame : IGame<TGame, TBoard, TMove, TPlayer>
    {
        private CueLearningTrainerSettings(Random random, double explorationRate, double learningRate, double discountFactor, IEqualityComparer<TBoard> boardComparer, IEqualityComparer<TMove> moveComparer)
        {
            //// TODO add a builder
            this.Random = random;
            this.ExplorationRate = explorationRate;
            this.LearningRate = learningRate;
            this.DiscountFactor = discountFactor;
            this.BoardComparer = boardComparer;
            this.MoveComparer = moveComparer;
        }

        public static CueLearningTrainerSettings<TGame, TBoard, TMove, TPlayer> Default = new CueLearningTrainerSettings<TGame, TBoard, TMove, TPlayer>(new Random(), 0.1, 0.1, 0.9, EqualityComparer<TBoard>.Default, EqualityComparer<TMove>.Default);

        public Random Random { get; }

        public double ExplorationRate { get; } //// TODO this is a rate but is expressed as a double

        public double LearningRate { get; } //// TODO this is a rate but is expressed as a double

        public double DiscountFactor { get; } //// TODO why is this a double? is it also a rate?

        public IEqualityComparer<TBoard> BoardComparer { get; }

        public IEqualityComparer<TMove> MoveComparer { get; }

        public sealed class Builder
        {
            public IEqualityComparer<TBoard> BoardComparer { get; set; } = CueLearningTrainerSettings<TGame, TBoard, TMove, TPlayer>.Default.BoardComparer;

            public IEqualityComparer<TMove> MoveComparer { get; set; } = CueLearningTrainerSettings<TGame, TBoard, TMove, TPlayer>.Default.MoveComparer;

            public CueLearningTrainerSettings<TGame, TBoard, TMove, TPlayer> Build()
            {
                //// TODO

                return new CueLearningTrainerSettings<TGame, TBoard, TMove, TPlayer>(Default.Random, Default.ExplorationRate, Default.LearningRate, Default.DiscountFactor, this.BoardComparer, this.MoveComparer);
            }
        }
    }
}