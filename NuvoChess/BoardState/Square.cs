namespace NuvoChess.BoardState;

public static class Square
{
    public const byte Empty = 0;            // 00000000
    public const byte Pawn = 1;             // 00000001
    public const byte Knight = 2;           // 00000010
    public const byte Bishop = 3;           // 00000011
    public const byte Rook = 4;             // 00000100
    public const byte Queen = 5;            // 00000101
    public const byte King = 6;             // 00000110
    public const byte HasMovedFlag = 8;     // 00001000
    public const byte CanCastleFlag = 16;   // 00010000
    public const byte ColorFlag = 32;       // 00100000
    public const byte OffBoardFlag = 64;    // 01000000
    public const byte PieceBits = 39;       // 00100111

    public static bool IsOffBoard(byte piece)
    {
        return (piece & OffBoardFlag) == OffBoardFlag;
    }

    public static bool IsColorWhite(byte piece)
    {
        return (piece & ColorFlag) != ColorFlag;
    }

    public static bool CanCastle(byte piece)
    {
        return (piece & CanCastleFlag) == CanCastleFlag;
    }

    public static bool HasMoved(byte piece)
    {
        return (piece & HasMovedFlag) == HasMovedFlag;
    }

    public static bool IsKing(byte piece)
    {
        return (piece & King) == King;
    }

    public static bool IsQueen(byte piece)
    {
        return (piece & Queen) == Queen;
    }

    public static bool IsRook(byte piece)
    {
        return (piece & Rook) == Rook;
    }

    public static bool IsBishop(byte piece)
    {
        return (piece & Bishop) == Bishop;
    }

    public static bool IsKnight(byte piece)
    {
        return (piece & Knight) == Knight;
    }

    public static bool IsPawn(byte piece)
    {
        return (piece & Pawn) == Pawn;
    }

    public static bool IsEmpty(byte piece)
    {
        return (piece & Empty) == Empty;
    }
}
