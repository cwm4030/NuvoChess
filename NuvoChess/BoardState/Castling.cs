using System.Text;

namespace NuvoChess.BoardState;

public static class Castling
{
    public const byte WhiteKing = 1;
    public const byte WhiteQueen = 2;
    public const byte BlackKing = 4;
    public const byte BlackQueen = 8;

    public static string ToString(byte castleRights)
    {
        var castleRightsString = new StringBuilder()
            .Append((castleRights & WhiteKing) == WhiteKing ? "K" : string.Empty)
            .Append((castleRights & WhiteQueen) == WhiteQueen ? "Q" : string.Empty)
            .Append((castleRights & BlackKing) == BlackKing ? "k" : string.Empty)
            .Append((castleRights & BlackQueen) == BlackQueen ? "q" : string.Empty)
            .ToString();
        return string.IsNullOrEmpty(castleRightsString) ? "-" : castleRightsString;
    }
}