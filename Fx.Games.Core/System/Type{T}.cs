using Fx.Games.Driver;
using Fx.Games.Game;
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
        public Driver<TGame, TBoard, TMove, TPlayer> Build()
        {
            return new Driver<TGame, TBoard, TMove, TPlayer>(null, null, null);
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
