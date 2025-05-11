namespace NuvoChess.BoardState;

public static class PieceType
{
    public const byte PawnPiece = 1;
    public const byte KnightPiece = 2;
    public const byte BishopPiece = 3;
    public const byte RookPiece = 4;
    public const byte QueenPiece = 5;
    public const byte KingPiece = 6;
    public const byte WhitePiece = 8;
    public const byte BlackPiece = 16;

    public static bool IsPawnPiece(byte piece)
    {
        return (piece & PawnPiece) == PawnPiece;
    }

    public static bool IsKnightPiece(byte piece)
    {
        return (piece & KnightPiece) == KnightPiece;
    }

    public static bool IsBishopPiece(byte piece)
    {
        return (piece & BishopPiece) == BishopPiece;
    }

    public static bool IsRookPiece(byte piece)
    {
        return (piece & RookPiece) == RookPiece;
    }

    public static bool IsQueenPiece(byte piece)
    {
        return (piece & QueenPiece) == QueenPiece;
    }

    public static bool IsKingPiece(byte piece)
    {
        return (piece & KingPiece) == KingPiece;
    }

    public static bool IsWhitePiece(byte piece)
    {
        return (piece & WhitePiece) == WhitePiece;
    }

    public static bool IsBlackPiece(byte piece)
    {
        return (piece & BlackPiece) == BlackPiece;
    }
}