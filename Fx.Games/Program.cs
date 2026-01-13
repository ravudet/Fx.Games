namespace ConsoleApplication1
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Numerics;
    using System.Reflection.Metadata.Ecma335;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Security.Cryptography.X509Certificates;
    using System.Text.Json.Serialization;
    using System.Threading;
    using System.Threading.Tasks;

    using Db.System.Collections.Generic;

    using DbAdapters.System.Collections.Generic;

    using Fx.Distribution;
    using Fx.Games.Displayer;
    using Fx.Games.Driver;
    using Fx.Games.Game;
    using Fx.Games.Game.Amazons;
    using Fx.Games.Strategy;

    using static ConsoleApplication1.Program;

    using Amazons = Fx.Games.Game.Amazons;

    class Program
    {
        public sealed class Boat
        {
            public Boat(string name, int length)
            {
                Name = name;
                Length = length;
            }

            public string Name { get; }
            public int Length { get; }
        }

        public sealed class BoatComparer : IEqualityComparer<Boat>
        {
            private BoatComparer()
            {
            }

            public static BoatComparer Instance { get; } = new BoatComparer();

            public bool Equals(Boat? x, Boat? y)
            {
                if (object.ReferenceEquals(x, y))
                {
                    return true;
                }

                if (x == null)
                {
                    return false;
                }

                if (y == null)
                {
                    return false;
                }

                return string.Equals(x.Name, y.Name) && x.Length == y.Length;
            }

            public int GetHashCode(Boat obj)
            {
                return obj.Name.GetHashCode() ^ obj.Length.GetHashCode();
            }
        }

        public sealed class Coordinate
        {
            public Coordinate(long x, long y)
            {
                X = x;
                Y = y;
            }

            public long X { get; }
            public long Y { get; }
        }

        public sealed class Placement
        {
            public Placement(Boat boat, Coordinate coordinate, bool leftToRight)
            {
                Boat = boat;
                Coordinate = coordinate;
                LeftToRight = leftToRight;
            }

            public Boat Boat { get; }
            public Coordinate Coordinate { get; }
            public bool LeftToRight { get; }
        }

        private static void GenerateLegalBoards()
        {
            var start = DateTime.UtcNow;
            var workingDirectory = "C:\\github\\battleship_board_states";

            long id = 0;
            var filePath = Path.Combine(workingDirectory, $"{id}.txt"); //// TODO this should be a ".bin" file
            using (var file = File.OpenWrite(filePath))
            {
                foreach (var board in LegalBoards())
                {
                    file.WriteByte((byte)(board >> 0));
                    file.WriteByte((byte)(board >> 8));
                    file.WriteByte((byte)(board >> 16));
                    file.WriteByte((byte)(board >> 24));
                    file.WriteByte((byte)(board >> 32));
                    file.WriteByte((byte)(board >> 40));

                    ++id;
                    if (id % 100000 == 0)
                    {
                        file.Flush();
                    }
                }
            }

            Console.WriteLine($"started {start} ended {DateTime.UtcNow} with {id} legal boards");
            Console.ReadLine();
        }

        private static System.Collections.Generic.IEnumerable<long> LegalBoards()
        {
            var destroyer = PossiblePlacements(2).ToList(); // 90 * 2 possible placements
            var cruiser = PossiblePlacements(3).ToList(); // 80 * 2 possible placemenets
            var submarine = PossiblePlacements(3).ToList(); // 80 * 2 possible placements
            var battleship = PossiblePlacements(4).ToList(); // 70 * 2 possible placements
            var carrier = PossiblePlacements(5).ToList(); // 60 * 2 possible placements

            // there are a maximum of 90 * 2 * 80 * 2 * 80 * 2 * 70 * 2 * 60 * 2 = (20 ^ 5) * (9 * 8 * 8 * 7 * 6) = 77,414,400,000 placements to test

            long count = 0;
            foreach (var destroyerPlacement in destroyer)
            {
                foreach (var cruiserPlacement in cruiser)
                {
                    foreach (var submarinePlacement in submarine)
                    {
                        foreach (var battleshipPlacement in battleship)
                        {
                            foreach (var carrierPlacement in carrier)
                            {
                                if ((destroyerPlacement.Bitboard & cruiserPlacement.Bitboard) == 0 &&
                                    (destroyerPlacement.Bitboard & submarinePlacement.Bitboard) == 0 &&
                                    (destroyerPlacement.Bitboard & battleshipPlacement.Bitboard) == 0 &&
                                    (destroyerPlacement.Bitboard & carrierPlacement.Bitboard) == 0 &&

                                    (cruiserPlacement.Bitboard & submarinePlacement.Bitboard) == 0 &&
                                    (cruiserPlacement.Bitboard & battleshipPlacement.Bitboard) == 0 &&
                                    (cruiserPlacement.Bitboard & carrierPlacement.Bitboard) == 0 &&

                                    (submarinePlacement.Bitboard & battleshipPlacement.Bitboard) == 0 &&
                                    (submarinePlacement.Bitboard & carrierPlacement.Bitboard) == 0 &&

                                    (battleshipPlacement.Bitboard & carrierPlacement.Bitboard) == 0)
                                {
                                    /*var bitboard = destroyerPlacement.Bitboard;
                                    var newBitboard = bitboard | cruiserPlacement.Bitboard;
                                    if ((newBitboard ^ cruiserPlacement.Bitboard) == bitboard)
                                    {
                                        bitboard = newBitboard;
                                        newBitboard = bitboard | submarinePlacement.Bitboard;
                                        if ((newBitboard ^ submarinePlacement.Bitboard) == bitboard)
                                        {
                                            bitboard = newBitboard;
                                            newBitboard = bitboard | battleshipPlacement.Bitboard;
                                            if ((newBitboard ^ battleshipPlacement.Bitboard) == bitboard)
                                            {
                                                bitboard = newBitboard;
                                                newBitboard = bitboard | carrierPlacement.Bitboard;
                                                if ((newBitboard ^ carrierPlacement.Bitboard) == bitboard)
                                                {*/
                                    yield return
                                        (long)destroyerPlacement.Placement << 36 |
                                        (long)cruiserPlacement.Placement << 27 |
                                        (long)submarinePlacement.Placement << 18 |
                                        (long)battleshipPlacement.Placement << 9 |
                                        (long)carrierPlacement.Placement;
                                    /*}
                                }
                            }
                        }*/
                                }

                                ++count;
                                if (count % 10000 == 0)
                                {
                                    Console.WriteLine($"possible board count {count}");
                                }
                            }
                        }
                    }
                }
            }
        }

        private static UInt128 GetBitFromCoordinate(byte i, byte j)
        {
            var bit = i * 10 + j;
            if (bit < 64)
            {
                return new UInt128(0, (ulong)1 << bit);
            }
            else
            {
                return new UInt128((ulong)1 << (bit - 64), 0);
            }
        }

        /// <summary>
        /// The returned "placement" is 16 bits with a format of:
        /// 
        /// rrrrrrrxxxxyyyyo
        /// 
        /// where r is a reserved bit and should be 0; x is the "x" coordinate of the boat; y is the "y" coordinate of the boat; o is the orientation of the boat, where 0 indicates left-to-right and 1 indicates top-to-bottom
        /// 
        /// this will return (10 - length + 1) * 2 total placements
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        private static System.Collections.Generic.IEnumerable<(short Placement, UInt128 Bitboard)> PossiblePlacements(int length)
        {
            for (byte i = 0; i < 10 - (length - 1); ++i)
            {
                for (byte j = 0; j < 10; ++j)
                {
                    short placement = 0 << 0;
                    placement |= (short)(i << 5);
                    placement |= (short)(j << 1);

                    UInt128 bitboard = 0;
                    for (byte k = 0; k < length; ++k)
                    {
                        bitboard |= GetBitFromCoordinate((byte)(i + k), j);
                    }

                    yield return (placement, bitboard);
                }
            }

            for (byte i = 0; i < 10; ++i)
            {
                for (byte j = 0; j < 10 - (length - 1); ++j)
                {
                    short placement = 1 << 0;
                    placement |= (short)(i << 5);
                    placement |= (short)(j << 1);

                    UInt128 bitboard = 0;
                    for (byte k = 0; k < length; ++k)
                    {
                        bitboard |= GetBitFromCoordinate(i, (byte)(j + k));
                    }

                    yield return (placement, bitboard);
                }
            }
        }

        public interface ISetupStore
        {
            BattleshipSetup Get(long index);
        }

        public sealed class StreamSetupStore : ISetupStore
        {
            private readonly Stream stream;

            public StreamSetupStore(Stream stream)
            {
                if (!stream.CanSeek)
                {
                    throw new Exception("TODO must be seekable");
                }

                if (!stream.CanRead)
                {
                    throw new Exception("TODO must be readable");
                }

                this.stream = stream;
            }

            public BattleshipSetup Get(long index)
            {
                const int setupSizeInBytes = 6; // there are 9 bits per placement and 5 placements; that's 45 bits; rounding to the next largest byte, thats 6 bytes
                long board = 0;
                this.stream.Seek(index * setupSizeInBytes, SeekOrigin.Begin);

                for (int i = 0; i < setupSizeInBytes; ++i)
                {
                    var read = this.stream.ReadByte();
                    if (read == -1)
                    {
                        throw new Exception("TODO store file does not contain that index");
                    }

                    board |= (long)read << 8 * i;
                }

                var placements = GetPlacementsFromBinary(board);
                return new BattleshipSetup(placements);
            }
        }

        public static void DisplayRandomBoard()
        {
            var filePath = "C:\\github\\battleship_board_states\\0.txt";

            using (var file = File.OpenRead(filePath))
            {
                var setupStore = new StreamSetupStore(file);

                var ticks = Environment.TickCount;
                Console.WriteLine(ticks);
                var random = new Random(ticks);
                for (int i = 0; i < 10000; ++i)
                {
                    var next = random.NextInt64(0, 30_093_975_536); //// TODO add the count to the file

                    var setup = setupStore.Get(next);

                    DisplaySetup(setup);
                }

                Console.ReadLine();
            }
        }

        public static void DisplaySetup(BattleshipSetup setup)
        {
            Console.Write("  ");
            for (int i = 0; i < 10; ++i)
            {
                Console.Write($"{i}");
            }

            Console.WriteLine();

            for (int i = 0; i < 10; ++i)
            {
                Console.Write($"{(char)('A' + i)} ");
                for (int j = 0; j < 10; ++j)
                {
                    var square = setup.Squares[i][j];
                    if (square == null)
                    {
                        Console.Write(' ');
                    }
                    else
                    {
                        Console.Write(square.Name[0]);
                    }
                }

                Console.WriteLine();
            }
        }

        public static System.Collections.Generic.IEnumerable<Placement> GetPlacementsFromBinary(long binary)
        {
            var carrierPlacement = binary >> 0;
            yield return GetPlacementFromBinary(
                carrierPlacement,
                new Boat("carrier", 5));

            var battleshipPlacement = binary >> 9;
            yield return GetPlacementFromBinary(
                battleshipPlacement,
                new Boat("battleship", 4));

            var submarinePlacement = binary >> 18;
            yield return GetPlacementFromBinary(
                submarinePlacement,
                new Boat("submarine", 3));

            var cruiserPlacement = binary >> 27;
            yield return GetPlacementFromBinary(
                cruiserPlacement,
                new Boat("cruiser", 3));

            var destroyerPlacement = binary >> 36;
            yield return GetPlacementFromBinary(
                destroyerPlacement,
                new Boat("destroyer", 2));
        }

        private static Placement GetPlacementFromBinary(long placement, Boat boat)
        {
            var xMask = 0b111100000;
            var yMask = 0b000011110;
            var orientationMask = 0b000000001;

            return new Placement(
                boat,
                new Coordinate(
                    (placement & xMask) >> 5,
                    (placement & yMask) >> 1),
                (placement & orientationMask >> 0) == 0);
        }

        public sealed class BattleshipSetup
        {
            public BattleshipSetup(System.Collections.Generic.IEnumerable<Placement> placements)
            {
                this.Squares = new Boat?[10][];
                for (int i = 0; i < this.Squares.Length; ++i)
                {
                    this.Squares[i] = new Boat?[10];
                }

                foreach (var placement in placements)
                {
                    if (placement.LeftToRight)
                    {
                        for (long i = placement.Coordinate.X; i < placement.Coordinate.X + placement.Boat.Length; ++i)
                        {
                            this.Squares[i][placement.Coordinate.Y] = placement.Boat;
                        }
                    }
                    else
                    {
                        for (long i = placement.Coordinate.Y; i < placement.Coordinate.Y + placement.Boat.Length; ++i)
                        {
                            this.Squares[placement.Coordinate.X][i] = placement.Boat;
                        }
                    }
                }
            }

            public BattleshipSetup(Boat?[][] squares)
            {
                Squares = squares;
            }

            public Boat?[][] Squares { get; }
        }

        public sealed class BattleshipShotResults
        {
            public BattleshipShotResults()
                : this(Enumerable.Repeat(Enumerable.Repeat(BattleshipShotResult.NoShot.Instance, 10).ToArray(), 10).ToArray())
            {
            }

            public BattleshipShotResults(BattleshipShotResult[][] shots)
            {
                Shots = shots;
            }

            public BattleshipShotResult[][] Shots { get; }
        }

        public abstract class BattleshipShotResult
        {
            private BattleshipShotResult()
            {
            }

            public sealed class NoShot : BattleshipShotResult
            {
                private NoShot()
                {
                }

                public static NoShot Instance { get; } = new NoShot();
            }

            public sealed class Miss : BattleshipShotResult
            {
                private Miss()
                {
                }

                public static Miss Instance { get; } = new Miss();
            }

            public sealed class Hit : BattleshipShotResult
            {
                public Hit(Boat boat)
                {
                    Boat = boat;
                }

                public Boat Boat { get; }
            }
        }

        public sealed class BattleshipDisplayer : IDisplayer<Battleship, BattleshipShotResults, Coordinate, string, Univariate<Battleship>>
        {
            private BattleshipDisplayer()
            {
            }

            public static BattleshipDisplayer Instance { get; } = new BattleshipDisplayer();

            public void DisplayAvailableMoves(Battleship game)
            {
            }

            public void DisplayBoard(Battleship game)
            {
                Console.Write("  ");
                for (int i = 0; i < 10; ++i)
                {
                    Console.Write($"{i}");
                }

                Console.WriteLine();

                for (int i = 0; i < 10; ++i)
                {
                    Console.Write($"{(char)('A' + i)} ");
                    for (int j = 0; j < 10; ++j)
                    {
                        var square = game.Board.Shots[i][j];
                        if (square is BattleshipShotResult.Hit hit)
                        {
                            Console.Write(hit.Boat.Name[2]);
                        }
                        else if (square is BattleshipShotResult.Miss miss)
                        {
                            Console.Write('m');
                        }
                        else
                        {
                            Console.Write(' ');
                        }
                    }

                    Console.Write('\t');
                    if (i == 3)
                    {
                        Console.Write("s = destroyer (2)");
                    }
                    else if (i == 4)
                    {
                        Console.Write("u = cruiser (3)");
                    }
                    else if (i == 5)
                    {
                        Console.Write("b = submarine (3)");
                    }
                    else if (i == 6)
                    {
                        Console.Write("t = battleship (4)");
                    }
                    else if (i == 7)
                    {
                        Console.Write("r = carrier (5)");
                    }

                    Console.WriteLine();
                }
            }

            public void DisplayOutcome(Battleship game)
            {
                var winner = game.WinnersAndLosers.Winners.First();
                Console.WriteLine($"{winner} wins in {game.MoveCount} moves!");
            }

            public void DisplaySelectedMove(Coordinate move)
            {
                Console.WriteLine($"{(char)(move.X + 'A')}{move.Y}");
            }
        }

        public sealed class BattleshipConsoleStrategy : IStrategy<Battleship, BattleshipShotResults, Coordinate, string, Univariate<Battleship>>
        {
            private BattleshipConsoleStrategy()
            {
            }

            public static BattleshipConsoleStrategy Instance { get; } = new BattleshipConsoleStrategy();

            public Coordinate SelectMove(Battleship game)
            {
                Console.WriteLine("Select move:");
                var read = Console.ReadLine();
                if (read == null)
                {
                    throw new Exception("tODO");
                }

                var x = read[0] - 'A';
                var y = int.Parse(read.AsSpan().Slice(1));

                return new Coordinate(x, y);
            }
        }

        public sealed class Battleship : IGame<Battleship, BattleshipShotResults, Coordinate, string, Univariate<Battleship>>
        {
            private readonly BattleshipSetup battleshipSetup;
            private readonly string player;
            private readonly BattleshipShotResults battleshipShotResults;

            public Battleship(BattleshipSetup battleshipSetup, string player)
                : this(
                      battleshipSetup,
                      player,
                      new BattleshipShotResults(),
                      0)
            {
            }

            private Battleship(
                BattleshipSetup battleshipSetup,
                string player,
                BattleshipShotResults battleshipShotResults,
                int moveCount)
            {
                this.battleshipSetup = battleshipSetup;
                this.player = player;
                this.battleshipShotResults = battleshipShotResults;
                this.MoveCount = moveCount;
            }

            public int MoveCount { get; }

            public string CurrentPlayer
            {
                get
                {
                    return this.player;
                }
            }

            public WinnersAndLosers<string> WinnersAndLosers
            {
                get
                {
                    if (this.IsGameOver)
                    {
                        return new WinnersAndLosers<string>(
                            new[] { this.player },
                            Enumerable.Empty<string>(),
                            Enumerable.Empty<string>());
                    }
                    else
                    {
                        return new WinnersAndLosers<string>(
                            Enumerable.Empty<string>(),
                            new[] { this.player },
                            Enumerable.Empty<string>());
                    }
                }
            }

            public bool IsGameOver
            {
                get
                {
                    for (int i = 0; i < 10; ++i)
                    {
                        for (int j = 0; j < 10; ++j)
                        {
                            if (this.battleshipSetup.Squares[i][j] != null)
                            {
                                if (!(this.battleshipShotResults.Shots[i][j] is BattleshipShotResult.Hit))
                                {
                                    return false;
                                }
                            }
                        }
                    }

                    return true;
                }
            }

            public System.Collections.Generic.IEnumerable<Coordinate> Moves
            {
                get
                {
                    for (int i = 0; i < 10; ++i)
                    {
                        for (int j = 0; j < 10; ++j)
                        {
                            if (this.battleshipShotResults.Shots[i][j] is BattleshipShotResult.NoShot)
                            {
                                yield return new Coordinate(i, j);
                            }
                        }
                    }
                }
            }

            public BattleshipShotResults Board
            {
                get
                {
                    return this.battleshipShotResults;
                }
            }

            public Battleship CommitMove(Coordinate move)
            {
                if (!(this.battleshipShotResults.Shots[move.X][move.Y] is BattleshipShotResult.NoShot))
                {
                    throw new IllegalMoveExeption("TODO");
                }

                var newShots = this.battleshipShotResults.Shots.Select(row => row.ToArray()).ToArray();

                var square = this.battleshipSetup.Squares[move.X][move.Y];
                if (square == null)
                {
                    newShots[move.X][move.Y] = BattleshipShotResult.Miss.Instance;
                }
                else
                {
                    newShots[move.X][move.Y] = new BattleshipShotResult.Hit(square);
                }

                return new Battleship(
                    this.battleshipSetup,
                    this.player,
                    new BattleshipShotResults(newShots),
                    this.MoveCount + 1);
            }

            public Univariate<Battleship> ExploreMove(Coordinate move)
            {
                return new Univariate<Battleship>(this.CommitMove(move));
            }
        }

        public sealed class BattleshipNaive : IStrategy<Battleship, BattleshipShotResults, Coordinate, string, Univariate<Battleship>>
        {
            private Coordinate? lastMove;

            private HashSet<Boat> remainingTwosToDiscover;

            private HashSet<Boat> remainingThreesToDiscover;

            private HashSet<Boat> remainingFoursToDiscover;

            private HashSet<Boat> remainingFivesToDiscover;

            private (int distance, long row)? recentlyDiscoveredTheLastBoatOfALength;

            public BattleshipNaive()
            {
                this.lastMove = null;

                this.remainingTwosToDiscover = new HashSet<Boat>(
                    new[]
                    {
                        new Boat("destroyer", 2),
                    },
                    BoatComparer.Instance);
                this.remainingThreesToDiscover = new HashSet<Boat>(
                    new[]
                    {
                        new Boat("submarine", 3),
                        new Boat("cruiser", 3),
                    },
                    BoatComparer.Instance);
                this.remainingFoursToDiscover = new HashSet<Boat>(
                    new[]
                    {
                        new Boat("battleship", 4),
                    },
                    BoatComparer.Instance);
                this.remainingFivesToDiscover = new HashSet<Boat>(
                    new[]
                    {
                        new Boat("carrier", 5),
                    },
                    BoatComparer.Instance);

                this.recentlyDiscoveredTheLastBoatOfALength = null;
            }

            public Coordinate SelectMove(Battleship game)
            {
                if (this.lastMove == null)
                {
                    var move = new Coordinate(0, 0);
                    this.lastMove = move;
                    return this.lastMove;
                }

                var previousDistance = 2;
                if (!this.remainingTwosToDiscover.Any())
                {
                    previousDistance = 3;
                    if (!this.remainingThreesToDiscover.Any())
                    {
                        previousDistance = 4;
                        if (!this.remainingFoursToDiscover.Any())
                        {
                            previousDistance = 5;
                            if (!this.remainingFivesToDiscover.Any())
                            {
                                this.lastMove = Program.DestroyShips(game);
                                return this.lastMove;
                            }
                        }
                    }
                }

                var resultOfLastShot = game.Board.Shots[this.lastMove.X][this.lastMove.Y];
                if (resultOfLastShot is BattleshipShotResult.Hit hit)
                {
                    var boat = hit.Boat;
                    if (boat.Length == 2)
                    {
                        if (this.remainingTwosToDiscover.Remove(boat) && previousDistance == 2)
                        {
                            this.recentlyDiscoveredTheLastBoatOfALength = (previousDistance, this.lastMove.X);
                        }
                    }
                    else if (boat.Length == 3)
                    {
                        if (this.remainingThreesToDiscover.Remove(boat) && !this.remainingThreesToDiscover.Any() && previousDistance == 3)
                        {
                            this.recentlyDiscoveredTheLastBoatOfALength = (previousDistance, this.lastMove.X);
                        }
                    }
                    else if (boat.Length == 4)
                    {
                        if (this.remainingFoursToDiscover.Remove(boat) && previousDistance == 4)
                        {
                            this.recentlyDiscoveredTheLastBoatOfALength = (previousDistance, this.lastMove.X);
                        }
                    }
                    else if (boat.Length == 5)
                    {
                        if (this.remainingFivesToDiscover.Remove(boat) && previousDistance == 5)
                        {
                            this.recentlyDiscoveredTheLastBoatOfALength = (previousDistance, this.lastMove.X);
                        }
                    }
                }

                if (this.recentlyDiscoveredTheLastBoatOfALength != null)
                {
                    if (this.lastMove.X == this.recentlyDiscoveredTheLastBoatOfALength.Value.row)
                    {
                        var nextMove = GetNextMove(game, this.lastMove, this.recentlyDiscoveredTheLastBoatOfALength.Value.distance);
                        if (nextMove.X == this.recentlyDiscoveredTheLastBoatOfALength.Value.row + 1)
                        {
                            if (nextMove.X <= 9)
                            {
                                // start a new full row after finishing the row at the old distance to make sure we don't accidentally lose track of a boat
                                this.lastMove = new Coordinate(nextMove.X, 0);
                            }
                            else
                            {
                                // you're actually at the end of the board, so the last shot must have discovered the last boat
                                this.recentlyDiscoveredTheLastBoatOfALength = null;
                                this.lastMove = Program.DestroyShips(game);
                            }
                        }
                        else
                        {
                            // finish out the row at the old distance to make sure we don't accidentally lose track of a boat
                            this.lastMove = nextMove;
                        }

                        return this.lastMove;
                    }
                    else if (
                        this.lastMove.X == this.recentlyDiscoveredTheLastBoatOfALength.Value.row + 1 &&
                        this.lastMove.Y < 9)
                    {
                        // complete the full row to make sure we don't accidentally lose track of a boat
                        this.lastMove = new Coordinate(this.lastMove.X, this.lastMove.Y + 1);
                        return this.lastMove;
                    }
                    else
                    {
                        this.recentlyDiscoveredTheLastBoatOfALength = null;
                        if (this.lastMove.X < 9)
                        {
                            // just reset the column once you've completed the full row
                            this.lastMove = new Coordinate(this.lastMove.X + 1, 0);
                            return this.lastMove;
                        }
                        else
                        {
                            // you're actually at the end of the board, so the last shot must have discovered the last boat
                            this.lastMove = Program.DestroyShips(game);
                            return this.lastMove;
                        }
                    }
                }
                else
                {
                    var distance = 2;
                    if (!this.remainingTwosToDiscover.Any())
                    {
                        distance = 3;
                        if (!this.remainingThreesToDiscover.Any())
                        {
                            distance = 4;
                            if (!this.remainingFoursToDiscover.Any())
                            {
                                distance = 5;
                                if (!this.remainingFivesToDiscover.Any())
                                {
                                    this.lastMove = Program.DestroyShips(game);
                                    return this.lastMove;
                                }
                            }
                        }
                    }

                    this.lastMove = GetNextMove(game, this.lastMove, distance);
                    return this.lastMove;
                }

                throw new Exception("TODO");
            }

            private static Coordinate GetNextMove(Battleship game, Coordinate lastMove, int distance)
            {
                var nextY = lastMove.Y + distance;
                if (nextY < 10)
                {
                    return new Coordinate(lastMove.X, nextY);
                }

                for (int j = 0; j < distance - 1; ++j)
                {
                    if (!(game.Board.Shots[lastMove.X][j] is BattleshipShotResult.NoShot))
                    {
                        return new Coordinate(lastMove.X + 1, j + 1);
                    }
                }

                return new Coordinate(lastMove.X + 1, 0);
            }

            private Coordinate DestroyShips(Battleship game)
            {
                for (int i = 0; i < 10; ++i)
                {
                    for (int j = 0; j < 10; ++j)
                    {
                        if (game.Board.Shots[i][j] is BattleshipShotResult.Hit hit)
                        {
                            for (int k = 1; k < hit.Boat.Length; ++k)
                            {
                                Coordinate? coordinate;
                                if (this.TryShoot(game, i, j + k, out coordinate))
                                {
                                    return coordinate;
                                }

                                if (this.TryShoot(game, i + k, j, out coordinate))
                                {
                                    return coordinate;
                                }

                                if (this.TryShoot(game, i, j - k, out coordinate))
                                {
                                    return coordinate;
                                }

                                if (this.TryShoot(game, i - k, j, out coordinate))
                                {
                                    return coordinate;
                                }
                            }
                        }
                    }
                }

                throw new Exception("TODO the game is still in progress, but the strategy thinks we've sunk all the ships");
            }

            private bool TryShoot(Battleship game, int i, int j, [MaybeNullWhen(false)] out Coordinate coordinate)
            {
                if (i >= 10 || i < 0 || j >= 10 || j < 0)
                {
                    coordinate = null;
                    return false;
                }

                if (game.Board.Shots[i][j] is BattleshipShotResult.NoShot)
                {
                    coordinate = new Coordinate(i, j);
                    return true;
                }

                coordinate = null;
                return false;
            }
        }

        public sealed class BattleshipReverseDistance : IStrategy<Battleship, BattleshipShotResults, Coordinate, string, Univariate<Battleship>>
        {
            private Coordinate? lastMove;

            private HashSet<Boat> remainingTwosToDiscover;

            private HashSet<Boat> remainingThreesToDiscover;

            private HashSet<Boat> remainingFoursToDiscover;

            private HashSet<Boat> remainingFivesToDiscover;

            private (int distance, long row)? recentlyDiscoveredTheLastBoatOfALength;

            private bool finishedFours;

            public BattleshipReverseDistance()
            {
                this.lastMove = null;

                this.remainingTwosToDiscover = new HashSet<Boat>(
                    new[]
                    {
                        new Boat("destroyer", 2),
                    },
                    BoatComparer.Instance);
                this.remainingThreesToDiscover = new HashSet<Boat>(
                    new[]
                    {
                        new Boat("submarine", 3),
                        new Boat("cruiser", 3),
                    },
                    BoatComparer.Instance);
                this.remainingFoursToDiscover = new HashSet<Boat>(
                    new[]
                    {
                        new Boat("battleship", 4),
                    },
                    BoatComparer.Instance);
                this.remainingFivesToDiscover = new HashSet<Boat>(
                    new[]
                    {
                        new Boat("carrier", 5),
                    },
                    BoatComparer.Instance);

                this.recentlyDiscoveredTheLastBoatOfALength = null;

                this.finishedFours = false;
            }

            public Coordinate SelectMove(Battleship game)
            {
                if (this.lastMove == null)
                {
                    var move = new Coordinate(0, 0);
                    this.lastMove = move;
                    return this.lastMove;
                }

                var distance = 4;
                var resultOfLastShot = game.Board.Shots[this.lastMove.X][this.lastMove.Y];
                if (resultOfLastShot is BattleshipShotResult.Hit hit)
                {
                    var boat = hit.Boat;
                    if (boat.Length == 2)
                    {
                        if (this.remainingTwosToDiscover.Remove(boat) && distance == 2)
                        {
                            this.recentlyDiscoveredTheLastBoatOfALength = (distance, this.lastMove.X);
                        }
                    }
                    else if (boat.Length == 3)
                    {
                        if (this.remainingThreesToDiscover.Remove(boat) && !this.remainingThreesToDiscover.Any() && distance == 3)
                        {
                            this.recentlyDiscoveredTheLastBoatOfALength = (distance, this.lastMove.X);
                        }
                    }
                    else if (boat.Length == 4)
                    {
                        if (this.remainingFoursToDiscover.Remove(boat) && distance == 4)
                        {
                            this.recentlyDiscoveredTheLastBoatOfALength = (distance, this.lastMove.X);
                        }
                    }
                    else if (boat.Length == 5)
                    {
                        if (this.remainingFivesToDiscover.Remove(boat) && distance == 5)
                        {
                            this.recentlyDiscoveredTheLastBoatOfALength = (distance, this.lastMove.X);
                        }
                    }
                }

                if (lastMove.X == 9 && this.lastMove.Y == 9)
                {
                    this.finishedFours = true;
                    this.lastMove = new Coordinate(0, 2);
                    return this.lastMove;
                }

                if (this.finishedFours)
                {
                    if (!this.remainingThreesToDiscover.Any())
                    {
                        if (!this.remainingTwosToDiscover.Any())
                        {
                            this.lastMove = Program.DestroyShips(game);
                            return this.lastMove;
                        }
                    }

                    distance = 2;
                }

                /*if (this.recentlyDiscoveredTheLastBoatOfALength != null)
                {
                    if (this.lastMove.X == this.recentlyDiscoveredTheLastBoatOfALength.Value.row)
                    {
                        var nextMove = GetNextMove(game, this.lastMove, this.recentlyDiscoveredTheLastBoatOfALength.Value.distance);
                        if (nextMove.X == this.recentlyDiscoveredTheLastBoatOfALength.Value.row + 1)
                        {
                            if (nextMove.X <= 9)
                            {
                                // start a new full row after finishing the row at the old distance to make sure we don't accidentally lose track of a boat
                                this.lastMove = new Coordinate(nextMove.X, 0);
                            }
                            else
                            {
                                // you're actually at the end of the board, so the last shot must have discovered the last boat
                                this.recentlyDiscoveredTheLastBoatOfALength = null;
                                this.lastMove = DestroyShips(game);
                            }
                        }
                        else
                        {
                            // finish out the row at the old distance to make sure we don't accidentally lose track of a boat
                            this.lastMove = nextMove;
                        }

                        return this.lastMove;
                    }
                    else if (
                        this.lastMove.X == this.recentlyDiscoveredTheLastBoatOfALength.Value.row + 1 &&
                        this.lastMove.Y < 9)
                    {
                        // complete the full row to make sure we don't accidentally lose track of a boat
                        this.lastMove = new Coordinate(this.lastMove.X, this.lastMove.Y + 1);
                        return this.lastMove;
                    }
                    else
                    {
                        this.recentlyDiscoveredTheLastBoatOfALength = null;
                        if (this.lastMove.X < 9)
                        {
                            // just reset the column once you've completed the full row
                            this.lastMove = new Coordinate(this.lastMove.X + 1, 0);
                            return this.lastMove;
                        }
                        else
                        {
                            // you're actually at the end of the board, so the last shot must have discovered the last boat
                            ////this.lastMove = DestroyShips(game);
                            this.lastMove = this.SelectMove(game);
                            return this.lastMove;
                        }
                    }
                }
                else
                {*/
                this.lastMove = GetNextMove(game, this.lastMove, distance);
                if (game.Board.Shots[this.lastMove.X][this.lastMove.Y] is BattleshipShotResult.NoShot)
                {
                    return this.lastMove;
                }
                else
                {
                    return this.SelectMove(game);
                }
                ///}

                throw new Exception("TODO");
            }

            private static Coordinate GetNextMove(Battleship game, Coordinate lastMove, int distance)
            {
                var nextY = lastMove.Y + distance;
                if (nextY < 10)
                {
                    return new Coordinate(lastMove.X, nextY);
                }

                for (int j = 0; j < distance - 1; ++j)
                {
                    if (!(game.Board.Shots[lastMove.X][j] is BattleshipShotResult.NoShot))
                    {
                        return new Coordinate(lastMove.X + 1, j + 1);
                    }
                }

                return new Coordinate(lastMove.X + 1, 0);
            }

            private Coordinate DestroyShips(Battleship game)
            {
                for (int i = 0; i < 10; ++i)
                {
                    for (int j = 0; j < 10; ++j)
                    {
                        if (game.Board.Shots[i][j] is BattleshipShotResult.Hit hit)
                        {
                            for (int k = 1; k < hit.Boat.Length; ++k)
                            {
                                Coordinate? coordinate;
                                if (this.TryShoot(game, i, j + k, out coordinate))
                                {
                                    return coordinate;
                                }

                                if (this.TryShoot(game, i + k, j, out coordinate))
                                {
                                    return coordinate;
                                }

                                if (this.TryShoot(game, i, j - k, out coordinate))
                                {
                                    return coordinate;
                                }

                                if (this.TryShoot(game, i - k, j, out coordinate))
                                {
                                    return coordinate;
                                }
                            }
                        }
                    }
                }

                throw new Exception("TODO the game is still in progress, but the strategy thinks we've sunk all the ships");
            }

            private bool TryShoot(Battleship game, int i, int j, [MaybeNullWhen(false)] out Coordinate coordinate)
            {
                if (i >= 10 || i < 0 || j >= 10 || j < 0)
                {
                    coordinate = null;
                    return false;
                }

                if (game.Board.Shots[i][j] is BattleshipShotResult.NoShot)
                {
                    coordinate = new Coordinate(i, j);
                    return true;
                }

                coordinate = null;
                return false;
            }
        }


        public sealed class BattleshipRandomDiscovery : IStrategy<Battleship, BattleshipShotResults, Coordinate, string, Univariate<Battleship>>
        {
            private Coordinate? lastMove;

            private HashSet<Boat> remainingTwosToDiscover;

            private HashSet<Boat> remainingThreesToDiscover;

            private HashSet<Boat> remainingFoursToDiscover;

            private HashSet<Boat> remainingFivesToDiscover;

            private (int distance, long row)? recentlyDiscoveredTheLastBoatOfALength;

            private bool finishedFours;
            private readonly Random random;

            public BattleshipRandomDiscovery(Random random)
            {
                this.lastMove = null;

                this.remainingTwosToDiscover = new HashSet<Boat>(
                    new[]
                    {
                        new Boat("destroyer", 2),
                    },
                    BoatComparer.Instance);
                this.remainingThreesToDiscover = new HashSet<Boat>(
                    new[]
                    {
                        new Boat("submarine", 3),
                        new Boat("cruiser", 3),
                    },
                    BoatComparer.Instance);
                this.remainingFoursToDiscover = new HashSet<Boat>(
                    new[]
                    {
                        new Boat("battleship", 4),
                    },
                    BoatComparer.Instance);
                this.remainingFivesToDiscover = new HashSet<Boat>(
                    new[]
                    {
                        new Boat("carrier", 5),
                    },
                    BoatComparer.Instance);

                this.recentlyDiscoveredTheLastBoatOfALength = null;

                this.finishedFours = false;
                this.random = random;
            }

            public Coordinate SelectMove(Battleship game)
            {
                if (
                    this.remainingTwosToDiscover.Any() ||
                    this.remainingThreesToDiscover.Any() ||
                    this.remainingFoursToDiscover.Any() ||
                    this.remainingFivesToDiscover.Any())
                {
                    var moves = game.Moves.ToList();
                    var next = random.Next(0, moves.Count);

                    if (this.lastMove != null)
                    {
                        var resultOfLastShot = game.Board.Shots[this.lastMove.X][this.lastMove.Y];
                        if (resultOfLastShot is BattleshipShotResult.Hit hit)
                        {
                            var boat = hit.Boat;
                            if (boat.Length == 2)
                            {
                                this.remainingTwosToDiscover.Remove(boat);
                            }
                            else if (boat.Length == 3)
                            {
                                this.remainingThreesToDiscover.Remove(boat);
                            }
                            else if (boat.Length == 4)
                            {
                                this.remainingFoursToDiscover.Remove(boat);
                            }
                            else if (boat.Length == 5)
                            {
                                this.remainingFivesToDiscover.Remove(boat);
                            }
                        }
                    }

                    this.lastMove = moves[next];
                    return this.lastMove;
                }

                this.lastMove = DestroyShips(game);
                return this.lastMove;
            }

            private static Coordinate GetNextMove(Battleship game, Coordinate lastMove, int distance)
            {
                var nextY = lastMove.Y + distance;
                if (nextY < 10)
                {
                    return new Coordinate(lastMove.X, nextY);
                }

                for (int j = 0; j < distance - 1; ++j)
                {
                    if (!(game.Board.Shots[lastMove.X][j] is BattleshipShotResult.NoShot))
                    {
                        return new Coordinate(lastMove.X + 1, j + 1);
                    }
                }

                return new Coordinate(lastMove.X + 1, 0);
            }

            private static Coordinate DestroyShips(Battleship game)
            {
                for (int i = 0; i < 10; ++i)
                {
                    for (int j = 0; j < 10; ++j)
                    {
                        if (game.Board.Shots[i][j] is BattleshipShotResult.Hit hit)
                        {
                            for (int k = 1; k < hit.Boat.Length; ++k)
                            {
                                Coordinate? coordinate;
                                if (TryShoot(game, i, j + k, out coordinate))
                                {
                                    return coordinate;
                                }

                                if (TryShoot(game, i + k, j, out coordinate))
                                {
                                    return coordinate;
                                }

                                if (TryShoot(game, i, j - k, out coordinate))
                                {
                                    return coordinate;
                                }

                                if (TryShoot(game, i - k, j, out coordinate))
                                {
                                    return coordinate;
                                }
                            }
                        }
                    }
                }

                throw new Exception("TODO the game is still in progress, but the strategy thinks we've sunk all the ships");
            }

            private static bool TryShoot(Battleship game, int i, int j, [MaybeNullWhen(false)] out Coordinate coordinate)
            {
                if (i >= 10 || i < 0 || j >= 10 || j < 0)
                {
                    coordinate = null;
                    return false;
                }

                if (game.Board.Shots[i][j] is BattleshipShotResult.NoShot)
                {
                    coordinate = new Coordinate(i, j);
                    return true;
                }

                coordinate = null;
                return false;
            }
        }

        private static Coordinate DestroyShips(Battleship game)
        {
            if (!TryDestroyShips(game, out var coordinate))
            {
                throw new Exception("TODO the game is still in progress, but the strategy thinks we've sunk all the ships");
            }
            
            return coordinate;
        }

        private static bool TryDestroyShips(Battleship game, [MaybeNullWhen(false)] out Coordinate coordinate)
        {
            for (int i = 0; i < 10; ++i)
            {
                for (int j = 0; j < 10; ++j)
                {
                    if (game.Board.Shots[i][j] is BattleshipShotResult.Hit hit)
                    {
                        var boat = hit.Boat;
                        var axis = DetermineAxis(game, boat, i, j);
                        if (axis == null)
                        {
                            if (TryShoot(game, i, j + 1, out coordinate))
                            {
                                return true;
                            }

                            if (TryShoot(game, i + 1, j, out coordinate))
                            {
                                return true;
                            }

                            if (TryShoot(game, i, j - 1, out coordinate))
                            {
                                return true;
                            }

                            if (TryShoot(game, i - 1, j, out coordinate))
                            {
                                return true;
                            }
                        }
                        else
                        {
                            var extremes = DetermineExtremes(game, boat, i, j, axis.Value);
                            if (axis.Value)
                            {
                                var distance = extremes.max.y - extremes.min.y + 1;
                                if (distance == boat.Length)
                                {
                                    for (int k = 0; k < boat.Length; ++k)
                                    {
                                        if (TryShoot(game, i, extremes.min.y + k, out coordinate))
                                        {
                                            return true;
                                        }
                                    }
                                }
                                else
                                {
                                    for (int k = 0; k < 10; ++k)
                                    {
                                        if (TryShoot(game, extremes.min.x, extremes.max.y + k, out coordinate))
                                        {
                                            return true;
                                        }

                                        if (TryShoot(game, extremes.min.x, extremes.min.y - k, out coordinate))
                                        {
                                            return true;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                var distance = extremes.max.x - extremes.min.x + 1;
                                if (distance == boat.Length)
                                {
                                    for (int k = 0; k < boat.Length; ++k)
                                    {
                                        if (TryShoot(game, extremes.min.x + k, j, out coordinate))
                                        {
                                            return true;
                                        }
                                    }
                                }
                                else
                                {
                                    for (int k = 0; k < 10; ++k)
                                    {
                                        if (TryShoot(game, extremes.max.x + k, extremes.min.y, out coordinate))
                                        {
                                            return true;
                                        }

                                        if (TryShoot(game, extremes.min.x - k, extremes.min.y, out coordinate))
                                        {
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            coordinate = default;
            return false;
        }

        private static ((int x, int y) min, (int x, int y) max) DetermineExtremes(
            Battleship game,
            Boat boat,
            int i,
            int j,
            bool leftToRight)
        {
            (int x, int y)? min = null;
            (int x, int y)? max = null;
            if (leftToRight)
            {
                for (int k = 0; k < 10; ++k)
                {
                    if (game.Board.Shots[i][k] is BattleshipShotResult.Hit hit)
                    {
                        if (BoatComparer.Instance.Equals(hit.Boat, boat))
                        {
                            if (min == null)
                            {
                                min = (i, k);
                            }

                            max = (i, k);
                        }
                    }
                }
            }
            else
            {
                for (int k = 0; k < 10; ++k)
                {
                    if (game.Board.Shots[k][j] is BattleshipShotResult.Hit hit)
                    {
                        if (BoatComparer.Instance.Equals(hit.Boat, boat))
                        {
                            if (min == null)
                            {
                                min = (k, j);
                            }

                            max = (k, j);
                        }
                    }
                }
            }

            return (min!.Value, max!.Value);
        }

        private static bool? DetermineAxis(Battleship game, Boat boat, int i, int j)
        {
            for (int k = 0; k < 10; ++k)
            {
                if (IsSameBoat(game, boat, i, k) && j != k)
                {
                    return true;
                }

                if (IsSameBoat(game, boat, k, j) && i != k)
                {
                    return false;
                }
            }

            return null;
        }

        private static bool IsSameBoat(Battleship game, Boat boat, int i, int j)
        {
            if (i < 0 || i >= 10 || j < 0 || j >= 10)
            {
                return false;
            }

            var square = game.Board.Shots[i][j];
            if (square is BattleshipShotResult.Hit hit)
            {
                return BoatComparer.Instance.Equals(hit.Boat, boat);
            }

            return false;
        }

        private static bool TryShoot(Battleship game, long i, long j, [MaybeNullWhen(false)] out Coordinate coordinate)
        {
            if (i >= 10 || i < 0 || j >= 10 || j < 0)
            {
                coordinate = null;
                return false;
            }

            if (game.Board.Shots[i][j] is BattleshipShotResult.NoShot)
            {
                coordinate = new Coordinate(i, j);
                return true;
            }

            coordinate = null;
            return false;
        }

        public interface ITransform
        {
            Coordinate Transform(Coordinate coordinate);
        }

        public static class Transform
        {
            public static ITransform MirrorOverX { get; } = MirrorOverXTransform.Instance;

            private sealed class MirrorOverXTransform : ITransform
            {
                private MirrorOverXTransform()
                {
                }

                public static MirrorOverXTransform Instance { get; } = new MirrorOverXTransform();

                public Coordinate Transform(Coordinate coordinate)
                {
                    return new Coordinate(9 - coordinate.X, coordinate.Y);
                }
            }

            public static ITransform MirrorOverY { get; } = MirrorOverYTransform.Instance;

            private sealed class MirrorOverYTransform : ITransform
            {
                private MirrorOverYTransform()
                {
                }

                public static MirrorOverYTransform Instance { get; } = new MirrorOverYTransform();

                public Coordinate Transform(Coordinate coordinate)
                {
                    return new Coordinate(coordinate.X, 9 - coordinate.Y);
                }
            }

            public static ITransform MirrorOver45 { get; } = MirrorOver45Transform.Instance;

            private sealed class MirrorOver45Transform : ITransform
            {
                private MirrorOver45Transform()
                {
                }

                public static MirrorOver45Transform Instance { get; } = new MirrorOver45Transform();

                public Coordinate Transform(Coordinate coordinate)
                {
                    return new Coordinate(9 - coordinate.X, 9 - coordinate.Y);
                }
            }
        }

        private static BattleshipSetup TransformSetup(BattleshipSetup setup, ITransform transform)
        {
            var squares = new Boat?[10][];
            for (int i = 0; i < 10; ++i)
            {
                squares[i] = new Boat?[10];
            }

            for (int i = 0; i < 10; ++i)
            {
                for (int j = 0; j < 10; ++j)
                {
                    var coordinate = new Coordinate(i, j);
                    var transformedCoordinate = transform.Transform(coordinate);

                    squares[transformedCoordinate.X][transformedCoordinate.Y] = setup.Squares[i][j];
                }
            }

            return new BattleshipSetup(squares);
        }

        public sealed class HardcodedSquares : IStrategy<Battleship, BattleshipShotResults, Coordinate, string, Univariate<Battleship>>
        {
            private readonly IReadOnlyList<IReadOnlyList<Coordinate>> squares;

            private int squareIndex;

            private int sequenceIndex;

            public HardcodedSquares(IReadOnlyList<Coordinate> squares)
                : this(new[] { squares })
            {
            }

            public HardcodedSquares(IReadOnlyList<IReadOnlyList<Coordinate>> squares)
            {
                this.squares = squares;

                this.squareIndex = -1;
                this.sequenceIndex = 0;
            }

            public Coordinate SelectMove(Battleship game)
            {
                if (this.sequenceIndex >= this.squares.Count)
                {
                    return Program.DestroyShips(game);
                }

                var sequence = this.squares[this.sequenceIndex];
                if (++this.squareIndex < sequence.Count)
                {
                    var square = sequence[this.squareIndex];
                    if (Program.TryShoot(game, square.X, square.Y, out _))
                    {
                        return square;
                    }
                    else
                    {
                        return this.SelectMove(game);
                    }
                }
                else
                {
                    ++this.sequenceIndex;
                    this.squareIndex = -1;

                    return this.SelectMove(game);
                }
            }

            public static IReadOnlyList<Coordinate> _2sHeatmap { get; } = new[]
            {
                new Coordinate(4, 4),
                new Coordinate(5, 5),
                new Coordinate(6, 4),
                new Coordinate(5, 3),
                new Coordinate(3, 5),
                new Coordinate(4, 6),
                new Coordinate(6, 6),
                new Coordinate(3, 3),
                new Coordinate(5, 7),
                new Coordinate(7, 5),
                new Coordinate(4, 2),
                new Coordinate(2, 4),
                new Coordinate(2, 6),
                new Coordinate(3, 7),
                new Coordinate(7, 3),
                new Coordinate(6, 2),
                new Coordinate(2, 2),
                new Coordinate(7, 7),
                new Coordinate(1, 5),
                new Coordinate(4, 8),
                new Coordinate(8, 4),
                new Coordinate(5, 1),
                new Coordinate(3, 1),
                new Coordinate(1, 3),
                new Coordinate(6, 8),
                new Coordinate(8, 6),
                new Coordinate(1, 7),
                new Coordinate(2, 8),
                new Coordinate(8, 2),
                new Coordinate(7, 1),
                new Coordinate(4, 0),
                new Coordinate(0, 4),
                new Coordinate(5, 9),
                new Coordinate(9, 5),
                new Coordinate(9, 3),
                new Coordinate(6, 0),
                new Coordinate(0, 6),
                new Coordinate(3, 9),
                new Coordinate(8, 8),
                new Coordinate(1, 1),
                new Coordinate(0, 2),
                new Coordinate(7, 9),
                new Coordinate(9, 7),
                new Coordinate(2, 0),
                new Coordinate(0, 8),
                new Coordinate(1, 9),
                new Coordinate(9, 1),
                new Coordinate(8, 0),
                new Coordinate(0, 0),
                new Coordinate(9, 9),
            };

            public static IReadOnlyList<Coordinate> _4sHeatmap { get; } = new[]
            {
                new Coordinate(4, 0),
                new Coordinate(5, 5),
                new Coordinate(6, 6),
                new Coordinate(3, 3),
                new Coordinate(2, 6),
                new Coordinate(3, 7),
                new Coordinate(7, 3),
                new Coordinate(6, 2),
                new Coordinate(2, 2),
                new Coordinate(7, 7),
                new Coordinate(5, 1),
                new Coordinate(1, 5),
                new Coordinate(4, 8),
                new Coordinate(8, 4),
                new Coordinate(4, 0),
                new Coordinate(0, 4),
                new Coordinate(5, 9),
                new Coordinate(9, 5),
                new Coordinate(1, 1),
                new Coordinate(8, 8),
                new Coordinate(9, 1),
                new Coordinate(8, 0),
                new Coordinate(0, 8),
                new Coordinate(1, 9),
                new Coordinate(0, 0),
                new Coordinate(9, 9),
            };
        }

        public sealed class HeatmapStrategy : IStrategy<Battleship, BattleshipShotResults, Coordinate, string, Univariate<Battleship>>
        {
            private readonly (int temperate, int adjacentShots)[][] heatmap;

            private readonly HashSet<Boat> remainingBoats;

            private Coordinate? lastMove;

            private bool destroyingBoats;

            public HeatmapStrategy()
            {
                this.heatmap = new (int temperate, int adjacentShots)[10][];
                for (int i = 0; i < this.heatmap.Length; ++i)
                {
                    this.heatmap[i] = new (int temperate, int adjacentShots)[10];
                }

                this.remainingBoats = new HashSet<Boat>(
                    new[]
                    {
                        new Boat("destroyer", 2),
                        new Boat("submarine", 3),
                        new Boat("cruiser", 3),
                        new Boat("battleship", 4),
                        new Boat("carrier", 5),
                    },
                    BoatComparer.Instance);

                this.lastMove = null;
                this.destroyingBoats = false;
            }

            public Coordinate SelectMove(Battleship game)
            {
                this.lastMove = this.SelectMoveImpl(game);
                return this.lastMove;
            }

            private Coordinate SelectMoveImpl(Battleship game)
            {
                if (this.destroyingBoats)
                {
                    if (Program.TryDestroyShips(game, out var coordinate))
                    {
                        return coordinate;
                    }

                    this.destroyingBoats = false;
                }
                else if (this.lastMove != null)
                {
                    if (game.Board.Shots[this.lastMove.X][this.lastMove.Y] is BattleshipShotResult.Hit hit)
                    {
                        this.remainingBoats.Remove(hit.Boat);
                        this.destroyingBoats = true;
                        return Program.DestroyShips(game);
                    }
                }

                for (int i = 0; i < 10; ++i)
                {
                    for (int j = 0; j < 10; ++j)
                    {
                        if (game.Board.Shots[i][j] is BattleshipShotResult.NoShot)
                        {
                            var squaresToTheLeft = SquaresToTheLeft(game, i, j);
                            var squaresToTheRight = SquaresToTheRight(game, i, j);
                            var squaresAbove = SquaresAbove(game, i, j);
                            var squaresBelow = SquaresBelow(game, i, j);

                            var currentTemperature = 0;
                            foreach (var boat in this.remainingBoats)
                            {
                                currentTemperature +=
                                    Min(squaresToTheLeft, squaresToTheRight, boat.Length - 1) +
                                    Min(squaresAbove, squaresBelow, boat.Length - 1) +
                                    2;
                            }

                            var adjacentShots =
                                IsShot(game, i + 1, j) +
                                IsShot(game, i - 1, j) +
                                IsShot(game, i, j + 1) +
                                IsShot(game, i, j - 1);

                            this.heatmap[i][j] = (currentTemperature, adjacentShots);
                        }
                        else
                        {
                            this.heatmap[i][j] = (0, 0);
                        }
                    }
                }

                var (x, y) = (0, 0);
                int temperature = 0;
                int shots = 4;
                for (int i = 0; i < 10; ++i)
                {
                    for (int j = 0; j < 10; ++j)
                    {
                        var currentTemperate = this.heatmap[i][j].temperate;
                        var currentShots = this.heatmap[i][j].adjacentShots;
                        if (currentTemperate > temperature ||
                            (currentTemperate == temperature && currentShots <= shots))
                        {
                            temperature = currentTemperate;
                            shots = currentShots;
                            (x, y) = (i, j);
                        }
                    }
                }

                return new Coordinate(x, y);
            }

            private static int Min(int x, int y, int z)
            {
                return Math.Min(x, Math.Min(y, z));
            }

            private static int IsShot(Battleship game, int i, int j)
            {
                if (i < 0 || i > 9 || j < 0 || j > 9)
                {
                    return 0;
                }

                return game.Board.Shots[i][j] is BattleshipShotResult.NoShot ? 0 : 1;
            }

            private static int SquaresToTheLeft(Battleship game, int i, int j)
            {
                int left;
                for (left = j; left >= 0 && game.Board.Shots[i][left] is BattleshipShotResult.NoShot; --left)
                {
                }

                return j - left - 1;
            }

            private static int SquaresToTheRight(Battleship game, int i, int j)
            {
                int right;
                for (right = j; right < 10 && game.Board.Shots[i][right] is BattleshipShotResult.NoShot; ++right)
                {
                }

                return right - j - 1;
            }

            private static int SquaresAbove(Battleship game, int i, int j)
            {
                int above;
                for (above = i; above >= 0 && game.Board.Shots[above][j] is BattleshipShotResult.NoShot; --above)
                {
                }

                return i - above - 1;
            }

            private static int SquaresBelow(Battleship game, int i, int j)
            {
                int above;
                for (above = i; above < 10 && game.Board.Shots[above][j] is BattleshipShotResult.NoShot; ++above)
                {
                }

                return above - i - 1;
            }
        }


        private static readonly IReadOnlyList<(string, Action)> games = new (string, Action)[]
        {
            (nameof(PegsRandom), PegsRandom),
            (nameof(PegsHuman), PegsHuman),
            (nameof(PegsMonteCarlo), PegsMonteCarlo),
            (nameof(PegsDecision), PegsDecision),
            (nameof(TicTacToeHumanVersusHuman), TicTacToeHumanVersusHuman),
            (nameof(TicTacToeHumanVersusRandom), TicTacToeHumanVersusRandom),
            (nameof(TicTacToeMonteCarloVersusRandom), TicTacToeMonteCarloVersusRandom),
            (nameof(TicTacToeMonteCarloVersusHuman), TicTacToeMonteCarloVersusHuman),
            (nameof(TicTacToeDecisionVersusHuman), TicTacToeDecisionVersusHuman),
            (nameof(AmazonsHumanVersusRandom_5x6), AmazonsHumanVersusRandom_5x6),
            (nameof(AmazonsHumanVersusMonteCarlo), AmazonsHumanVersusMonteCarlo),
            (nameof(AmazonsHumanVersusMinimizeMoves), AmazonsHumanVersusMinimizeMoves),
            (nameof(AmazonsRandomVersusRandom), AmazonsRandomVersusRandom),
            (nameof(AmazonsMonteCarloVersusMinimizeMoves_5x6), AmazonsMonteCarloVersusMinimizeMoves_5x6),
            (nameof(AmazonsMonteCarloVersusMinimizeMoves_8x8), AmazonsMonteCarloVersusMinimizeMoves_8x8),
            (nameof(ConnectFourRandomVersusRandom), ConnectFourRandomVersusRandom),
            (nameof(ConnectFourRandomVersusMontyCarlo), ConnectFourRandomVersusMontyCarlo),
            (nameof(ConnectFourHumanVersusMontyCarlo), ConnectFourHumanVersusMontyCarlo),
            (nameof(ConnectFourHumanVersusHuman), ConnectFourHumanVersusHuman),
            (nameof(ConnectFourDecisionVersusHuman), ConnectFourDecisionVersusHuman),
            (nameof(BattleshipConsole), BattleshipConsole),
        };

        static void Main(string[] args)
        {
            ////Fx.Games.Game.NewAttempt.CreateOrdered();

            for (int i = 0; true; ++i)
            {
                var sku = GetSkuFromArgsOrConsole(args, i);
                Console.Clear();
                games[sku].Item2();
                Console.WriteLine();
            }
        }

        private static int GetSkuFromArgsOrConsole(string[] args, int arg)
        {
            if (args.Length > arg && int.TryParse(args[arg], out var num))
            {
                return num;
            }

            Console.WriteLine("Available games:");
            for (int i = 0; i < games.Count; ++i)
            {
                Console.WriteLine($"{i}: {games[i].Item1}");
            }

            do
            {
                Console.WriteLine();
                Console.WriteLine("Provide SKU:");
                if (int.TryParse(Console.ReadLine(), out var sku) && sku >= 0 && sku < games.Count)
                {
                    return sku;
                }
            }
            while (true);
        }

        private static void BattleshipConsole()
        {
            //// TODO now that you have all of the legal setups, you can create a "heat map" which shows, for each square, the probability that a boat is in that square; you should shoot the squares with the highest probability first //// TODO this strategy feels like it could be gamed by someone who know the heatmap

            //// TODO should you orient the boat before continuing with the distance strategy?
            //// TODO should you destroy the boat before continuing with the distance strategy?
            //// TODO you boat destruction algorithm doesn't try to leverage any knowledge of the strategy; there might be strategies that can use more optimized tactics //// TODO also, your boat destruction algorithm treats every boat the same; there might be optimizations available for boats of specific sizes

            ////var displayer = BattleshipDisplayer.Instance;
            var displayer = NullDisplayer<Battleship, BattleshipShotResults, Coordinate, string, Univariate<Battleship>>.Instance;
            var player1 = "player1";

            // interesting seeds:
            // 59 moves seed: 175297017
            // 59 moves seed: 175363003
            // 60 moves seed: 175393835
            // 61 moves seed: 300138036

            ////var ticks = 155221062;
            ////var ticks = 158349719;
            ////var ticks = 159575046;
            ////var ticks = 165841500;
            ////var ticks = 165841503;
            var ticks = Environment.TickCount;
            var average = 0;
            var length = 10000;
            for (int i = 0; i < length; ++i)
            {
                ++ticks;
                ////Console.WriteLine(ticks);
                var random = new Random(ticks);

                var filePath = "C:\\github\\battleship_board_states\\0.txt";
                BattleshipSetup setup;
                using (var file = File.OpenRead(filePath))
                {
                    var setupStore = new StreamSetupStore(file);
                    var next = random.NextInt64(0, 30_093_975_536); //// TODO add the count to the file

                    setup = setupStore.Get(next);
                }


                Func<IStrategy<Battleship, BattleshipShotResults, Coordinate, string, Univariate<Battleship>>> strategyFactory =
                    ////() => new BattleshipReverseDistance();
                    ////() => new HardcodedSquares(HardcodedSquares._2sHeatmap);
                    ////() => new HardcodedSquares(new[] { HardcodedSquares._4sHeatmap, HardcodedSquares._2sHeatmap });
                    () => new HeatmapStrategy();

                Battleship result;
                {
                    var strategy = strategyFactory();
                    ////DisplaySetup(setup); 
                    var battleship = new Battleship(setup, player1);

                    var driver = Driver.Create(
                        new[]
                        {
                        ////KeyValuePair.Create(player1, new BattleshipNaive()),
                        KeyValuePair.Create(player1, strategy),
                            ////KeyValuePair.Create(player1, BattleshipConsoleStrategy.Instance),
                            ////KeyValuePair.Create(player1, new RandomStrategy<Battleship, BattleshipShotResults, Coordinate, string, Univariate<Battleship>>()),
                        }.ToDb().ToDictionary(),
                        displayer);
                    result = driver.Run(battleship);
                }

                average += result.MoveCount;

                /*var transforms = new[] { Transform.MirrorOverX, Transform.MirrorOverY, Transform.MirrorOver45 };
                var miniAverage = result.MoveCount;
                if (result.MoveCount > 50)
                {
                    foreach (var transform in transforms)
                    {
                        var strategy = strategyFactory();
                        var transformedSetup = TransformSetup(setup, transform);
                        ////DisplaySetup(transformedSetup);
                        var battleship = new Battleship(transformedSetup, player1);

                        var driver = Driver.Create(
                            new[]
                            {
                                KeyValuePair.Create(player1, strategy),
                            }.ToDb().ToDictionary(),
                            displayer);
                        result = driver.Run(battleship);

                        miniAverage += result.MoveCount;
                    }

                    var actualAverage = miniAverage / (transforms.Length + 1);
                    if (actualAverage > 60)
                    {
                        Console.WriteLine($"A variation was found that averaged {actualAverage} moves using seed {ticks}:");
                        DisplaySetup(setup);
                    }
                }*/
            }

            Console.WriteLine(average / length);
        }

        private static void ConnectFourDecisionVersusHuman()
        {
            var displayer = new ConnectFourDisplayer<string>(_ => _);
            var player1 = "player1";
            var player2 = "player2";

            var random1 = new Random();

            var game = new ConnectFour<string>(player1, player2);
            /*var strategy = new DecisionTreeStrategy<ConnectFour<string>, ConnectFourBoard, ConnectFourMove, string, Univariate<ConnectFour<string>>>(
                player1,
                univariate => univariate.ToPortion(),
                StringComparer.OrdinalIgnoreCase, 
                -1.0);*/

            var driver = Driver.Create(
                new[]
                {
                    KeyValuePair.Create(player1, (IStrategy<ConnectFour<string>, ConnectFourBoard, ConnectFourMove, string, Univariate<ConnectFour<string>>>) null),
                    KeyValuePair.Create(player2, (IStrategy<ConnectFour<string>, ConnectFourBoard, ConnectFourMove, string, Univariate<ConnectFour<string>>>) ConsoleStrategy<ConnectFour<string>, ConnectFourBoard, ConnectFourMove, string, Univariate<ConnectFour<string>>>.Instance),
                }.ToDb().ToDictionary(),
                displayer);
            var result = driver.Run(game);
        }

        private static void ConnectFourHumanVersusHuman()
        {
            var displayer = new ConnectFourDisplayer<string>(_ => _);
            var player1 = "player1";
            var player2 = "player2";

            var random1 = new Random();

            var game = new ConnectFour<string>(player1, player2);

            var driver = Driver.Create(
                new[]
                {
                    KeyValuePair.Create(player1, (IStrategy<ConnectFour<string>, ConnectFourBoard, ConnectFourMove, string, Univariate<ConnectFour<string>> >) ConsoleStrategy<ConnectFour<string>, ConnectFourBoard, ConnectFourMove, string, Univariate<ConnectFour<string>>>.Instance),
                    KeyValuePair.Create(player2, (IStrategy<ConnectFour<string>, ConnectFourBoard, ConnectFourMove, string, Univariate<ConnectFour<string>>>) ConsoleStrategy<ConnectFour<string>, ConnectFourBoard, ConnectFourMove, string, Univariate<ConnectFour<string>>>.Instance),
                }.ToDb().ToDictionary(),
                displayer);
            var result = driver.Run(game);
        }

        private static void ConnectFourHumanVersusMontyCarlo()
        {
            var displayer = new ConnectFourDisplayer<string>(_ => _);
            var player1 = "player1";
            var player2 = "player2";

            var random1 = new Random();

            var game = new ConnectFour<string>(player1, player2);

            var driver = Driver.Create(
                new[]
                {
                    KeyValuePair.Create(player1, (IStrategy<ConnectFour<string>, ConnectFourBoard, ConnectFourMove, string, Univariate<ConnectFour<string>>>) ConsoleStrategy<ConnectFour<string>, ConnectFourBoard, ConnectFourMove, string, Univariate<ConnectFour<string>>>.Instance),
                    KeyValuePair.Create(player2, (IStrategy<ConnectFour<string>, ConnectFourBoard, ConnectFourMove, string, Univariate<ConnectFour<string>>>)game.MonteCarloStrategy(player2, 1000000, game.MonteCarloStrategySettings())),
                }.ToDb().ToDictionary(),
                displayer);
            var result = driver.Run(game);
        }

        private static void ConnectFourRandomVersusMontyCarlo()
        {
            var displayer = new ConnectFourDisplayer<string>(_ => _);
            var player1 = "player1";
            var player2 = "player2";

            var random1 = new Random();

            var game = new ConnectFour<string>(player1, player2);

            var driver = Driver.Create(
                new[]
                {
                    KeyValuePair.Create(player1, (IStrategy<ConnectFour<string>, ConnectFourBoard, ConnectFourMove, string, Univariate<ConnectFour<string>>>)new RandomStrategy<ConnectFour<string>, ConnectFourBoard, ConnectFourMove, string, Univariate<ConnectFour<string>>>(new RandomStrategySettings<ConnectFour<string>, ConnectFourBoard, ConnectFourMove, string, Univariate<ConnectFour<string>>>.Builder() {Random = random1 }.Build())),
                    KeyValuePair.Create(player2, (IStrategy<ConnectFour<string>, ConnectFourBoard, ConnectFourMove, string, Univariate<ConnectFour<string>>>)game.MonteCarloStrategy(player2, 1000, game.MonteCarloStrategySettings())),
                }.ToDb().ToDictionary(),
                displayer);
            var result = driver.Run(game);
        }

        private static void ConnectFourRandomVersusRandom()
        {
            var displayer = new ConnectFourDisplayer<string>(_ => _);
            var player1 = "player1";
            var player2 = "player2";

            var random1 = new Random(Environment.TickCount);
            var random2 = new Random(Environment.TickCount);

            var game = new ConnectFour<string>(player1, player2);
            var driver = Driver.Create(
                new[]
                {
                    KeyValuePair.Create(player1, (IStrategy<ConnectFour<string>, ConnectFourBoard, ConnectFourMove, string, Univariate<ConnectFour<string>>>)new RandomStrategy<ConnectFour<string>, ConnectFourBoard, ConnectFourMove, string, Univariate<ConnectFour<string>>>(new RandomStrategySettings<ConnectFour<string>, ConnectFourBoard, ConnectFourMove, string, Univariate<ConnectFour<string>>>.Builder() {Random = random1 }.Build())),
                    KeyValuePair.Create(player2, (IStrategy<ConnectFour<string>, ConnectFourBoard, ConnectFourMove, string, Univariate<ConnectFour<string>>>)new RandomStrategy<ConnectFour<string>, ConnectFourBoard, ConnectFourMove, string, Univariate<ConnectFour<string>>>(new RandomStrategySettings<ConnectFour<string>, ConnectFourBoard, ConnectFourMove, string, Univariate<ConnectFour<string>>>.Builder() {Random = random2 }.Build())),
                }.ToDb().ToDictionary(),
                displayer);
            var result = driver.Run(game);
        }

        private static void AmazonsMonteCarloVersusMinimizeMoves_8x8()
        {
            var white = "white";
            var black = "black";
            var game = new Amazons.Game<string>(white, black, (8, 8));
            var displayer = new Amazons.Displayer<string>(_ => _);
            var strategies = new[] {
                KeyValuePair.Create<string,IStrategy<Amazons.Game<string>, Amazons.Board, Amazons.Move, string, Univariate<Amazons.Game<string>>>>(white, game.MonteCarloStrategy(white, 100000, game.MonteCarloStrategySettings())),
                KeyValuePair.Create<string,IStrategy<Amazons.Game<string>, Amazons.Board, Amazons.Move, string, Univariate<Amazons.Game<string>>>>(black, game.MinimizeMovesStrategy()),
            };
            var driver = new Driver<Amazons.Game<string>, Amazons.Board, Amazons.Move, string, Univariate<Amazons.Game<string>>>(
                strategies.ToDb().ToDictionary(),
                displayer);

            var result = driver.Run(game);
        }

        private static void AmazonsMonteCarloVersusMinimizeMoves_5x6()
        {
            var white = "white";
            var black = "black";
            var game = new Amazons.Game<string>(white, black, (5, 6));
            var displayer = new Amazons.Displayer<string>(_ => _);
            var strategies = new[] {
                KeyValuePair.Create<string,IStrategy<Amazons.Game<string>, Amazons.Board, Amazons.Move, string, Univariate<Amazons.Game<string>>>>(white, game.MonteCarloStrategy(white, 100000, game.MonteCarloStrategySettings())),
                KeyValuePair.Create<string,IStrategy<Amazons.Game<string>, Amazons.Board, Amazons.Move, string, Univariate<Amazons.Game<string>>>>(black, game.MinimizeMovesStrategy()),
            };
            var driver = new Driver<Amazons.Game<string>, Amazons.Board, Amazons.Move, string, Univariate<Amazons.Game<string>>>(
                strategies.ToDb().ToDictionary(),
                displayer);

            var result = driver.Run(game);
        }

        private static void AmazonsRandomVersusRandom()
        {
            var white = "white";
            var black = "black";
            var game = new Amazons.Game<string>(white, black);
            var displayer = new Amazons.Displayer<string>(_ => _);
            var strategies = new[] {
                KeyValuePair.Create(white, game.RandomStrategy()),
                KeyValuePair.Create(black, game.RandomStrategy()),
            };
            var driver = new Driver<Amazons.Game<string>, Amazons.Board, Amazons.Move, string, Univariate<Amazons.Game<string>>>(
                strategies.ToDb().ToDictionary(),
                displayer);

            var result = driver.Run(game);
        }

        private static void AmazonsHumanVersusMinimizeMoves()
        {
            var displayer = new Amazons.Displayer<string>(_ => _);

            var white = "white";
            var black = "black";
            var game = new Amazons.Game<string>(white, black, (5, 6));
            var strategies = new[] {
                KeyValuePair.Create<string,IStrategy<Amazons.Game<string>, Amazons.Board, Amazons.Move, string, Univariate<Amazons.Game<string>>>>(white, game.AmazonsConsoleStrategy()),
                KeyValuePair.Create<string,IStrategy<Amazons.Game<string>, Amazons.Board, Amazons.Move, string, Univariate<Amazons.Game<string>>>>(black, game.MinimizeMovesStrategy()),
            };
            var driver = new Driver<Amazons.Game<string>, Amazons.Board, Amazons.Move, string, Univariate<Amazons.Game<string>>>(
                strategies.ToDb().ToDictionary(),
                displayer);

            var result = driver.Run(game);
        }

        private static void AmazonsHumanVersusMonteCarlo()
        {
            var displayer = new Amazons.Displayer<string>(_ => _);

            var white = "white";
            var black = "black";
            var game = new Amazons.Game<string>(white, black, (5, 6));
            var strategies = new[] {
                KeyValuePair.Create<string,IStrategy<Amazons.Game<string>, Amazons.Board, Amazons.Move, string, Univariate<Amazons.Game<string>>>>(white, game.AmazonsConsoleStrategy()),
                KeyValuePair.Create<string,IStrategy<Amazons.Game<string>, Amazons.Board, Amazons.Move, string, Univariate<Amazons.Game<string>>>>(black, game.MonteCarloStrategy(black, 100000, game.MonteCarloStrategySettings())),
            };
            var driver = new Driver<Amazons.Game<string>, Amazons.Board, Amazons.Move, string, Univariate<Amazons.Game<string>>>(
                strategies.ToDb().ToDictionary(),
                displayer);

            var result = driver.Run(game);
        }

        private static void AmazonsHumanVersusRandom_5x6()
        {
            var displayer = new Amazons.Displayer<string>(_ => _);

            var white = "white";
            var black = "black";
            var game = new Amazons.Game<string>(white, black, (5, 6));
            var strategies = new[] {
                KeyValuePair.Create<string,IStrategy<Amazons.Game<string>, Amazons.Board, Amazons.Move, string, Univariate<Amazons.Game<string>>>>(white, game.AmazonsConsoleStrategy()),
                KeyValuePair.Create<string,IStrategy<Amazons.Game<string>, Amazons.Board, Amazons.Move, string, Univariate<Amazons.Game<string>>>>(black, game.RandomStrategy()),
            };
            var driver = new Driver<Amazons.Game<string>, Amazons.Board, Amazons.Move, string, Univariate<Amazons.Game<string>>>(
                strategies.ToDb().ToDictionary(),
                displayer);

            var result = driver.Run(game);
        }

        private static void TicTacToeDecisionVersusHuman()
        {
            var displayer = new TicTacToeConsoleDisplayer<string>(_ => _);
            var exes = "computer";
            var ohs = "ohs";

            var game = new TicTacToe<string>(exes, ohs);
            /*var strategy = new DecisionTreeStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string, Univariate<TicTacToe<string>>>(
                exes, 
                univariate => univariate.ToPortion(),
                StringComparer.OrdinalIgnoreCase, 
                -1.0);*/

            var driver = Driver.Create(
                new[]
                {
                    KeyValuePair.Create(exes, (IStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string, Univariate<TicTacToe<string>>>)null),
                    KeyValuePair.Create(ohs, (IStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string, Univariate<TicTacToe<string>>>)game.ConsoleStrategy()),
                }.ToDb().ToDictionary(),
                displayer);
            var result = driver.Run(game);
        }

        private static void TicTacToeMonteCarloVersusHuman()
        {
            var displayer = new TicTacToeConsoleDisplayer<string>(_ => _);
            var exes = "exes";
            var ohs = "ohs";

            var game = new TicTacToe<string>(exes, ohs);
            var driver = Driver.Create(
                new[]
                {
                    KeyValuePair.Create(exes, (IStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string, Univariate<TicTacToe<string>>>)game.MonteCarloStrategy(exes, 1000, game.MonteCarloStrategySettings())),
                    KeyValuePair.Create(ohs, (IStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string, Univariate<TicTacToe<string>>>)game.ConsoleStrategy()),
                }.ToDb().ToDictionary(),
                displayer);
            var result = driver.Run(game);
        }

        private static void TicTacToeMonteCarloVersusRandom()
        {
            var displayer = new TicTacToeConsoleDisplayer<string>(_ => _);
            var exes = "exes";
            var ohs = "ohs";

            var game = new TicTacToe<string>(exes, ohs);
            var driver = Driver.Create(
                new[]
                {
                    KeyValuePair.Create(exes, (IStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string, Univariate<TicTacToe<string>>>)game.MonteCarloStrategy(exes, 1000, game.MonteCarloStrategySettings())),
                    KeyValuePair.Create(ohs, (IStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string, Univariate<TicTacToe<string>>>)game.RandomStrategy()),
                }.ToDb().ToDictionary(),
                displayer);
            var result = driver.Run(game);
        }

        private static void TicTacToeHumanVersusRandom()
        {
            var displayer = new TicTacToeConsoleDisplayer<string>(_ => _);
            var exes = "exes";
            var ohs = "ohs";

            var game = new TicTacToe<string>(exes, ohs);
            var driver = Fx.Games.Driver.Driver.Create(
                (new[]
                {
                    KeyValuePair.Create(exes, (IStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string, Univariate<TicTacToe<string>>>)game.ConsoleStrategy()),
                    KeyValuePair.Create(ohs, (IStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string, Univariate<TicTacToe<string>>>)game.RandomStrategy()),
                }).ToDb().ToDictionary(),
                displayer);
            var result = driver.Run(game);
        }

        private static void TicTacToeHumanVersusHuman()
        {
            var displayer = new TicTacToeConsoleDisplayer<string>(_ => _);
            var exes = "exes";
            var ohs = "ohs";

            var game = new TicTacToe<string>(exes, ohs);
            var driver = Fx.Games.Driver.Driver.Create(
                (new[]
                {
                    KeyValuePair.Create(exes, game.ConsoleStrategy()),
                    KeyValuePair.Create(ohs, game.ConsoleStrategy()),
                }).ToDb().ToDictionary(),
                displayer);
            var result = driver.Run(game);
        }

        private static void PegsDecision()
        {
            var displayer = PegGameConsoleDisplayer<string>.Instance;
            var player = "player";
            var game = new PegGame<string>(player);

            /*var strategy = new DecisionTreeStrategy<PegGame<string>, PegBoard, PegMove, string, Univariate<PegGame<string>>>(
                player,
                univariate => univariate.ToPortion(),
                StringComparer.OrdinalIgnoreCase,
                0.5);*/

            /*var driver = Driver.Create(
                new[] //// TODO use a fluent builder?
                {
                    KeyValuePair.Create(player, null),
                }.ToDb().ToDictionary(),
                displayer);
            var result = driver.Run(game);*/
        }

        private static void PegsMonteCarlo()
        {
            var displayer = PegGameConsoleDisplayer<string>.Instance;
            var player = "player";
            var game = new PegGame<string>(player);
            var driver = Driver.Create(
                new[] //// TODO use a fluent builder?
                {
                    KeyValuePair.Create(player, game.MonteCarloStrategy(player, 10000, game.MonteCarloStrategySettings())),
                }.ToDb().ToDictionary(),
                displayer);
            var result = driver.Run(game);
        }

        private static void PegsHuman()
        {
            var displayer = PegGameConsoleDisplayer<string>.Instance;
            var player = "player";
            var game = new PegGame<string>(player);
            var driver = Fx.Games.Driver.Driver.Create(
                (new[] //// TODO use a fluent builder?
                {
                    KeyValuePair.Create(player, game.ConsoleStrategy()),
                }).ToDb().ToDictionary(),
                displayer);
            var result = driver.Run(game);
        }

        private static void PegsRandom()
        {
            var displayer = PegGameConsoleDisplayer<string>.Instance;
            var player = "random";
            var game = new PegGame<string>(player);
            var driver = Fx.Games.Driver.Driver.Create(
                (new[]
                {
                    KeyValuePair.Create(player, game.RandomStrategy()),
                }).ToDb().ToDictionary(),
                displayer);
            var result = driver.Run(game);
        }
    }
}
