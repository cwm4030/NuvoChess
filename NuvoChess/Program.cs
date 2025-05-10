using NuvoChess.BoardState;

namespace NuvoChess;

public static class Program
{
    public static void Main()
    {
        var board = new Board
        {
            Stm = SideToMove.White,
            Squares = stackalloc byte[256]
        };

        var input = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        while (input != "quit")
        {
            board.SetFromFen(input);
            board.PrintBoard();
            input = Console.ReadLine() ?? string.Empty;
        }
    }
}
