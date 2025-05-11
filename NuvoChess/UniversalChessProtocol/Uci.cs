using NuvoChess.BoardState;

namespace NuvoChess.UniversalChessProtocol;

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
                if (commandTokens.Length > 2 && commandTokens[1] == "fen")
                {
                    var fen = string.Join(' ', commandTokens.Skip(2));
                    board.SetFromFen(fen);
                }
                else if (commandTokens.Length > 1 && commandTokens[1] == "startpos")
                {
                    board.SetFromFen("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
                }
                else if (commandTokens.Length > 1 && commandTokens[1] == "print")
                {
                    board.PrintBoard();
                }
                else if (commandTokens.Length > 1 && commandTokens[1] == "print_ap_map")
                {
                    MoveGen.GenerateMoves(ref board, []);
                    board.PrintAttackPinMap();
                }
                break;
            default:
                Console.WriteLine($"Unknown command: {command}");
                break;
        }
        return true;
    }
}