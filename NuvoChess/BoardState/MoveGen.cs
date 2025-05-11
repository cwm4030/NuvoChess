namespace NuvoChess.BoardState;

public static class MoveGen
{
    private static int[] _pawnWhiteMoves = [-16, -32, -17, -15];
    private static int[] _pawnBlackMoves = [16, 32, 17, 15];
    private static int[] _knightMoves = [-18, -33, -31, -14, 14, 31, 33, 18];
    private static int[] _bishopMoves = [-17, -15, 15, 17];
    private static int[] _rookMoves = [-16, -1, 1, 16];
    private static int[] _queenMoves = [-17, -16, -15, -1, 1, 15, 16, 17];
    private static int[] _kingMoves = [-17, -16, -15, -1, 1, 15, 16, 17];
    private const byte _attacked = 1;
    private const byte _pinned = 1;

    public static Span<Move> GenerateMoves(ref Board board, Span<Move> moves)
    {
        Span<byte> apMap = stackalloc byte[256];
        return moves;
    }
}