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
        for (var i = 0; i < board.AttackPinMap.Length; i++)
        {
            board.AttackPinMap[i] = AttackPinMap.Attack;
        }
        var stm = PieceType.WhitePiece;
        var startIndex = PieceIndex.WhiteKingIndex;
        var stopIndex = PieceIndex.WhiteKingIndex + board.WhitePieceCount;
        int[] moves;
        bool isSlider;
        if (board.Stm == PieceType.WhitePiece)
        {
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
                for (var i = 0; i < moves.Length; i++)
                {
                    var destSquareIndex = squareIndex + moves[i];
                    var destSquare = board.Squares[destSquareIndex];
                    if (destSquare == SquareType.OffBoardSquare)
                    {
                        continue;
                    }
                    else if (destSquare == SquareType.EmptySquare)
                    {
                        board.AttackPinMap[destSquareIndex] = AttackPinMap.Attack;
                    }
                    else
                    {
                        var destPiece = board.Pieces[destSquare].PieceType;
                        if (stm == PieceType.WhitePiece && PieceType.IsBlackPiece(destPiece))
                        {
                            board.AttackPinMap[destSquareIndex] = AttackPinMap.Attack;
                        }
                        else if (stm == PieceType.BlackPiece && PieceType.IsWhitePiece(destPiece))
                        {
                            board.AttackPinMap[destSquareIndex] = AttackPinMap.Attack;
                        }
                    }
                }
            }
            else
            {
                for (var i = 0; i < moves.Length; i++)
                {
                    var direction = moves[i];
                    var destSquareIndex = squareIndex;
                    while (true)
                    {
                        destSquareIndex += (byte)direction;
                        var destSquare = board.Squares[destSquareIndex];
                        if (destSquare == SquareType.OffBoardSquare)
                        {
                            break;
                        }
                        else if (destSquare == SquareType.EmptySquare)
                        {
                            board.AttackPinMap[destSquareIndex] = AttackPinMap.Attack;
                        }
                        else
                        {
                            var destPiece = board.Pieces[destSquare].PieceType;
                            if (stm == PieceType.WhitePiece && PieceType.IsBlackPiece(destPiece))
                            {
                                board.AttackPinMap[destSquareIndex] = AttackPinMap.Attack;
                            }
                            else if (stm == PieceType.BlackPiece && PieceType.IsWhitePiece(destPiece))
                            {
                                board.AttackPinMap[destSquareIndex] = AttackPinMap.Attack;
                            }
                            else
                            {
                                break;
                            }

                            var pinDestSquareIndex = destSquareIndex;
                            while (true)
                            {
                                pinDestSquareIndex += (byte)direction;
                                var pinDestSquare = board.Squares[pinDestSquareIndex];

                                if (pinDestSquare == SquareType.OffBoardSquare)
                                {
                                    break;
                                }
                                else if (pinDestSquare == SquareType.EmptySquare)
                                {
                                    continue;
                                }
                                else
                                {
                                    var pinDestPiece = board.Pieces[pinDestSquare].PieceType;
                                    if (stm == PieceType.WhitePiece && PieceType.IsBlackPiece(pinDestPiece) && PieceType.IsKingPiece(pinDestPiece))
                                    {
                                        board.AttackPinMap[destSquareIndex] = AttackPinMap.Pin;
                                    }
                                    else if (stm == PieceType.BlackPiece && PieceType.IsWhitePiece(pinDestPiece) && PieceType.IsKingPiece(pinDestPiece))
                                    {
                                        board.AttackPinMap[destSquareIndex] = AttackPinMap.Pin;
                                    }
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
            }
        }
    }
}