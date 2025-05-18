namespace NuvoChess.BoardState;

public static class MoveGen
{
    private const int _rayDetectionOffset = -1 * (SquareIndex.A8 - SquareIndex.H1);
    private static readonly int[] s_rayDetection =
    [
        17, 0, 0, 0, 0, 0, 0, 16, 0, 0, 0, 0, 0, 0, 15,
        0, 0, 17, 0, 0, 0, 0, 0, 16, 0, 0, 0, 0, 0, 15,
        0, 0, 0, 0, 17, 0, 0, 0, 0, 16, 0, 0, 0, 0, 15,
        0, 0, 0, 0, 0, 0, 17, 0, 0, 0, 16, 0, 0, 0, 15,
        0, 0, 0, 0, 0, 0, 0, 0, 17, 0, 0, 16, 0, 0, 15,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 17, 0, 16, 0, 15,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 17, 16, 15,
        0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0,
        -1, -1, -1, -1, -1, -1, -1, 0, 0, 0, 0, 0, 0, 0, -15,
        -16, -17, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -15,
        0, -16, 0, -17, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -15,
        0, 0, -16, 0, 0, -17, 0, 0, 0, 0, 0, 0, 0, 0, -15,
        0, 0, 0, -16, 0, 0, 0, -17, 0, 0, 0, 0, 0, 0, -15,
        0, 0, 0, 0, -16, 0, 0, 0, 0, -17, 0, 0, 0, 0, -15,
        0, 0, 0, 0, 0, -16, 0, 0, 0, 0, 0, -17, 0, 0, -15,
        0, 0, 0, 0, 0, 0, -16, 0, 0, 0, 0, 0, 0, -17, 0
    ];
    private static readonly byte[] s_pawnStartSquares =
    [
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 32, 32, 32, 32, 32, 32, 32, 32, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 16, 16, 16, 16, 16, 16, 16, 16, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
    ];
    private static readonly byte[] s_pawnPromotionSquares =
    [
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 16, 16, 16, 16, 16, 16, 16, 16, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 32, 32, 32, 32, 32, 32, 32, 32, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
    ];
    private static readonly int[] s_pawnWhiteMoves = [-16, -32, -17, -15];
    private static readonly int[] s_pawnBlackMoves = [16, 32, 17, 15];
    private static readonly int[] s_knightMoves = [-18, -33, -31, -14, 14, 31, 33, 18];
    private static readonly int[] s_bishopMoves = [-17, -15, 15, 17];
    private static readonly int[] s_rookMoves = [-16, -1, 1, 16];
    private static readonly int[] s_queenMoves = [-17, -16, -15, -1, 1, 15, 16, 17];
    private static readonly int[] s_kingMoves = [-17, -16, -15, -1, 1, 15, 16, 17];

    public static Span<Move> GenerateMoves(ref Board board, Span<Move> moveList)
    {
        GenerateAdpMap(ref board);
        var stm = board.Stm;
        var startIndex = PieceIndex.BlackKingIndex;
        var stopIndex = PieceIndex.BlackKingIndex + 16;
        int[] moves;
        bool isSlider;
        if (stm == PieceType.WhitePiece)
        {
            startIndex = PieceIndex.WhiteKingIndex;
            stopIndex = PieceIndex.WhiteKingIndex + 16;
        }

        for (var pieceIndex = startIndex; pieceIndex < stopIndex; pieceIndex++)
        {
            var piece = board.Pieces[pieceIndex];
            var pieceType = piece.PieceType;
            (moves, isSlider) = (pieceType & PieceType.PieceMask) switch
            {
                PieceType.PawnPiece => stm == PieceType.WhitePiece ? (s_pawnWhiteMoves, false) : (s_pawnBlackMoves, false),
                PieceType.KnightPiece => (s_knightMoves, false),
                PieceType.BishopPiece => (s_bishopMoves, true),
                PieceType.RookPiece => (s_rookMoves, true),
                PieceType.QueenPiece => (s_queenMoves, true),
                PieceType.KingPiece => (s_kingMoves, false),
                _ => ([], false)
            };
        }


        return moveList;
    }

    private static void GenerateAdpMap(ref Board board)
    {
        for (var i = 0; i < board.AttackDefendPinMap.Length; i++)
        {
            board.AttackDefendPinMap[i] = 0;
        }
        board.Checks = 0;

        var oppKingIndex = board.Pieces[PieceIndex.BlackKingIndex].SquareIndex;
        var stm = PieceType.WhitePiece;
        var startPieceIndex = PieceIndex.WhiteKingIndex;
        var stopPieceIndex = PieceIndex.WhiteKingIndex + 16;
        int[] moves;
        bool isSlider;
        if (board.Stm == PieceType.WhitePiece)
        {
            oppKingIndex = board.Pieces[PieceIndex.WhiteKingIndex].SquareIndex;
            stm = PieceType.BlackPiece;
            startPieceIndex = PieceIndex.BlackKingIndex;
            stopPieceIndex = PieceIndex.BlackKingIndex + board.BlackPieceCount;
        }

        for (var pieceIndex = startPieceIndex; pieceIndex < stopPieceIndex; pieceIndex++)
        {
            var piece = board.Pieces[pieceIndex];
            var pieceType = piece.PieceType;
            var fromIndex = piece.SquareIndex;
            (moves, isSlider) = (pieceType & PieceType.PieceMask) switch
            {
                PieceType.PawnPiece => stm == PieceType.WhitePiece ? (s_pawnWhiteMoves[2..4], false) : (s_pawnBlackMoves[2..4], false),
                PieceType.KnightPiece => (s_knightMoves, false),
                PieceType.BishopPiece => (s_bishopMoves, true),
                PieceType.RookPiece => (s_rookMoves, true),
                PieceType.QueenPiece => (s_queenMoves, true),
                PieceType.KingPiece => (s_kingMoves, false),
                _ => ([], false)
            };

            if (!isSlider)
            {
                GenerateNonSliderAdpMap(ref board, moves, fromIndex, oppKingIndex);
            }
            else
            {
                GenerateSliderAdpMap(ref board, moves, fromIndex, pieceType, oppKingIndex);
            }
        }
    }

    private static void GenerateNonSliderAdpMap(ref Board board, int[] moves, int fromIndex, int oppKingIndex)
    {
        for (var i = 0; i < moves.Length; i++)
        {
            var toIndex = fromIndex + moves[i];
            var toSquare = board.Squares[toIndex];
            if (toSquare == PieceType.GuardSquare) continue;

            if (toSquare == PieceType.EmptySquare)
            {
                board.AttackDefendPinMap[toIndex] |= AttackCheckPin.Attack;
            }
            else
            {
                board.AttackDefendPinMap[toIndex] |= AttackCheckPin.Attack;
                if (toIndex == oppKingIndex)
                {
                    board.Checks += 1;
                    board.AttackDefendPinMap[fromIndex] |= AttackCheckPin.Defend;
                }
            }
        }
    }

    private static void GenerateSliderAdpMap(ref Board board, int[] moves, int fromIndex, byte pieceType, int oppKingIndex)
    {
        for (var i = 0; i < moves.Length; i++)
        {
            var direction = moves[i];
            var toIndex = fromIndex;
            while (true)
            {
                toIndex += direction;
                var toSquare = board.Squares[toIndex];
                if (toSquare == PieceType.GuardSquare)
                {
                    break;
                }
                else if (toSquare == PieceType.EmptySquare)
                {
                    board.AttackDefendPinMap[toIndex] |= AttackCheckPin.Attack;
                }
                else
                {
                    board.AttackDefendPinMap[toIndex] |= AttackCheckPin.Attack;
                    if (PieceType.IsSameColorPiece(pieceType, board.Pieces[toSquare].PieceType))
                    {
                        break;
                    }
                    else if (toIndex == oppKingIndex)
                    {
                        board.Checks += 1;
                        GenerateDefendSliderAdpMap(ref board, toIndex, fromIndex, direction * -1);
                    }
                    else
                    {
                        GeneratePinSliderAdpMap(ref board, toIndex, direction, oppKingIndex);
                        break;
                    }
                }
            }
        }
    }

    private static void GenerateDefendSliderAdpMap(ref Board board, int toIndex, int fromIndex, int direction)
    {
        var currentIndex = toIndex;
        while (true)
        {
            currentIndex += direction;
            board.AttackDefendPinMap[currentIndex] |= AttackCheckPin.Defend;
            if (currentIndex == fromIndex) break;
        }
    }

    private static void GeneratePinSliderAdpMap(ref Board board, int fromIndex, int direction, int oppKingIndex)
    {
        var possiblePin = s_rayDetection[fromIndex - oppKingIndex + _rayDetectionOffset] == direction;
        if (!possiblePin) return;

        var pinIndex = fromIndex;
        while (true)
        {
            pinIndex += direction;
            var pinSquare = board.Squares[pinIndex];
            if (pinSquare == PieceType.EmptySquare) continue;

            if (pinSquare == PieceType.GuardSquare)
            {
                break;
            }
            else
            {
                if (pinIndex == oppKingIndex)
                {
                    board.AttackDefendPinMap[fromIndex] |= AttackCheckPin.Pin;
                }
                break;
            }
        }
    }
}