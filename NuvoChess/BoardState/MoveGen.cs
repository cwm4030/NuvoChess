namespace NuvoChess.BoardState;

public static class MoveGen
{
    private static readonly int[] _pawnWhiteMoves = [-16, -32, -17, -15];
    private static readonly int[] _pawnBlackMoves = [16, 32, 17, 15];
    private static readonly int[] _knightMoves = [-18, -33, -31, -14, 14, 31, 33, 18];
    private static readonly int[] _bishopMoves = [-17, -15, 15, 17];
    private static readonly int[] _rookMoves = [-16, -1, 1, 16];
    private static readonly int[] _queenMoves = [-17, -16, -15, -1, 1, 15, 16, 17];
    private static readonly int[] _kingMoves = [-17, -16, -15, -1, 1, 15, 16, 17];

    public static Span<Move> GenerateMoves(ref Board board, Span<Move> moves)
    {
        GenerateAttackPinMap(ref board);

        return moves;
    }

    private static void GenerateAttackPinMap(ref Board board)
    {
        board.ClearAttackPinMap();
    }
}