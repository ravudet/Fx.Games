namespace ConsoleApplication1
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
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

        public sealed class CoordinateComparer : IEqualityComparer<Coordinate>
        {
            private CoordinateComparer()
            {
            }

            public static CoordinateComparer Instance { get; } = new CoordinateComparer();

            public bool Equals(Coordinate? x, Coordinate? y)
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

                return x.X == y.X && x.Y == y.Y;
            }

            public int GetHashCode(Coordinate obj)
            {
                return obj.X.GetHashCode() ^ obj.Y.GetHashCode();
            }
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

        public sealed class PlacementComparer : IEqualityComparer<Placement>
        {
            private PlacementComparer()
            {
            }

            public static PlacementComparer Instance { get; } = new PlacementComparer();

            public bool Equals(Placement? x, Placement? y)
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

                return BoatComparer.Instance.Equals(x.Boat, y.Boat) && CoordinateComparer.Instance.Equals(x.Coordinate, y.Coordinate) && x.LeftToRight == y.LeftToRight;
            }

            public int GetHashCode(Placement obj)
            {
                return BoatComparer.Instance.GetHashCode(obj.Boat) ^ CoordinateComparer.Instance.GetHashCode(obj.Coordinate) ^ obj.LeftToRight.GetHashCode();
            }
        }

        public sealed class BoardState
        {
            public BoardState(System.Collections.Generic.IEnumerable<Placement> placements)
            {
                Placements = placements;
            }

            public System.Collections.Generic.IEnumerable<Placement> Placements { get; }
        }

        public sealed class BoardStateComparer : IEqualityComparer<BoardState>
        {
            private BoardStateComparer()
            {
            }

            public static BoardStateComparer Instance { get; } = new BoardStateComparer();

            public bool Equals(BoardState? x, BoardState? y)
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

                return Enumerable.SequenceEqual(x.Placements, y.Placements, PlacementComparer.Instance);
            }

            public int GetHashCode(BoardState obj)
            {
                var hashCode = 0;
                foreach (var placement in obj.Placements)
                {
                    hashCode ^= PlacementComparer.Instance.GetHashCode(placement);
                }

                return hashCode;
            }
        }

        public static bool Fits(Placement placement)
        {
            if (placement.LeftToRight)
            {
                if (placement.Coordinate.X + placement.Boat.Length >= 10)
                {
                    return false;
                }
            }
            else
            {
                if (placement.Coordinate.Y + placement.Boat.Length >= 10)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool Overlaps(System.Collections.Generic.IEnumerable<System.Collections.Generic.IEnumerable<Coordinate>> boardState, Placement placement)
        {
            var newBoatCoordinates = BoatCoordinates(placement).ToHashSet(CoordinateComparer.Instance);

            foreach (var boat in boardState)
            {
                var boatCoordinates = boat;
                if (newBoatCoordinates.Intersect(boatCoordinates).Any())
                {
                    return true;
                }
            }

            return false;
        }

        private static System.Collections.Generic.IEnumerable<Coordinate> BoatCoordinates(Placement placement)
        {
            if (placement.LeftToRight)
            {
                for (int i = 0; i < placement.Boat.Length; ++i)
                {
                    yield return new Coordinate(placement.Coordinate.X + i, placement.Coordinate.Y);
                }
            }
            else
            {
                for (int i = 0; i < placement.Boat.Length; ++i)
                {
                    yield return new Coordinate(placement.Coordinate.X, placement.Coordinate.Y + i);
                }
            }
        }



        public sealed class BetterBoard
        {
            public BetterBoard(char[][] board)
            {
                Board = board;
            }

            public char[][] Board { get; }
        }

        private static void DoWork2()
        {
            var boats = new[]
            {
                new Boat("carrier", 5),
                new Boat("battleship", 4),
                new Boat("submarine", 3),
                new Boat("cruiser", 3),
                new Boat("destroyer", 2),
            };

            System.Collections.Generic.IEnumerable<BetterBoard> existingBoardStates = new[] { new BetterBoard(new[] { new char[10], new char[10], new char[10], new char[10], new char[10], new char[10], new char[10], new char[10], new char[10], new char[10] }) };
            foreach (var boat in boats)
            {
                var newBoardStates = new List<BetterBoard>();
                int count = 0;
                foreach (var boardState in existingBoardStates)
                {
                    foreach (var leftToRight in new[] { true, false })
                    {
                        for (int i = 0; i < 10; ++i)
                        {
                            for (int j = 0; j < 10; ++j)
                            {
                                if (boardState.Board[i][j] == 0)
                                {
                                    if (leftToRight)
                                    {
                                        var keep = true;
                                        for (int k = 1; k < boat.Length; ++k)
                                        {
                                            if (i + k >= 10 || boardState.Board[i + k][j] != 0)
                                            {
                                                keep = false;
                                                break;
                                            }
                                        }

                                        if (keep)
                                        {
                                            var newBoardState = boardState.Board.Select(row => (row.Clone() as char[])!).ToArray();
                                            for (int k = 0; k < boat.Length; ++k)
                                            {
                                                newBoardState[i + k][j] = boat.Name[0];
                                            }

                                            newBoardStates.Add(new BetterBoard(newBoardState));
                                        }
                                    }
                                    else
                                    {
                                        var keep = true;
                                        for (int k = 1; k < boat.Length; ++k)
                                        {
                                            if (j + k >= 10 || boardState.Board[i][j + k] != 0)
                                            {
                                                keep = false;
                                                break;
                                            }
                                        }

                                        if (keep)
                                        {
                                            var newBoardState = boardState.Board.Select(row => (row.Clone() as char[])!).ToArray();
                                            for (int k = 0; k < boat.Length; ++k)
                                            {
                                                newBoardState[i][j + k] = boat.Name[0];
                                            }

                                            newBoardStates.Add(new BetterBoard(newBoardState));
                                        }
                                    }
                                }
                            }
                        }
                    }

                    ++count;
                }

                existingBoardStates = newBoardStates;
            }

            Console.WriteLine(existingBoardStates.Count());
        }

        private static void DoWork()
        {
            var boats = new[]
            {
                new Boat("carrier", 5),
                new Boat("battleship", 4),
                new Boat("submarine", 3),
                new Boat("cruiser", 3),
                new Boat("destroyer", 2),
            };

            List<System.Collections.Generic.IEnumerable<List<Coordinate>>> existingBoardStates =
                PossiblePlacements(boats[0])
                .Select(placement => new BoardState(new[] { placement }))
                .Select(boardState => boardState.Placements.Select(placement => BoatCoordinates(placement).ToList()).ToList().AsEnumerable())
                .ToList();
            foreach (var boat in boats.Skip(1))
            {
                var newBoardStates = new List<System.Collections.Generic.IEnumerable<List<Coordinate>>>();
                foreach (var placement in PossiblePlacements(boat))
                {
                    foreach (var boardState in existingBoardStates)
                    {
                        if (!Overlaps(boardState, placement))
                        {
                            newBoardStates.Add(boardState.Append(BoatCoordinates(placement).ToList()).ToList());
                        }
                    }
                }

                existingBoardStates = newBoardStates;
            }

            Console.WriteLine(existingBoardStates.Count());
        }

        private static System.Collections.Generic.IEnumerable<Placement> PossiblePlacements(Boat boat)
        {
            for (int i = 0; i < 10; ++i)
            {
                for (int j = 0; j < 10; ++j)
                {
                    foreach (var orientation in new[] { true, false })
                    {
                        var placement = new Placement(boat, new Coordinate(i, j), orientation);
                        if (Fits(placement))
                        {
                            yield return placement;
                        }
                    }
                }
            }
        }

        private static void DoWork3()
        {
            var start = DateTime.UtcNow;
            var workingDirectory = "C:\\github\\battleship_board_states";

            long id = 0;
            var filePath = Path.Combine(workingDirectory, $"{id}.txt");
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
            var destroyer = PossiblePlacements(2).ToList();
            var cruiser = PossiblePlacements(3).ToList();
            var submarine = PossiblePlacements(3).ToList();
            var battleship = PossiblePlacements(4).ToList();
            var carrier = PossiblePlacements(5).ToList();

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

        public static void DoWork4()
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
            }

            public Coordinate SelectMove(Battleship game)
            {
                if (this.lastMove == null)
                {
                    var move = new Coordinate(0, 0);
                    this.lastMove = move;
                    return this.lastMove;
                }

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
                                this.lastMove = DestroyShips(game);
                                return this.lastMove;
                            }
                        }
                    }
                }

                var nextY = this.lastMove.Y + distance;
                if (nextY < 10)
                {
                    this.lastMove = new Coordinate(this.lastMove.X, nextY);
                    return this.lastMove;
                }

                var nextX = this.lastMove.X + 1;
                for (nextY = 0; nextY < 10; ++nextY)
                {
                    if (nextX - (distance - 1) >= 0 && game.Board.Shots[nextX - (distance - 1)][nextY] is BattleshipShotResult.NoShot)
                    {
                        this.lastMove = new Coordinate(nextX, nextY);
                        return this.lastMove;
                    }
                }

                throw new Exception("TODO");
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
            ////DoWork4();

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
            var displayer = BattleshipDisplayer.Instance;
            var player1 = "player1";

            var ticks = Environment.TickCount;
            Console.WriteLine(ticks);
            var random = new Random(ticks);
            var filePath = "C:\\github\\battleship_board_states\\0.txt";
            Battleship battleship;
            using (var file = File.OpenRead(filePath))
            {
                var setupStore = new StreamSetupStore(file);
                var next = random.NextInt64(0, 30_093_975_536); //// TODO add the count to the file

                var setup = setupStore.Get(next);
                //// DisplaySetup(setup);

                battleship = new Battleship(setup, player1);
            }

            var driver = Driver.Create(
                new[]
                {
                    KeyValuePair.Create(player1, new BattleshipNaive()),
                    ////KeyValuePair.Create(player1, BattleshipConsoleStrategy.Instance),
                    ////KeyValuePair.Create(player1, new RandomStrategy<Battleship, BattleshipShotResults, Coordinate, string, Univariate<Battleship>>()),
                }.ToDb().ToDictionary(),
                displayer);
            var result = driver.Run(battleship);
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
