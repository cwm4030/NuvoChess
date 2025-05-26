namespace NuvoChess.BoardState;

public static class MoveGen
{
    private const int RayDetectionOffset = -1 * (SquareIndex.A8 - SquareIndex.H1);
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

    public static void GenerateMoves(ref Board board, ref Span<Move> moveList, ref int moveIndex)
    {
        GenerateAdpMap(ref board);
        var startPieceIndex = board.Stm == PieceType.WhitePiece ? PieceIndex.WhiteKingIndex : PieceIndex.BlackKingIndex;
        var stopPieceIndex = startPieceIndex + 16;

        for (var pieceIndex = startPieceIndex; pieceIndex < stopPieceIndex; pieceIndex++)
        {
            var piece = board.Pieces[pieceIndex];
            var pieceType = piece.PieceType;
            var fromIndex = piece.SquareIndex;
            if (PieceType.IsCapturedPiece(pieceType)) continue;
            
            var (moves, isSlider) = (pieceType & PieceType.PieceMask) switch
            {
                PieceType.PawnPiece => board.Stm == PieceType.WhitePiece ? (s_pawnWhiteMoves, false) : (s_pawnBlackMoves, false),
                PieceType.KnightPiece => (s_knightMoves, false),
                PieceType.BishopPiece => (s_bishopMoves, true),
                PieceType.RookPiece => (s_rookMoves, true),
                PieceType.QueenPiece => (s_queenMoves, true),
                PieceType.KingPiece => (s_kingMoves, false),
                _ => ([], false)
            };

            if (PieceType.IsPawnPiece(pieceType))
            {
                GeneratePawnMoves(ref board, ref moveList, ref moveIndex, fromIndex, moves);
            }
            else if (!isSlider)
            {
                GenerateNonSliderMoves(ref board, ref moveList, ref moveIndex, fromIndex, moves);
            }
            else
            {
                GenerateSliderMoves(ref board, ref moveList, ref moveIndex, fromIndex, moves);
            }
        }
        GenerateCastleMoves(ref board, ref moveList, ref moveIndex);
    }
    
    private static void GeneratePawnMoves(ref Board board, ref Span<Move> moveList, ref int moveIndex, int fromIndex, int[] moves)
    {
        var upOneIndex = fromIndex + moves[0];
        var upTwoIndex = fromIndex + moves[1];
        var isPromotion = s_pawnPromotionSquares[upOneIndex] == board.Stm;
        for (var i = 2; i < moves.Length; i++)
        {
            var captureIndex = fromIndex + moves[i];
            var captureSquare = board.Squares[captureIndex];
            var capturePiece = board.Pieces[captureSquare];
            if ((PieceType.IsEmptySquare(board.Squares[captureSquare]) && captureSquare != board.EpSquare)
                || PieceType.IsGuardSquare(captureSquare)
                || PieceType.IsSameColorPiece(board.Stm, capturePiece.PieceType))
                continue;
            AddMove(ref board, ref moveList, ref moveIndex, fromIndex, captureIndex);
        }

        if (!PieceType.IsEmptySquare(board.Squares[upOneIndex])) return;
        if (isPromotion)
        {
            for (var promotionPiece = PieceType.PawnPiece; promotionPiece <= PieceType.QueenPiece; promotionPiece += 2)
            {
                AddMove(ref board, ref moveList, ref moveIndex, fromIndex, upOneIndex, promotionPiece);
            }
        }
        else
        {
            AddMove(ref board, ref moveList, ref moveIndex, fromIndex, upOneIndex);
        }

        if (s_pawnStartSquares[fromIndex] == board.Stm && PieceType.IsEmptySquare(board.Squares[upTwoIndex]))
        {
            AddMove(ref board, ref moveList, ref moveIndex, fromIndex, upTwoIndex);
        }
    }
    
    private static void GenerateNonSliderMoves(ref Board board, ref Span<Move> moveList, ref int moveIndex, int fromIndex, int[] moves)
    {
        foreach (var direction in moves)
        {
            var toIndex = fromIndex + direction;
            AddMove(ref board, ref moveList, ref moveIndex, fromIndex, toIndex);
        }
    }

    private static void GenerateSliderMoves(ref Board board, ref Span<Move> moveList, ref int moveIndex, int fromIndex, int[] moves)
    {
        foreach (var direction in moves)
        {
            var toIndex = fromIndex;
            while (true)
            {
                toIndex += direction;
                var toSquare = board.Squares[toIndex];
                if (PieceType.IsGuardSquare(toSquare))
                {
                    break;
                }
                else if (PieceType.IsEmptySquare(toSquare))
                {
                    AddMove(ref board, ref moveList, ref moveIndex, fromIndex, toIndex);
                }
                else
                {
                    AddMove(ref board, ref moveList, ref moveIndex, fromIndex, toIndex);
                    break;
                }
            }
        }
    }

    private static void GenerateCastleMoves(ref Board board, ref Span<Move> moveList, ref int moveIndex)
    {
        if (board.Checks > 0) return;
        var fromIndex = board.Stm == PieceType.WhitePiece ? SquareIndex.E1 : SquareIndex.E8;
        if (board.Stm == PieceType.WhitePiece
            && (board.CastleRights & Castling.WhiteKing) == Castling.WhiteKing
            && board.AttackDefendPinMap[SquareIndex.F1] == 0
            && board.AttackDefendPinMap[SquareIndex.G1] == 0)
        {
            AddMove(ref board, ref moveList, ref moveIndex, fromIndex, SquareIndex.G1);
        }
        if (board.Stm == PieceType.WhitePiece
            && (board.CastleRights & Castling.WhiteQueen) == Castling.WhiteQueen
            && board.AttackDefendPinMap[SquareIndex.D1] == 0
            && board.AttackDefendPinMap[SquareIndex.C1] == 0
            && board.AttackDefendPinMap[SquareIndex.B1] == 0)
        {
            AddMove(ref board, ref moveList, ref moveIndex, fromIndex, SquareIndex.C1);
        }
        if (board.Stm == PieceType.BlackPiece
            && (board.CastleRights & Castling.BlackKing) == Castling.BlackKing
            && board.AttackDefendPinMap[SquareIndex.F8] == 0
            && board.AttackDefendPinMap[SquareIndex.G8] == 0)
        {
            AddMove(ref board, ref moveList, ref moveIndex, fromIndex, SquareIndex.G8);
        }
        if (board.Stm == PieceType.BlackPiece
            && (board.CastleRights & Castling.BlackQueen) == Castling.BlackQueen
            && board.AttackDefendPinMap[SquareIndex.D8] == 0
            && board.AttackDefendPinMap[SquareIndex.C8] == 0
            && board.AttackDefendPinMap[SquareIndex.B8] == 0)
        {
            AddMove(ref board, ref moveList, ref moveIndex, fromIndex, SquareIndex.C8);
        }
    }
    
    private static void AddMove(ref Board board, ref Span<Move> moveList, ref int moveIndex, int fromIndex, int toIndex, int promotionPiece = 0)
    {
        var fromSquare = board.Squares[fromIndex];
        var toSquare = board.Squares[toIndex];
        var fromPiece = board.Pieces[fromSquare];
        var toPiece = board.Pieces[toSquare];
        if (PieceType.IsGuardSquare(toSquare)
            || PieceType.IsSameColorPiece(fromPiece.PieceType, toPiece.PieceType)
            || board.AttackDefendPinMap[fromIndex] == AttackDefendPin.Pin
            || (PieceType.IsPawnPiece(fromPiece.PieceType) && board.AttackDefendPinMap[toIndex] == AttackDefendPin.EpPin)
            || (board.Checks > 1 && !PieceType.IsKingPiece(fromPiece.PieceType))
            || (PieceType.IsKingPiece(fromPiece.PieceType) && board.AttackDefendPinMap[toIndex] != 0)
            || (board.Checks == 1 && !PieceType.IsKingPiece(fromPiece.PieceType) && board.AttackDefendPinMap[toIndex] != AttackDefendPin.Defend))
            return;

        moveList[moveIndex].FromSquare = fromIndex;
        moveList[moveIndex].ToSquare = toIndex;
        moveList[moveIndex].PromotionPiece = promotionPiece;
        moveIndex += 1;
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
            var (moves, isSlider) = (pieceType & PieceType.PieceMask) switch
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
                GenerateSliderAdpMap(ref board, moves, fromIndex, oppKingIndex);
            }
        }
    }

    private static void GenerateNonSliderAdpMap(ref Board board, int[] moves, int fromIndex, int oppKingIndex)
    {
        foreach (var t in moves)
        {
            var toIndex = fromIndex + t;
            var toSquare = board.Squares[toIndex];
            if (PieceType.IsGuardSquare(toSquare)) continue;

            if (PieceType.IsEmptySquare(toSquare))
            {
                board.AttackDefendPinMap[toIndex] |= AttackDefendPin.Attack;
            }
            else
            {
                board.AttackDefendPinMap[toIndex] |= AttackDefendPin.Attack;
                if (toIndex != oppKingIndex) continue;
                board.Checks += 1;
                board.AttackDefendPinMap[fromIndex] |= AttackDefendPin.Defend;
            }
        }
    }

    private static void GenerateSliderAdpMap(ref Board board, int[] moves, int fromIndex, int oppKingIndex)
    {
        var fromSquare = board.Squares[fromIndex];
        foreach (var direction in moves)
        {
            var toIndex = fromIndex;
            while (true)
            {
                toIndex += direction;
                var toSquare = board.Squares[toIndex];
                if (PieceType.IsGuardSquare(toSquare))
                {
                    break;
                }
                else if (PieceType.IsEmptySquare(toSquare))
                {
                    board.AttackDefendPinMap[toIndex] |= AttackDefendPin.Attack;
                }
                else
                {
                    board.AttackDefendPinMap[toIndex] |= AttackDefendPin.Attack;
                    var isEpPin= IsPossibleEpPin(ref board, toIndex, toIndex + direction, direction);
                    if (isEpPin)
                    {
                        GeneratePinSliderAdpMap(ref board, board.EpSquare, toIndex + direction, direction, oppKingIndex, AttackDefendPin.EpPin);
                        break;
                    }
                    else if (PieceType.IsSameColorPiece(board.Pieces[fromSquare].PieceType, board.Pieces[toSquare].PieceType))
                    {
                        break;
                    }
                    else if (toIndex == oppKingIndex)
                    {
                        board.Checks += 1;
                        GenerateDefendSliderAdpMap(ref board, toIndex, fromIndex, direction * -1);
                        break;
                    }
                    else
                    {
                        GeneratePinSliderAdpMap(ref board, toIndex, toIndex, direction, oppKingIndex, AttackDefendPin.Pin);
                        break;
                    }
                }
            }
        }
    }

    private static bool IsPossibleEpPin(ref Board board, int squareIndex1, int squareIndex2, int direction)
    {
        var square1 = board.Squares[squareIndex1];
        var square2 = board.Squares[squareIndex2];
        return direction is -1 or 1
               && board.EpPawnSquare != 0
               && (squareIndex1 == board.EpPawnSquare || squareIndex2 == board.EpPawnSquare)
               && PieceType.IsPawnPiece(board.Pieces[square1].PieceType)
               && PieceType.IsPawnPiece(board.Pieces[square2].PieceType);
    }

    private static void GenerateDefendSliderAdpMap(ref Board board, int toIndex, int fromIndex, int direction)
    {
        var currentIndex = toIndex;
        while (true)
        {
            currentIndex += direction;
            board.AttackDefendPinMap[currentIndex] |= AttackDefendPin.Defend;
            if (currentIndex == fromIndex) break;
        }
    }

    private static void GeneratePinSliderAdpMap(ref Board board, int pinIndex, int fromIndex, int direction, int oppKingIndex, byte pinType)
    {
        var possiblePin = s_rayDetection[fromIndex - oppKingIndex + RayDetectionOffset] == direction;
        if (!possiblePin) return;
        
        while (true)
        {
            fromIndex += direction;
            var fromSquare = board.Squares[fromIndex];
            if (PieceType.IsEmptySquare(fromSquare)) continue;

            if (PieceType.IsGuardSquare(fromSquare))
            {
                break;
            }
            else
            {
                if (fromIndex == oppKingIndex)
                {
                    board.AttackDefendPinMap[pinIndex] |= pinType;
                }
                break;
            }
        }
    }
}