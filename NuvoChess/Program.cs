using NuvoChess.BoardState;
using NuvoChess.UniversalChessInterface;

namespace NuvoChess;

public static class Program
{
    public static void Main()
    {
        Span<Piece> pieces = stackalloc Piece[PieceIndex.PieceListLength];
        Span<byte> squares = stackalloc byte[SquareIndex.SquareListLength];
        Span<byte> attackCheckPinMap = stackalloc byte[AttackDefendPin.AttackCheckPinLength];
        var board = new Board(pieces, squares, attackCheckPinMap);
        var input = "position fen rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        while (Uci.Exec(ref board, input))
        {
            input = Console.ReadLine() ?? string.Empty;
        }
    }
}
