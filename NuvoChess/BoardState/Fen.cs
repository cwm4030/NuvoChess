using System.Collections.Frozen;

namespace NuvoChess.BoardState;

public static class Fen
{
    public static FrozenDictionary<byte, char> PieceToFen { get; } = new Dictionary<byte, char>
    {
        { Square.Pawn | Square.ColorFlag, '♟' },
        { Square.Knight | Square.ColorFlag, '♞' },
        { Square.Bishop | Square.ColorFlag, '♝' },
        { Square.Rook | Square.ColorFlag, '♜' },
        { Square.Queen | Square.ColorFlag, '♛' },
        { Square.King | Square.ColorFlag, '♚' },
        { Square.Pawn, '♟' },
        { Square.Knight, '♞' },
        { Square.Bishop, '♝' },
        { Square.Rook, '♜' },
        { Square.Queen, '♛' },
        { Square.King, '♚' }
    }.ToFrozenDictionary();

    public static FrozenDictionary<char, (byte, bool)> FenPieces { get; } = new Dictionary<char, (byte, bool)>
    {
        { 'p', (Square.Pawn | Square.ColorFlag, true) },
        { 'n', (Square.Knight | Square.ColorFlag, true) },
        { 'b', (Square.Bishop | Square.ColorFlag, true) },
        { 'r', (Square.Rook | Square.ColorFlag, true) },
        { 'q', (Square.Queen | Square.ColorFlag, true) },
        { 'k', (Square.King | Square.ColorFlag, true) },
        { 'P', (Square.Pawn, true) },
        { 'N', (Square.Knight, true) },
        { 'B', (Square.Bishop, true) },
        { 'R', (Square.Rook, true) },
        { 'Q', (Square.Queen, true) },
        { 'K', (Square.King, true) },
        { '/', (0, false) },
        { '1', (1, false) },
        { '2', (2, false) },
        { '3', (3, false) },
        { '4', (4, false) },
        { '5', (5, false) },
        { '6', (6, false) },
        { '7', (7, false) },
        { '8', (8, false) },
    }.ToFrozenDictionary();

    public static string[] SquareNames { get; } =
    [
        "a8", "b8", "c8", "d8", "e8", "f8", "g8", "h8",
        "a7", "b7", "c7", "d7", "e7", "f7", "g7", "h7",
        "a6", "b6", "c6", "d6", "e6", "f6", "g6", "h6",
        "a5", "b5", "c5", "d5", "e5", "f5", "g5", "h5",
        "a4", "b4", "c4", "d4", "e4", "f4", "g4", "h4",
        "a3", "b3", "c3", "d3", "e3", "f3", "g3", "h3",
        "a2", "b2", "c2", "d2", "e2", "f2", "g2", "h2",
        "a1", "b1", "c1", "d1", "e1", "f1", "g1", "h1",
    ];

    public static int[] OnBoardSquareIndexes { get; } =
    [
        68, 69, 70, 71, 72, 73, 74, 75,
        84, 85, 86, 87, 88, 89, 90, 91,
        100, 101, 102, 103, 104, 105, 106, 107,
        116, 117, 118, 119, 120, 121, 122, 123,
        132, 133, 134, 135, 136, 137, 138, 139,
        148, 149, 150, 151, 152, 153, 154, 155,
        164, 165, 166, 167, 168, 169, 170, 171,
        180, 181, 182, 183, 184, 185, 186, 187
    ];

    public static FrozenDictionary<string, int> SquareNameToIndex { get; } = SquareNames.Zip(OnBoardSquareIndexes, (k, v) => (k, v)).ToDictionary(x => x.k, x => x.v).ToFrozenDictionary();
    public static FrozenDictionary<int, string> SquareIndexToName { get; } = OnBoardSquareIndexes.Zip(SquareNames, (k, v) => (k, v)).ToDictionary(x => x.k, x => x.v).ToFrozenDictionary();
}