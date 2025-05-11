using NuvoChess.BoardState;
using NuvoChess.UniversalChessProtocol;

namespace NuvoChess;

public static class Program
{
    public static void Main()
    {
        var board = new Board
        {
            Pieces = stackalloc Piece[PieceIndex.PieceListLength],
            Squares = stackalloc byte[SquareIndex.SquareListLength]
        };
        board.SetFromFen("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");

        while (Uci.Exec(ref board, Console.ReadLine() ?? string.Empty))
        {
        }
    }
}
