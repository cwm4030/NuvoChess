using NuvoChess.BoardState;

namespace NuvoChess.UniversalChessInterface;

public static class Uci
{
    public static bool Exec(ref Board board, string command)
    {
        var commandTokens = command.Split(' ').Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
        if (commandTokens.Length == 0) return true;

        var initalCommand = commandTokens[0].ToLower();
        switch (initalCommand)
        {
            case "uci":
                Console.WriteLine("id name NuvoChess");
                Console.WriteLine("id author Caden Miller");
                Console.WriteLine("uciok");
                break;
            case "isready":
                Console.WriteLine("readyok");
                break;
            case "quit":
                return false;
            case "position":
                switch (commandTokens.Length)
                {
                    case > 2 when commandTokens[1] == "fen":
                    {
                        var fen = string.Join(' ', commandTokens.Skip(2));
                        board.SetFromFen(fen);
                        break;
                    }
                    case > 1 when commandTokens[1] == "startpos":
                        board.SetFromFen("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
                        break;
                    case > 1 when commandTokens[1] == "print":
                        board.PrintBoard();
                        break;
                    case > 1 when commandTokens[1] == "print_simple":
                        board.PrintSimpleBoard();
                        break;
                    case > 1 when commandTokens[1] == "print_adp_map":
                        Span<Move> moveList = new Move[256];
                        var moveIndex = 0;
                        MoveGen.GenerateMoves(ref board, ref moveList, ref moveIndex);
                        board.PrintAttackDefendPinMap();
                        break;
                }
                break;
            default:
                Console.WriteLine($"Unknown command: {command}");
                break;
        }
        return true;
    }
}