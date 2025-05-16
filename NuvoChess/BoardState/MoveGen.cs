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

    public static Span<Move> GenerateMoves(ref Board board, Span<Move> moveList)
    {
        GenerateAcpMap(ref board);
        var stm = board.Stm;
        var startIndex = PieceIndex.BlackKingIndex;
        var stopIndex = PieceIndex.BlackKingIndex + board.BlackPieceCount;
        int[] moves;
        bool isSlider;
        if (stm == PieceType.WhitePiece)
        {
            startIndex = PieceIndex.WhiteKingIndex;
            stopIndex = PieceIndex.WhiteKingIndex + board.WhitePieceCount;
        }

        for (var pieceIndex = startIndex; pieceIndex < stopIndex; pieceIndex++)
        {
            var piece = board.Pieces[pieceIndex];
            var pieceType = piece.PieceType;
            (moves, isSlider) = (pieceType & PieceType.PieceMask) switch
            {
                PieceType.PawnPiece => stm == PieceType.WhitePiece ? (_pawnWhiteMoves[2..4], false) : (_pawnBlackMoves[2..4], false),
                PieceType.KnightPiece => (_knightMoves, false),
                PieceType.BishopPiece => (_bishopMoves, true),
                PieceType.RookPiece => (_rookMoves, true),
                PieceType.QueenPiece => (_queenMoves, true),
                PieceType.KingPiece => (_kingMoves, false),
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
        var stopIndex = PieceIndex.WhiteKingIndex + board.WhitePieceCount;
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
                PieceType.PawnPiece => stm == PieceType.WhitePiece ? (_pawnWhiteMoves[2..4], false) : (_pawnBlackMoves[2..4], false),
                PieceType.KnightPiece => (_knightMoves, false),
                PieceType.BishopPiece => (_bishopMoves, true),
                PieceType.RookPiece => (_rookMoves, true),
                PieceType.QueenPiece => (_queenMoves, true),
                PieceType.KingPiece => (_kingMoves, false),
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
                if (destSquare == oppKingIndex)
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