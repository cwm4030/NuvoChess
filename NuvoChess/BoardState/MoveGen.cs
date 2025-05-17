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
        GenerateAcpMap(ref board);
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
                PieceType.PawnPiece => stm == PieceType.WhitePiece ? (s_pawnWhiteMoves[2..4], false) : (s_pawnBlackMoves[2..4], false),
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

    private static void GenerateAcpMap(ref Board board)
    {
        for (var i = 0; i < board.AttackCheckPinMap.Length; i++)
        {
            board.AttackCheckPinMap[i] = 0;
        }
        board.Checks = 0;

        var oppKingIndex = board.Pieces[PieceIndex.BlackKingIndex].SquareIndex;
        var stm = PieceType.WhitePiece;
        var startIndex = PieceIndex.WhiteKingIndex;
        var stopIndex = PieceIndex.WhiteKingIndex + 16;
        int[] moves;
        bool isSlider;
        if (board.Stm == PieceType.WhitePiece)
        {
            oppKingIndex = board.Pieces[PieceIndex.WhiteKingIndex].SquareIndex;
            stm = PieceType.BlackPiece;
            startIndex = PieceIndex.BlackKingIndex;
            stopIndex = PieceIndex.BlackKingIndex + board.BlackPieceCount;
        }

        for (var pieceIndex = startIndex; pieceIndex < stopIndex; pieceIndex++)
        {
            var piece = board.Pieces[pieceIndex];
            var pieceType = piece.PieceType;
            var squareIndex = piece.SquareIndex;
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
                GenerateNonSliderAcpMap(ref board, moves, squareIndex, oppKingIndex);
            }
            else
            {
                GenerateSliderAcpMap(ref board, moves, squareIndex, pieceType, oppKingIndex);
            }
        }
    }

    private static void GenerateNonSliderAcpMap(ref Board board, int[] moves, int squareIndex, int oppKingIndex)
    {
        for (var i = 0; i < moves.Length; i++)
        {
            var destSquareIndex = squareIndex + moves[i];
            var destSquare = board.Squares[destSquareIndex];
            if (destSquare == PieceType.GuardSquare) continue;

            if (destSquare == PieceType.EmptySquare)
            {
                board.AttackCheckPinMap[destSquareIndex] |= AttackCheckPin.Attack;
            }
            else
            {
                board.AttackCheckPinMap[destSquareIndex] |= AttackCheckPin.Attack;
                if (destSquareIndex == oppKingIndex)
                {
                    board.Checks += 1;
                    board.AttackCheckPinMap[squareIndex] |= AttackCheckPin.Check;
                }
            }
        }
    }

    private static void GenerateSliderAcpMap(ref Board board, int[] moves, int squareIndex, byte pieceType, int oppKingIndex)
    {
        for (var i = 0; i < moves.Length; i++)
        {
            var direction = moves[i];
            var destSquareIndex = squareIndex;
            while (true)
            {
                destSquareIndex += direction;
                var destSquare = board.Squares[destSquareIndex];
                if (destSquare == PieceType.GuardSquare)
                {
                    break;
                }
                else if (destSquare == PieceType.EmptySquare)
                {
                    board.AttackCheckPinMap[destSquareIndex] |= AttackCheckPin.Attack;
                }
                else
                {
                    board.AttackCheckPinMap[destSquareIndex] |= AttackCheckPin.Attack;
                    if (PieceType.IsSameColorPiece(pieceType, board.Pieces[destSquare].PieceType))
                    {
                        break;
                    }
                    else if (destSquareIndex == oppKingIndex)
                    {
                        board.Checks += 1;
                        board.AttackCheckPinMap[squareIndex] |= AttackCheckPin.Check;
                        break;
                    }
                    else
                    {
                        GeneratePinSliderAcpMap(ref board, destSquareIndex, direction, oppKingIndex);
                        break;
                    }
                }
            }
        }
    }

    public static void GeneratePinSliderAcpMap(ref Board board, int destSquareIndex, int direction, int oppKingIndex)
    {
        var possiblePin = s_rayDetection[destSquareIndex - oppKingIndex + _rayDetectionOffset] == direction;
        if (!possiblePin) return;

        var pinDestSquareIndex = destSquareIndex;
        while (true)
        {
            pinDestSquareIndex += direction;
            var pinDestSquare = board.Squares[pinDestSquareIndex];
            if (pinDestSquare == PieceType.EmptySquare) continue;

            if (pinDestSquare == PieceType.GuardSquare)
            {
                break;
            }
            else
            {
                if (pinDestSquareIndex == oppKingIndex)
                {
                    board.AttackCheckPinMap[destSquareIndex] |= AttackCheckPin.Pin;
                }
                break;
            }
        }
    }
}