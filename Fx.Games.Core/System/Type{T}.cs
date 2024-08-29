using Fx.Games.Displayer;
using Fx.Games.Driver;
using Fx.Games.Game;
using Fx.Games.Strategy;
using Db.System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;

namespace System
{
    public static class Utilities
    {
        public static IType<T> Type<T>()
        {
            return new Type<T>(typeof(T));
        }
    }

    public static class TypeExtensions
    {
        public static Driver<TGame, TBoard, TMove, TPlayer> Driver<TGame, TBoard, TMove, TPlayer>(this IType<IGame<TGame, TBoard, TMove, TPlayer>> type) where TGame : IGame<TGame, TBoard, TMove, TPlayer>
        {
            return new Driver<TGame, TBoard, TMove, TPlayer>(null, null, null);
        }

        public static DriverBuilder<TGame, TBoard, TMove, TPlayer> DriverBuilder<TGame, TBoard, TMove, TPlayer>(this IGame<TGame, TBoard, TMove, TPlayer> game) where TGame : IGame<TGame, TBoard, TMove, TPlayer>
        {
            return new DriverBuilder<TGame, TBoard, TMove, TPlayer>();
        }
    }

    public sealed class DriverBuilder<TGame, TBoard, TMove, TPlayer> where TGame : IGame<TGame, TBoard, TMove, TPlayer>
    {
        private readonly Dictionary<TPlayer, IStrategy<TGame, TBoard, TMove, TPlayer>>? strategies;

        private readonly IDisplayer<TGame, TBoard, TMove, TPlayer>? displayer;

        private readonly DriverSettings<TGame, TBoard, TMove, TPlayer> settings;

        public DriverBuilder()
            : this(null, null, DriverSettings<TGame, TBoard, TMove, TPlayer>.Default)
        {
        }

        private DriverBuilder(Dictionary<TPlayer, IStrategy<TGame, TBoard, TMove, TPlayer>>? strategies, IDisplayer<TGame, TBoard, TMove, TPlayer>? displayer, DriverSettings<TGame, TBoard, TMove, TPlayer> settings)
        {
            //// TODO make this constructor public?
            this.strategies = strategies;
            this.displayer = displayer;
            this.settings = settings;
        }

        public DriverBuilder<TGame, TBoard, TMove, TPlayer> AddStrategy(TPlayer player, IStrategy<TGame, TBoard, TMove, TPlayer> strategy)
        {
            if (strategy == null)
            {
                throw new ArgumentNullException(nameof(strategy));
            }

            //// TODO need player comparer
            var newStrategies = new Dictionary<TPlayer, IStrategy<TGame, TBoard, TMove, TPlayer>>();
            if (this.strategies != null)
            {
                foreach (var key in this.strategies.Keys)
                {
                    newStrategies.Add(key, this.strategies.GetValueTry(key, out _));
                }
            }

            newStrategies.Add(player, strategy);

            //// TODO make this a struct or a ref struct?
            return new DriverBuilder<TGame, TBoard, TMove, TPlayer>(newStrategies, this.displayer, this.settings);
        }

        public DriverBuilder<TGame, TBoard, TMove, TPlayer> AddDisplayer(IDisplayer<TGame, TBoard, TMove, TPlayer> displayer)
        {
            if (displayer == null)
            {
                throw new ArgumentNullException(nameof(displayer));
            }

            return new DriverBuilder<TGame, TBoard, TMove, TPlayer>(this.strategies, displayer, this.settings);
        }

        public DriverBuilder<TGame, TBoard, TMove, TPlayer> AddSettings(DriverSettings<TGame, TBoard, TMove, TPlayer> settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            return new DriverBuilder<TGame, TBoard, TMove, TPlayer>(this.strategies, this.displayer, settings);
        }

        public Driver<TGame, TBoard, TMove, TPlayer> Build()
        {
            if (this.strategies == null)
            {
                throw new ArgumentNullException(nameof(this.strategies));
            }

            if (this.displayer == null)
            {
                throw new ArgumentNullException(nameof(this.displayer));
            }

            return new Driver<TGame, TBoard, TMove, TPlayer>(this.strategies, this.displayer, this.settings);
        }
    }

    public static class Main
    {
        public static void DoWork()
        {
            Utilities.Type<TicTacToe<string>>().Driver();
        }
    }

    public interface IType<out T>
    {
        Type Typed { get; }
    }

    public sealed class Type<T> : IType<T>
    {
        private readonly Type type;

        public Type(Type type)
        {
            this.type = type;
        }

        public Type Typed
        {
            get
            {
                return this.type;
            }
        }
    }
}
