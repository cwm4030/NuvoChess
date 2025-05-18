namespace NuvoChess.BoardState;

public static class PieceType
{
    public const byte EmptySquare = 0;
    public const byte GuardSquare = 1;
    public const byte PawnPiece = 2;
    public const byte KnightPiece = 4;
    public const byte BishopPiece = 6;
    public const byte RookPiece = 8;
    public const byte QueenPiece = 10;
    public const byte KingPiece = 12;
    public const byte WhitePiece = 16;
    public const byte BlackPiece = 32;
    public const byte CapturedPiece = 64;
    public const byte PieceMask = 14;
    public const byte ColorMask = 48;

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

    public static bool IsSameColorPiece(byte piece1, byte piece2)
    {
        return (piece1 & piece2 & ColorMask) != 0;
    }

    public static bool IsWhitePiece(byte piece)
    {
        return (piece & WhitePiece) == WhitePiece;
    }

    public static bool IsBlackPiece(byte piece)
    {
        return (piece & BlackPiece) == BlackPiece;
    }

    public static bool IsCapturedPiece(byte piece)
    {
        return (piece & CapturedPiece) == CapturedPiece;
    }

    public static bool IsPiece(byte piece)
    {
        return (piece & PieceMask) != EmptySquare;
    }
}