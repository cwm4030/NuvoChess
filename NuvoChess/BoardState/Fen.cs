using System.Collections.Frozen;

namespace NuvoChess.BoardState;

public static class Fen
{
    public static FrozenDictionary<byte, char> PieceToFen { get; } = new Dictionary<byte, char>
    {
        { PieceType.WhitePiece | PieceType.PawnPiece, '♟' },
        { PieceType.WhitePiece | PieceType.KnightPiece, '♞' },
        { PieceType.WhitePiece | PieceType.BishopPiece, '♝' },
        { PieceType.WhitePiece | PieceType.RookPiece, '♜' },
        { PieceType.WhitePiece | PieceType.QueenPiece, '♛' },
        { PieceType.WhitePiece | PieceType.KingPiece, '♚' },
        { PieceType.BlackPiece | PieceType.PawnPiece, '♟' },
        { PieceType.BlackPiece | PieceType.KnightPiece, '♞' },
        { PieceType.BlackPiece | PieceType.BishopPiece, '♝' },
        { PieceType.BlackPiece | PieceType.RookPiece, '♜' },
        { PieceType.BlackPiece | PieceType.QueenPiece, '♛' },
        { PieceType.BlackPiece | PieceType.KingPiece, '♚' }
    }.ToFrozenDictionary();

    public static FrozenDictionary<char, (byte, bool)> FenPieces { get; } = new Dictionary<char, (byte, bool)>
    {
        { 'P', (PieceType.WhitePiece | PieceType.PawnPiece, true) },
        { 'N', (PieceType.WhitePiece | PieceType.KnightPiece, true) },
        { 'B', (PieceType.WhitePiece | PieceType.BishopPiece, true) },
        { 'R', (PieceType.WhitePiece | PieceType.RookPiece, true) },
        { 'Q', (PieceType.WhitePiece | PieceType.QueenPiece, true) },
        { 'K', (PieceType.WhitePiece | PieceType.KingPiece, true) },
        { 'p', (PieceType.BlackPiece | PieceType.PawnPiece, true) },
        { 'n', (PieceType.BlackPiece | PieceType.KnightPiece, true) },
        { 'b', (PieceType.BlackPiece | PieceType.BishopPiece, true) },
        { 'r', (PieceType.BlackPiece | PieceType.RookPiece, true) },
        { 'q', (PieceType.BlackPiece | PieceType.QueenPiece, true) },
        { 'k', (PieceType.BlackPiece | PieceType.KingPiece, true) },
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
}