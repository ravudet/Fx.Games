namespace Fx.Games.Game
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Db.System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public interface IDbEnumerable<out T>
    {
        T Current { get; }

        IDbEnumerable<T> TryMoveNext(out bool moved);
    }

    public sealed class DbList<T> : IDbEnumerable<T>
    {
        private readonly List<T> list;

        private readonly int startIndex;

        public DbList()
            : this(new List<T>(), -1)
        {
        }

        private DbList(List<T> list, int startIndex)
        {
            this.list = list;
            this.startIndex = startIndex;
        }

        public void Add(T element)
        {
            this.list.Add(element);
        }

        public T Current
        {
            get
            {
                if (this.startIndex < 0)
                {
                    throw new InvalidOperationException("tODO");
                }

                return this.list[startIndex];
            }
        }

        public IDbEnumerable<T> TryMoveNext(out bool moved)
        {
            if (this.startIndex + 1 < this.list.Count)
            {
                moved = true;
                return new DbList<T>(this.list, this.startIndex + 1);
            }
            else
            {
                moved = false;
                return null;
            }
        }
    }

    public sealed class DbEmpty<T> : IDbEnumerable<T>
    {
        private DbEmpty()
        {
        }

        public static DbEmpty<T> Instance { get; } = new DbEmpty<T>();

        public T Current
        {
            get
            {
                throw new InvalidOperationException();
            }
        }

        public IDbEnumerable<T> TryMoveNext(out bool moved)
        {
            moved = false;
            return null;
        }
    }

    public static class DbEnumerableExtensions
    {
        public static IDbEnumerable<TElement> Where<TElement>(this IDbEnumerable<TElement> self, Func<TElement, bool> predicate)
        {
        }

        private sealed class WhereEnumerable<TElement> : IDbEnumerable<TElement>
        {
            private readonly IDbEnumerable<TElement> self;
            private readonly Func<TElement, bool> predicate;

            public WhereEnumerable(IDbEnumerable<TElement> self, Func<TElement, bool> predicate)
            {
                this.self = self;
                this.predicate = predicate;
            }

            public TElement Current
            {
                get
                {
                    return this.self.Current; //// TODO what if self is already at a value?
                }
            }

            public IDbEnumerable<TElement> TryMoveNext(out bool moved)
            {
                if ()
            }
        }

        public static IDbEnumerable<TResult> Select<TElement, TResult>(this IDbEnumerable<TElement> self, Func<TElement, TResult> selector)
        {
            return new SelectEnumerable<TElement, TResult>(self, selector);
        }

        private sealed class SelectEnumerable<TElement, TResult> : IDbEnumerable<TResult>
        {
            private readonly IDbEnumerable<TElement> self;
            private readonly Func<TElement, TResult> selector;

            public SelectEnumerable(IDbEnumerable<TElement> self, Func<TElement, TResult> selector)
            {
                this.self = self;
                this.selector = selector;
            }

            public TResult Current
            {
                get
                {
                    return this.selector(this.self.Current);
                }
            }

            public IDbEnumerable<TResult> TryMoveNext(out bool moved)
            {
                var selfEnumerable = this.self.TryMoveNext(out moved);
                return new SelectEnumerable<TElement, TResult>(selfEnumerable, selector);
            }
        }

        public static DbEnumerableAdapter<T> Enumerator<T>(this IDbEnumerable<T> dbEnumerable)
        {
            return new DbEnumerableAdapter<T>(dbEnumerable);
        }

        public struct DbEnumerableAdapter<T>
        {
            private readonly IDbEnumerable<T> dbEnumerable;

            public DbEnumerableAdapter(IDbEnumerable<T> dbEnumerable)
            {
                this.dbEnumerable = dbEnumerable;
            }

            public EnumeratorAdapter<T> GetEnumerator()
            {
                return new EnumeratorAdapter<T>(this.dbEnumerable);
            }
        }

        public struct EnumeratorAdapter<T>
        {
            private IDbEnumerable<T> dbEnumerable;

            public EnumeratorAdapter()
            {
                throw new Exception("todo");
            }

            internal EnumeratorAdapter(IDbEnumerable<T> dbEnumerable)
            {
                this.dbEnumerable = dbEnumerable;
            }

            public T Current
            {
                get
                {
                    return this.dbEnumerable.Current;
                }
            }

            public bool MoveNext()
            {
                this.dbEnumerable = this.dbEnumerable.TryMoveNext(out var moved);
                return moved;
            }
        }
    }

    /// <summary>
    /// Unit tests for <see cref="WinnersAndLosers{TPlayer}"/>
    /// </summary>
    [TestClass]
    public sealed class WinnersAndLosersUnitTests
    {
        [TestMethod]
        public void Test()
        {
            var list = new DbList<string>();
            foreach (var element in list.Enumerator())
            {
            }

            var list2 = new DbList<string>();
            list2.Add("Asdf");
            foreach (var element in list2.Enumerator())
            {
            }
        }

        /// <summary>
        /// Creates a <see cref="WinnersAndLosers{TPlayer}"/> with <see langword="null"/> winners
        /// </summary>
        [TestMethod]
        public void NullWinners()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new WinnersAndLosers<string>(
                null,
                Enumerable.Empty<string>(),
                Enumerable.Empty<string>()));
        }

        /// <summary>
        /// Creates a <see cref="WinnersAndLosers{TPlayer}"/> with <see langword="null"/> losers
        /// </summary>
        [TestMethod]
        public void NullLosers()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new WinnersAndLosers<string>(
                Enumerable.Empty<string>(),
                null,
                Enumerable.Empty<string>()));
        }

        /// <summary>
        /// Creates a <see cref="WinnersAndLosers{TPlayer}"/> with <see langword="null"/> drawers
        /// </summary>
        [TestMethod]
        public void NullDrawers()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new WinnersAndLosers<string>(
                Enumerable.Empty<string>(),
                Enumerable.Empty<string>(),
                null));
        }

        /// <summary>
        /// Creates a <see cref="WinnersAndLosers{TPlayer}"/> with <see langword="null"/> drawers
        /// </summary>
        [TestMethod]
        public void WinnersAndLosersData()
        {
            var winnersAndLosers = new WinnersAndLosers<string>(
                new[] { "winner" },
                new[] { "loser" },
                new[] { "drawer" });

            CollectionAssert.AreEqual(new[] { "winner" }, winnersAndLosers.Winners.ToList());
            CollectionAssert.AreEqual(new[] { "loser" }, winnersAndLosers.Losers.ToList());
            CollectionAssert.AreEqual(new[] { "drawer" }, winnersAndLosers.Drawers.ToList());
        }
    }
}
