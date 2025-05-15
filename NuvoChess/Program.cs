using NuvoChess.BoardState;
using NuvoChess.UniversalChessInterface;

namespace NuvoChess;

public static class Program
{
    public static void Main()
    {
        var board = new Board
        {
            Pieces = stackalloc Piece[PieceIndex.PieceListLength],
            Squares = stackalloc byte[SquareIndex.SquareListLength],
            AttackCheckPinMap = stackalloc byte[AttackCheckPin.AttackCheckPinLength]
        };

        var input = "position fen rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        while (Uci.Exec(ref board, input))
        {
            input = Console.ReadLine() ?? string.Empty;
        }
    }
}
