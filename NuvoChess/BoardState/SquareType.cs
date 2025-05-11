namespace NuvoChess.BoardState;

public static class SquareType
{
    public const byte EmptySquare = 0;
    public const byte OffBoardSquare = 1;

    public static bool ContainsPiece(byte square)
    {
        return square > OffBoardSquare;
    }
}
